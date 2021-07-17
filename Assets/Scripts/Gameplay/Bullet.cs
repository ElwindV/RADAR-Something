using Enemies;
using UnityEngine;

namespace Gameplay
{
    public class Bullet : MonoBehaviour {
    
        [Range(0,15)]
        public float speed = 2f;
        [HideInInspector] public Vector3 direction;
        private Rigidbody _rigidBody;
        [Range(0,15)]
        public float damage = 2;

        private void Start () 
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void FixedUpdate () 
        {
            _rigidBody.velocity = transform.forward * (60f * (speed * Time.fixedDeltaTime));
        }
        
        public virtual void OnCollisionEnter(Collision collision)
        {
            var enemy = collision.transform.GetComponent<EnemyBase>();

            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
        
            Destroy(gameObject);
        }
    }
}
