using Audio;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Range(1, 20)] public float speed = 5f;
    [Range(1, 20)] public float turnSpeed = 5f;

    [SerializeField]
    private Transform bulletSpawnPoint;
    [Range(0,10)] public float shootDelay;
    public float maxHitPoints;
    public float hitPoints;
    private float _shownHitPoints;
    public Image healthBar;
    public GameObject[] bulletPrefabs;

    [Header("Audio")] 
    public AudioCueEventChannelSO eventChannelSO;
    public AudioCueSO shootSfx;
    public AudioCueSO waveSfx;
    public AudioCueSO pickupSfx;
    
    // private Quaternion _currentRotation;
    // private Quaternion _targetRotation;
    private Rigidbody _rigidbody;

    private GameManager _gameManager;
    private float _lastFireTime = float.NegativeInfinity;
    private Camera _camera;

    public void Start()
    {
        _camera = Camera.main;
        _shownHitPoints = hitPoints = maxHitPoints;
        _rigidbody = GetComponent<Rigidbody>();

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Update ()
    {
        _shownHitPoints = Mathf.Lerp(_shownHitPoints, hitPoints, 10f * Time.deltaTime);
        if(healthBar != null) healthBar.fillAmount = Mathf.Clamp(_shownHitPoints, 0, maxHitPoints) / maxHitPoints;
    }

    public void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            _gameManager.ExitGame();

        _rigidbody.velocity = new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")) * (speed * 50f * Time.fixedDeltaTime);

        if (Input.GetButton("Fire1") || Input.GetMouseButton(0))
            ShootGun();

        if (Input.GetButton("Fire2") || Input.GetMouseButton(1))
            ShootWave();

        HandleRotation(new Vector2(Input.GetAxisRaw("RightJoyX"), Input.GetAxisRaw("RightJoyY")));
    }

    private void HandleRotation(Vector2 input)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;
        
        var targetDirection = hit.point;
        
        var position = transform.position;
        targetDirection.y = position.y;
        targetDirection -= position;

        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * turnSpeed, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);

        // if (Vector2.Distance(Vector2.zero, input) <= .1f)
        //     return;

        // _currentRotation = transform.rotation;
        // _targetRotation = (input.x > 0)
        //     ? Quaternion.Euler(0, Vector2.Angle(Vector2.down, input), 0)
        //     : Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.up, input), 0);

        // if (_currentRotation != _targetRotation)
        //     transform.rotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * turnSpeed);
    }
    
    private void ShootGun() 
    {
        if (! CanFire()) {
            return;
        }
        var bullet = Instantiate(bulletPrefabs[0]);
        eventChannelSO.RaiseEvent(shootSfx);
        var bulletScript = bullet.GetComponent<Bullet>();
        _lastFireTime = Time.time;
        bulletScript.Init(bulletSpawnPoint.position, bulletSpawnPoint.forward);
        Destroy(bullet, 5f);
    }

    private void ShootWave() 
    {
        if (! CanFire()) {
            return;
        }
        eventChannelSO.RaiseEvent(waveSfx);
        var bullet = Instantiate(bulletPrefabs[1]);
        var bulletScript = bullet.GetComponent<Bullet>();
        _lastFireTime = Time.time;
        bulletScript.Init(bulletSpawnPoint.position, bulletSpawnPoint.forward);
        Destroy(bullet, 5f);
    }

    private bool CanFire() 
    {
        return (_lastFireTime + shootDelay) < Time.time;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0 && _gameManager != null) {
            _gameManager.EndGame();
        }
    }

    public void RestoreHealth()
    {
        hitPoints = maxHitPoints;
    }
}
