using UnityEngine;

namespace Enemies
{
    public class WhiteGuy : EnemyBase {

        public Vector3 direction;
        private float _speed = 1.2f;

        public int timer;
        private float y;

        public override void Start()
        {
            base.Start();

            x = 1; z = 0;
            timer = 60 * 20;
            direction = new Vector3(x, 0, z);
        }
	
        public void FixedUpdate () 
        {
            if (mode != Mode.Moving) {
                rigid.isKinematic = true;

                return;
            }

            rigid.isKinematic = false;
            timer -= 1;
            var toTarget = player.transform.position - transform.position;
            var distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < 4) {
                _speed = 2.5f;
                toTarget = toTarget.normalized;
                direction = toTarget;
            }
            else {
                _speed = 1.2f;

                if (timer <= 0) {
                    
                    timer = 60 * 20;
                    y -= 90;

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);

                    direction = new Vector3(x, 0, z);
                }
            }

            transform.Translate(direction * (_speed * Time.fixedDeltaTime));
            ClampPosition();
        }
    }
}
