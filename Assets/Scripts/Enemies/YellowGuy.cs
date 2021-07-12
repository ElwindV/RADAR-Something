using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class YellowGuy : EnemyBase
    {
        private Vector3 _direction;
        private float _speed = 1.2f;

        public int timer;
        public bool change;

        public override void Start()
        {
            base.Start();
            timer = 60 * 5;
            change = false;

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
            timer -= 1;

            var toTarget = player.transform.position - transform.position;
            var distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < 4) {
                _speed = 2.5f;
                toTarget = toTarget.normalized;
                _direction = toTarget;
            }
            else if (timer <= 0) {
                timer = 60 * 5;

                if (change == false) {
                    x *= -1;
                    z *= -1;

                    change = true;
                }
                else {
                    x = Random.Range(-1, 2);
                    z = Random.Range(-1, 2);

                    while (x == 0 && z == 0) {
                        x = Random.Range(-1, 2);
                        z = Random.Range(-1, 2);
                    }

                    change = false;
                }

                _direction = new Vector3(x, 0, z);
            }
        

            transform.Translate(_direction * (_speed * Time.fixedDeltaTime));
            ClampPosition(); 
        }
    }
}
