﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGuy : EnemyBase
{ 
    public Vector3 number;

    public override void Start()
    {
        base.Start();
    }

    public void FixedUpdate()
    {
        number = player.transform.position - transform.position;

        if (mode != MODE_MOVING) {
            rb.isKinematic = true;
            return;
        }

        rb.isKinematic = false;
        Vector3 toTarget = player.transform.position - transform.position;
        float speed = 3f;

        toTarget = toTarget.normalized;

        transform.Translate(toTarget * speed * Time.fixedDeltaTime);
        ClampPosition();
    }
}
