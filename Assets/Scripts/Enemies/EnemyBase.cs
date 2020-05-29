using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    public const int MODE_MOVING = 1; 
    public const int MODE_STUNNED = 0;

    public int mode;
    public float startHP;
    public float hitPoints;

    [System.NonSerialized] public GameObject player;
    protected Renderer rend;

    public GameObject[] explosions;

    private Color col1, col2;

    private float shadeTime = 0f;
    private bool trans = true;

    protected float x, z;

    private GameManager gameManager;

    public AudioClip[] sounds;
    protected AudioSource audioSource;

    [System.NonSerialized] public Rigidbody rb;

    public virtual void Start () {
     
        hitPoints = startHP;
        mode = MODE_MOVING;
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        x = transform.position.x;
        z = transform.position.z;
        
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
	
	public void Update () {
        shadeTime += ((!trans) ? 1f : -1f) * Time.deltaTime;
        shadeTime = Mathf.Clamp(shadeTime, 0, 255);
        rend.material.color = new Vector4(rend.material.color.r, rend.material.color.b, rend.material.color.g, shadeTime);

        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(-relativePos);
        transform.rotation = rotation;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("beam"))
        {
            fadeIn();
        }
    }
    public void OnCollisionStay(Collision col)
    {
        if (col.transform.CompareTag("Player") && mode != MODE_STUNNED)
        {
            fadeIn();

            mode = MODE_STUNNED;
            StartCoroutine(Invis(true));
            col.transform.GetComponent<Player>().TakeDamage(2);
            gameManager.resetMultiplier();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("beam"))
        {
            StartCoroutine(Invis()); 
        }
    }

    IEnumerator Invis()
    {
        yield return new WaitForSeconds(2);

        fadeOut();
    }
    IEnumerator Invis(bool stun)
    {
        yield return new WaitForSeconds(2);
        mode = MODE_MOVING;
        fadeOut();
    }
    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        audioSource.PlayOneShot(sounds[0]);
        if (hitPoints <= 0)
        {
            if (explosions.Length > 0)
            {
                Instantiate(explosions[Random.Range(0, explosions.Length)]).transform.position = transform.position;
            }
            audioSource.PlayOneShot(sounds[1]);
            gameManager.addScore(1);
            Destroy(transform.gameObject);
        }
        else
        {
            audioSource.PlayOneShot(sounds[0]);
        }
    }

    public void fadeOut()
    {
        trans = true;
    }

    public void fadeIn()
    {
        trans = false;
    }

}
