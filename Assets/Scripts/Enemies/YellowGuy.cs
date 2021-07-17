using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class YellowGuy : EnemyBase
    {
        private Vector3 _direction;
        private float _speed = 1.2f;

        private int _timer;
        private bool _change;

        public override void Start()
        {
            base.Start();
            _timer = 60 * 5;
            _change = false;

            x = Random.Range(-1, 2);
            z = Random.Range(-1, 2);

            while (x == 0 && z == 0)
            {
                x = Random.Range(-1, 2);
                z = Random.Range(-1, 2);
            }

            _direction = new Vector3(x, 0, z);
        }
	
        public void FixedUpdate () {

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
                _direction = toTarget;
                _direction.y = 0;
            }
            else if (_timer <= 0) {
                _timer = 60 * 5;

                if (_change == false) {
                    x *= -1;
                    z *= -1;

                    _change = true;
                }
                else {
                    x = Random.Range(-1, 2);
                    z = Random.Range(-1, 2);

                    while (x == 0 && z == 0) {
                        x = Random.Range(-1, 2);
                        z = Random.Range(-1, 2);
                    }

                    _change = false;
                }

                _direction = new Vector3(x, 0, z);
            }
        

            transform.Translate(_direction * (_speed * Time.fixedDeltaTime));
            ClampPosition(); 
        }
    }
}
