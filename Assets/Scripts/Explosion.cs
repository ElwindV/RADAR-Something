using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public GameObject source;
    [Range(0,10)]
    public int timeRigidBody;
    [Range(0, 1500)]
    public float force=500;
    [Range(0, 300)]
        public float range;
    [Range(0, 0.2f)]
        public float upWard=0.05f;
	// Use this for initialization
	void Start () {
        foreach (Rigidbody cRigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            cRigidbody.AddExplosionForce(force, source.transform.position, range, upWard);
            if (timeRigidBody > 0)
            {
                Destroy(cRigidbody, timeRigidBody);
                Destroy(cRigidbody.GetComponent<Collider>(), timeRigidBody);

            }
            ParticleSystem particleSystem = cRigidbody.gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                //Debug.Log("yeah!");
                particleSystem.Simulate(Mathf.Pow(Random.Range(2, 6), 1.7f), false, true);
                particleSystem.Play();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
