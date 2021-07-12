using UnityEngine;

namespace Enemies
{
    public class RedGuy : EnemyBase {

        public Vector3 direction;
        private float _speed = 1.2f;

        private int _timer;
        private float _y;

        public override void Start()
        {
            base.Start();

            x = 1; z = 0;
            _timer = 60 * 20;
            direction = new Vector3(x, 0, z);
        }
	
        public void FixedUpdate () 
        {
            if (mode != Mode.Moving) {
                rigid.isKinematic = true;

                return;
            }

            rigid.isKinematic = false;
            _timer -= 1;
            var toTarget = player.transform.position - transform.position;
            var distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < 4) {
                _speed = 2.5f;
                toTarget = toTarget.normalized;
                direction = toTarget;
            }
            else {
                _speed = 1.2f;

                if (_timer <= 0) {
                    
                    _timer = 60 * 20;
                    _y -= 90;

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, _y, transform.eulerAngles.z);

                    direction = new Vector3(x, 0, z);
                }
            }

            transform.Translate(direction * (_speed * Time.fixedDeltaTime));
            ClampPosition();
        }
    }
}
