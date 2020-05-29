using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    [Range(0, 5)]
    public float heightPerMinute;
    [Range(0,15)]
    public float waveSpeed;
    [Range(0, 0.2f)]
    public float waveHeight;
    private float height;
    private float waveStep;
	// Use this for initialization
	void Start () {
        height = transform.position.y;
        waveStep = 0;
	}
	
	// Update is called once per frame
	void Update () {
        height += Time.deltaTime / 60f * heightPerMinute;
        waveStep += Time.deltaTime * waveSpeed;
        
	}
    void FixedUpdate(){
        transform.position = new Vector3(0, height + (Mathf.Sin(waveStep)*waveHeight), 0);
    }
}
