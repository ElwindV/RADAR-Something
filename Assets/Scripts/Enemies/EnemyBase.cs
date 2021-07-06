using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour {
        protected const int ModeMoving = 1;
        private const int ModeStunned = 0;

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

        private GameManager _gameManager;

        public AudioClip[] sounds;
        protected AudioSource audioSource;

        [System.NonSerialized] public Rigidbody rb;

        public virtual void Start() {

            hitPoints = startHP;
            mode = ModeMoving;
            rend = GetComponent<Renderer>();
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player");

            x = transform.position.x;
            z = transform.position.z;

            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        public void Update() {
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
            if (col.transform.CompareTag("Player") && mode != ModeStunned)
            {
                fadeIn();

                mode = ModeStunned;
                StartCoroutine(Invis(true));
                col.transform.GetComponent<Player>().TakeDamage(2);
                _gameManager.resetMultiplier();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("beam"))
            {
                StartCoroutine(Invis());
            }
        }

        private IEnumerator Invis()
        {
            yield return new WaitForSeconds(2);

            fadeOut();
        }
        private IEnumerator Invis(bool stun)
        {
            yield return new WaitForSeconds(2);
            mode = ModeMoving;
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
                _gameManager.addScore(1);
                Destroy(transform.gameObject);
            }
            else
            {
                audioSource.PlayOneShot(sounds[0]);
            }
        }

        protected void ClampPosition() 
        {
            var x = Mathf.Clamp(transform.position.x, -transform.parent.position.x, 32 - transform.parent.position.x);
            var z = Mathf.Clamp(transform.position.z, -transform.parent.position.z, 32 - transform.parent.position.z);
            transform.position = new Vector3(x, 1, z);

            //Vector3 worldPosition = transform.TransformVector(transform.position);
            //worldPosition.x = Mathf.Clamp(worldPosition.x, 0, 32);
            //worldPosition.z = Mathf.Clamp(worldPosition.z, 0, 32);
            //worldPosition.y = 1.5f;
            //transform.position = transform.InverseTransformPoint(worldPosition);
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
}
