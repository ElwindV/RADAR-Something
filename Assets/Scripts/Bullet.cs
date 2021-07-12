using Enemies;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    [Range(0,15)]
    public float speed = 2f;
    [HideInInspector] public Vector3 direction;
    private Rigidbody _rigidBody;
    [Range(0,15)]
    public float damage = 2;

    private void Start () 
    {
        _rigidBody = GetComponent<Rigidbody>();
	}

    public void FixedUpdate () 
    {
        _rigidBody.velocity = direction * speed;
	}

    public virtual void Init(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        this.direction = direction;

        var direction2d = new Vector2(direction.x, direction.z);

        if (direction2d.x > 0) {
            //targetRotation = Vector2.Angle(Vector2.down, input);
            transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.up, direction2d), 0);
            return;
        }
        //targetRotation = 180 + Vector2.Angle(Vector2.up, input);
        transform.rotation = Quaternion.Euler(0, 180 + Vector2.Angle(Vector2.down, direction2d), 0);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        var enemy = collision.transform.GetComponent<EnemyBase>();

        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
