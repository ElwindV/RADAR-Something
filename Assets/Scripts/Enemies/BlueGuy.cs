using UnityEngine;

namespace Enemies
{
    public class BlueGuy : EnemyBase
    {
        public void FixedUpdate()
        {
            if (mode != Mode.Moving) {
                rigid.isKinematic = true;
                return;
            }

            rigid.isKinematic = false;
            var toTarget = player.transform.position - transform.position;
            const float speed = 3f;

            toTarget = toTarget.normalized;

            transform.Translate(toTarget * (speed * Time.fixedDeltaTime));
            ClampPosition();
        }
    }
}
