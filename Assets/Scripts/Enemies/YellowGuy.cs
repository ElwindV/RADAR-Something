using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGuy : EnemyBase
{
    public Vector3 direction;
    private float speed = 1.2f;

    public int timer;
    public bool change;
    public float distance;


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

        direction = new Vector3(x, 0, z);
    }
	
	public void FixedUpdate () {

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

                if (timer <= 0)
                {
                    timer = 60 * 5;

                    if (change == false)
                    {
                        x *= -1;
                        z *= -1;

                        change = true;
                    }
                    else
                    {
                        x = Random.Range(-1, 2);
                        z = Random.Range(-1, 2);

                        while (x == 0 && z == 0)
                        {
                            x = Random.Range(-1, 2);
                            z = Random.Range(-1, 2);
                        }

                        change = false;
                    }


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
