using UnityEngine;

namespace Enemies
{
    public class GreenGuy : EnemyBase{

        public Vector3 direction;
        private float _speed = 1.2f;

        public int timer;

        public float distance;

        public override void Start()
        {
            base.Start();
            timer = 60 * 5;

            x = Random.Range(-1, 2);
            z = Random.Range(-1, 2);

            while (x == 0 && z == 0)
            {
                x = Random.Range(-1, 2);
                z = Random.Range(-1, 2);
            }

            direction = new Vector3(x, 0, z);
        }

        public void FixedUpdate () 
        {
            if (mode != Mode.Moving) {
                rb.isKinematic = true;
                return;
            }
            rb.isKinematic = false;
            timer -= 1;

            var toTarget = player.transform.position - transform.position;
            var distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < 4) {
                _speed = 2.5f;
                toTarget = toTarget.normalized;
                direction = toTarget;
            }
            else if (timer <= 0) {
                timer = 60 * 5;

                x = Random.Range(-1, 2);
                z = Random.Range(-1, 2);

                while (x == 0 && z == 0) {
                    x = Random.Range(-1, 2);
                    z = Random.Range(-1, 2);
                }

                direction = new Vector3(x, 0, z);
            }
        
            transform.Translate(direction * (_speed * Time.deltaTime));
            ClampPosition();
        }
    }
}
