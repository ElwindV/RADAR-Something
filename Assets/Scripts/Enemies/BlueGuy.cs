using UnityEngine;

namespace Enemies
{
    public class BlueGuy : EnemyBase
    { 
        public Vector3 number;

        public void FixedUpdate()
        {
            number = player.transform.position - transform.position;

            if (mode != ModeMoving) {
                rb.isKinematic = true;
                return;
            }

            rb.isKinematic = false;
            var toTarget = player.transform.position - transform.position;
            var speed = 3f;

            toTarget = toTarget.normalized;

            transform.Translate(toTarget * (speed * Time.fixedDeltaTime));
            ClampPosition();
        }
    }
}
