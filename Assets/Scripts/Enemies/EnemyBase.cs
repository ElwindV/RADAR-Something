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
        
        private float _shadeTime;
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
                FadeIn();
            }
        }
        public void OnCollisionStay(Collision col)
        {
            if (!col.transform.CompareTag("Player") || mode == Mode.Stunned) return;
            
            FadeIn();

            mode = Mode.Stunned;
            StartCoroutine(Invis(true));
            col.transform.GetComponent<Player>().TakeDamage(2);
            _gameManager.ResetMultiplier();
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

            FadeOut();
        }
        private IEnumerator Invis(bool stun)
        {
            yield return new WaitForSeconds(2);
            mode = Mode.Moving;
            FadeOut();
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
                _gameManager.AddScore(1);
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
        }

        public void FadeOut()
        {
            _isTransparent = true;
        }

        public void FadeIn()
        {
            _isTransparent = false;
        }

    }
}
