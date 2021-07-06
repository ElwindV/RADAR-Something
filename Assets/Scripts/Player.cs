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
    public AudioSource audioSource;
    public AudioClip[] sounds;
    private Quaternion _currentRotation;
    private Quaternion _targetRotation;
    private Rigidbody _myRigidbody;

    private bool _hasGun;
    private GameObject _gun;
    private Vector3 _gunPosition;
    private const float GunPickupRange = 1.5f;
    private GameManager _gameManager;
    private float _lastFireTime = float.NegativeInfinity;

    public void Start()
    {
        _shownHitPoints = hitPoints = maxHitPoints;
        _myRigidbody = GetComponent<Rigidbody>();
        _gun = GameObject.Find("Gun");
        _gunPosition = _gun.transform.position;

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
            _gameManager.exitGame();

        _myRigidbody.velocity = new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")) * speed * 50f * Time.fixedDeltaTime;

        if (Input.GetButton("Fire1"))
            ShootGun();

        if (Input.GetButton("Fire2"))
            ShootWave();

        HandleRotation(new Vector2(Input.GetAxisRaw("RightJoyX"), Input.GetAxisRaw("RightJoyY")));

        HandlePickup();
    }

    private void HandleRotation(Vector2 input) 
    {
        if (Vector2.Distance(Vector2.zero, input) <= .1f)
            return;

        _currentRotation = transform.rotation;
        _targetRotation = (input.x > 0)
            ? Quaternion.Euler(0, Vector2.Angle(Vector2.down, input), 0)
            : Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.up, input), 0);

        if (_currentRotation != _targetRotation) {
            transform.rotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * turnSpeed);
        } 
    }

    private void ShootGun() 
    {
        if (! _hasGun) {
            return;
        }

        if (! CanFire()) {
            return;
        }
        var bullet = Instantiate(bulletPrefabs[0]);
        audioSource.PlayOneShot(sounds[0]);
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
        audioSource.PlayOneShot(sounds[1]);
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
            _gameManager.endGame();
        }
    }

    public void RestoreHealth()
    {
        hitPoints = maxHitPoints;
    }

    private void HandlePickup() 
    {
        if (_hasGun)
            return;

        if (Vector3.Distance(transform.position, _gunPosition) >= GunPickupRange) {
            return;
        }

        _hasGun = true;
        Destroy(_gun);
        audioSource.PlayOneShot(sounds[2]);
    }
}
