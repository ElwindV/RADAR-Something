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
    private float timerShoot;
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
    private float gunPickupRange = 0.5f;
    private GameManager gameManager;

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
        if (Input.GetKey("escape")) 
        {
            Application.Quit();
        }

        shownHitPoints = Mathf.Lerp(shownHitPoints, hitPoints, 10f * Time.deltaTime);
        if(healthBar != null) healthBar.fillAmount = Mathf.Clamp(shownHitPoints, 0, maxHitPoints) / maxHitPoints;
        
        timerShoot -= Time.deltaTime;
        Vector2 input = new Vector2(Input.GetAxisRaw("RightJoyX"), Input.GetAxisRaw("RightJoyY"));
        //Debug.Log(input.ToString());

        if (timerShoot <= 0)
        {
            if (Input.GetButton("Fire1") && hasGun)
            {
                GameObject bullet = Instantiate(bulletPrefabs[0]) as GameObject;
                audioSource.PlayOneShot(sounds[0]);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                timerShoot = shootDelay;
                bulletScript.Init(bulletSpawnPoint.position, bulletSpawnPoint.forward);
                Destroy(bullet, 5f);
            }

            if (Input.GetButton("Fire2"))
            {
                audioSource.PlayOneShot(sounds[1]);
                GameObject bullet = Instantiate(bulletPrefabs[1]) as GameObject;
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                timerShoot = shootDelay;
                bulletScript.Init(bulletSpawnPoint.position, bulletSpawnPoint.forward);
                Destroy(bullet, 5f);
            }
        }

        if (Vector2.Distance(Vector2.zero, input) > .1f)
        {
            currentRotation = transform.rotation;
            if (input.x > 0)
            {
                targetRotation = Quaternion.Euler(0, Vector2.Angle(Vector2.down, input), 0);
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.up, input), 0);
            }

            if(currentRotation != targetRotation)
            {
                transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }

        if(transform.position.x < gunPosition.x + gunPickupRange &&
           transform.position.x > gunPosition.x - gunPickupRange &&
           transform.position.z < gunPosition.z + gunPickupRange &&
           transform.position.z > gunPosition.z - gunPickupRange && !hasGun) 
        {
            hasGun = true;
            Destroy(gun);
            audioSource.PlayOneShot(sounds[2]);
        }
    
    }
    public void FixedUpdate()
    {
       // Debug.Log(new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")).ToString());
        myRigidbody.velocity = new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")) * speed * 50f * Time.fixedDeltaTime;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            if (gameManager != null)
            {
                gameManager.endGame();
            }
        }
    }

    public void RestoreHealth()
    {
        hitPoints = maxHitPoints;
    }
}
