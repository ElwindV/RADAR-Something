using UnityEngine;

public class Explosion : MonoBehaviour {
    
    public GameObject source;
    
    [Range(0,10)]
    public int timeRigidBody;
    
    [Range(0, 1500)]
    public float force = 500;
    
    [Range(0, 300)]
    public float range;
    
    [Range(0, 0.2f)]
    public float upWard = 0.05f;

    private void Start () 
    {
        foreach (var rigidBody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rigidBody.AddExplosionForce(force, source.transform.position, range, upWard);
            if (timeRigidBody > 0)
            {
                Destroy(rigidBody, timeRigidBody);
                Destroy(rigidBody.GetComponent<Collider>(), timeRigidBody);

            }
            var particleSystem = rigidBody.gameObject.GetComponentInChildren<ParticleSystem>();
            
            if (particleSystem == null) continue;

            particleSystem.Simulate(Mathf.Pow(Random.Range(2, 6), 1.7f), false, true);
            particleSystem.Play();
        }
	}

}
