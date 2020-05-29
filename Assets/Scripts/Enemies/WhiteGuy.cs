using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteGuy : EnemyBase {

    public Vector3 direction;
    private float speed = 1.2f;

    public int timer;
    private float y;

    public float distance;

    public override void Start()
    {
        base.Start();

        x = 1; z = 0;
        timer = 60 * 20;
        direction = new Vector3(x, 0, z);
    }
	
	public void FixedUpdate () 
    {
        if (mode == MODE_MOVING)
        {
            rb.isKinematic = false;
            timer -= 1;
            Vector3 toTarget = player.transform.position - transform.position;
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < 4)
            {
                speed = 2.5f;
                toTarget = toTarget.normalized;
                direction = toTarget;
            }
            else
            {
                speed = 1.2f;
                if (timer <= 0)
                {
                    
                    timer = 60 * 20;

                    y -= 90;

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);

                    direction = new Vector3(x, 0, z);
                }
            }

            transform.Translate(direction * speed * Time.fixedDeltaTime);
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        else
        {
            rb.isKinematic = true;
        }

    }
}
