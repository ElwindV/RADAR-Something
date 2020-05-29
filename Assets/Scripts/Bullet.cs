using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    [Range(0,15)]
    public float speed=2;
    public Vector3 direction;
    public Rigidbody myRigidBody;
    [Range(0,15)]
    public float damage = 2;
    public GameObject[] explosions;

	void Start () 
    {
        myRigidBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () 
    {
        myRigidBody.velocity = direction * speed;
	}

    public virtual void Init(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        this.direction = direction;

        Vector2 direction2d = new Vector2(direction.x, direction.z);

        if (direction2d.x > 0) {
            //targetRotation = Vector2.Angle(Vector2.down, input);
            transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.up, direction2d), 0);
            return;
        }
        //targetRotation = 180 + Vector2.Angle(Vector2.up, input);
        transform.rotation = Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.down, direction2d), 0);
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        EnemyBase enemy = col.transform.GetComponent<EnemyBase>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
        if (explosions.Length > 0) {
            Instantiate(explosions[Random.Range(0, explosions.Length)]).transform.position = transform.position;
        }

        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(gameObject, 5);
        Destroy(this);
    }
}
