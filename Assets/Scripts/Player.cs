using System.Collections;
using System.Collections.Generic;
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
    private float shownHitPoints;
    public Image healthBar;
    public GameObject[] bulletPrefabs;
    public AudioSource audioSource;
    public AudioClip[] sounds;
    private Quaternion currentRotation;
    private Quaternion targetRotation;
    private Rigidbody myRigidbody;

    private bool hasGun = false;
    private GameObject gun;
    private Vector3 gunPosition;
    private float gunPickupRange = 1.5f;
    private GameManager gameManager;
    private float lastFireTime = float.NegativeInfinity;

    public void Start()
    {
        shownHitPoints = hitPoints = maxHitPoints;
        myRigidbody = GetComponent<Rigidbody>();
        gun = GameObject.Find("Gun");
        gunPosition = gun.transform.position;

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Update ()
    {
        shownHitPoints = Mathf.Lerp(shownHitPoints, hitPoints, 10f * Time.deltaTime);
        if(healthBar != null) healthBar.fillAmount = Mathf.Clamp(shownHitPoints, 0, maxHitPoints) / maxHitPoints;
    }

    public void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            gameManager.exitGame();

        myRigidbody.velocity = new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")) * speed * 50f * Time.fixedDeltaTime;

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

        currentRotation = transform.rotation;
        targetRotation = (input.x > 0)
            ? Quaternion.Euler(0, Vector2.Angle(Vector2.down, input), 0)
            : Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.up, input), 0);

        if (currentRotation != targetRotation) {
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * turnSpeed);
        } 
    }

    private void ShootGun() 
    {
        if (! hasGun) {
            return;
        }

        if (! CanFire()) {
            return;
        }
        var bullet = Instantiate(bulletPrefabs[0]);
        audioSource.PlayOneShot(sounds[0]);
        var bulletScript = bullet.GetComponent<Bullet>();
        lastFireTime = Time.time;
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
        lastFireTime = Time.time;
        bulletScript.Init(bulletSpawnPoint.position, bulletSpawnPoint.forward);
        Destroy(bullet, 5f);
    }

    private bool CanFire() 
    {
        return (lastFireTime + shootDelay) < Time.time;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0 && gameManager != null) {
            gameManager.endGame();
        }
    }

    public void RestoreHealth()
    {
        hitPoints = maxHitPoints;
    }

    private void HandlePickup() 
    {
        if (hasGun)
            return;

        if (Vector3.Distance(transform.position, gunPosition) >= gunPickupRange) {
            return;
        }

        hasGun = true;
        Destroy(gun);
        audioSource.PlayOneShot(sounds[2]);
    }
}
