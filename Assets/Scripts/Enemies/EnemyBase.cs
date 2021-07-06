using System.Collections;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyBase : MonoBehaviour {
        public Mode mode;
        public float startHP;
        public float hitPoints;

        protected GameObject player;

        private Renderer _renderer;

        public GameObject[] explosions;
        
        private float _shadeTime = 0f;
        private bool _isTransparent = true;

        protected float x, z;

        private GameManager _gameManager;

        public AudioClip[] sounds;
        private AudioSource _audioSource;

        protected Rigidbody rigid;

        public virtual void Start() {

            hitPoints = startHP;
            mode = Mode.Moving;
            _renderer = GetComponent<Renderer>();
            rigid = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player");

            var position = transform.position;
            x = position.x;
            z = position.z;

            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        public void Update() {
            _shadeTime += ((!_isTransparent) ? 1f : -1f) * Time.deltaTime;
            _shadeTime = Mathf.Clamp(_shadeTime, 0, 255);
            var material = _renderer.material;
            material.color = new Vector4(material.color.r, material.color.b, material.color.g, _shadeTime);

            var relativePos = player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(-relativePos);
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
            if (!col.transform.CompareTag("Player") || mode == Mode.Stunned) return;
            
            fadeIn();

            mode = Mode.Stunned;
            StartCoroutine(Invis(true));
            col.transform.GetComponent<Player>().TakeDamage(2);
            _gameManager.resetMultiplier();
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
            mode = Mode.Moving;
            fadeOut();
        }
        public void TakeDamage(float damage)
        {
            hitPoints -= damage;
            _audioSource.PlayOneShot(sounds[0]);
            if (hitPoints <= 0)
            {
                if (explosions.Length > 0)
                {
                    Instantiate(explosions[Random.Range(0, explosions.Length)]).transform.position = transform.position;
                }
                _audioSource.PlayOneShot(sounds[1]);
                _gameManager.addScore(1);
                Destroy(transform.gameObject);
            }
            else
            {
                _audioSource.PlayOneShot(sounds[0]);
            }
        }

        protected void ClampPosition() 
        {
            var x = Mathf.Clamp(transform.position.x, - transform.parent.position.x, 32 - transform.parent.position.x);
            var z = Mathf.Clamp(transform.position.z, - transform.parent.position.z, 32 - transform.parent.position.z);
            transform.position = new Vector3(x, 1, z);

            //Vector3 worldPosition = transform.TransformVector(transform.position);
            //worldPosition.x = Mathf.Clamp(worldPosition.x, 0, 32);
            //worldPosition.z = Mathf.Clamp(worldPosition.z, 0, 32);
            //worldPosition.y = 1.5f;
            //transform.position = transform.InverseTransformPoint(worldPosition);
        }

        public void fadeOut()
        {
            _isTransparent = true;
        }

        public void fadeIn()
        {
            _isTransparent = false;
        }

    }
}
