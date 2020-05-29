using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBullet : Bullet {


    public override void Init(Vector3 position, Vector3 direction)
    {
        base.Init(position, direction);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        //Debug.Log(direction.ToString());
        Vector2 direction2d = new Vector2(direction.x, direction.z);
       //Debug.DrawRay(position, direction);
          if (direction2d.x > 0)
            {
                //targetRotation = Vector2.Angle(Vector2.down, input);
                transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.up, direction2d), 0);
            }
            else
            {
                //targetRotation = 180 + Vector2.Angle(Vector2.up, input);
                transform.rotation = Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.down, direction2d), 0);
            }
    }
    public override void OnCollisionEnter(Collision col)
    {
    }
}
