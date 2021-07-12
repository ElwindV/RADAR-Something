using System.Collections;
using Audio;
using Managers;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyBase : MonoBehaviour {
        public Mode mode;
        public float startHP;
        private float _hitPoints;

        protected GameObject player;

        private Renderer _renderer;

        private float _shadeTime;
        private bool _isTransparent = true;

        protected float x, z;

        private GameManager _gameManager;

        [Header("Audio")] 
        public AudioCueEventChannelSO eventChannelSO;
        public AudioCueSO hurtSfx;
        public AudioCueSO deathSfx;

        protected Rigidbody rigid;

        public virtual void Start() 
        {
            _hitPoints = startHP;
            mode = Mode.Moving;
            _renderer = GetComponent<Renderer>();
            rigid = GetComponent<Rigidbody>();
            player = GameObject.FindGameObjectWithTag("Player");

            var position = transform.position;
            x = position.x;
            z = position.z;

            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        public void Update() 
        {
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
            StartCoroutine(Invisible(true));
            col.transform.GetComponent<Player>().TakeDamage(2);
            _gameManager.ResetMultiplier();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("beam"))
            {
                StartCoroutine(Invisible());
            }
        }

        private IEnumerator Invisible()
        {
            yield return new WaitForSeconds(2);

            FadeOut();
        }
        private IEnumerator Invisible(bool stun)
        {
            yield return new WaitForSeconds(2);
            mode = Mode.Moving;
            FadeOut();
        }
        public void TakeDamage(float damage)
        {
            _hitPoints -= damage;
            
            if (_hitPoints <= 0)
            {
                eventChannelSO.RaiseEvent(deathSfx);
                _gameManager.AddScore(1);
                Destroy(gameObject);

                return;
            }

            eventChannelSO.RaiseEvent(hurtSfx);
        }

        protected void ClampPosition() 
        {
            var x = Mathf.Clamp(transform.position.x, - transform.parent.position.x, 32 - transform.parent.position.x);
            var z = Mathf.Clamp(transform.position.z, - transform.parent.position.z, 32 - transform.parent.position.z);
            transform.position = new Vector3(x, 1, z);
        }

        private void FadeOut()
        {
            _isTransparent = true;
        }

        private void FadeIn()
        {
            _isTransparent = false;
        }

    }
}
