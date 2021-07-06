using UnityEngine;

public class Water : MonoBehaviour {
    
	[Range(0, 5)]
    public float heightPerMinute;
    
    [Range(0,15)]
    public float waveSpeed;
    
    [Range(0, 0.2f)]
    public float waveHeight;
    private float _height;
    private float _waveStep;

    private void Start () {
        _height = transform.position.y;
        _waveStep = 0;
	}
    
	private void Update () {
        _height += Time.deltaTime / 60f * heightPerMinute;
        _waveStep += Time.deltaTime * waveSpeed;
        
	}
    private void FixedUpdate(){
        transform.position = new Vector3(0, _height + (Mathf.Sin(_waveStep)*waveHeight), 0);
    }
}
