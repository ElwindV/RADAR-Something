using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject _player;
    private Vector3 _initialOffset;

    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private Vector3 _offSet;

    [Range(1f, 10f)]
    public float moveSpeed = 10f;

    public new Camera camera;

    public void Start ()
    {
        _player = GameObject.Find("Player");
        camera=GetComponent<Camera>();

        _offSet.y =  transform.position.y;
        _offSet.z -= 4f;
        transform.position = _player.transform.position + _offSet;
	}
	
	public void FixedUpdate ()
    {
        _targetPosition = _player.transform.position + _offSet;
        _targetPosition = ClampTargetPosition(_targetPosition);
        _currentPosition = transform.position;
        
        if(_currentPosition != _targetPosition) {
            transform.position = Vector3.Lerp(_currentPosition, _targetPosition, Time.fixedDeltaTime * moveSpeed);
        }
	}

    private Vector3 ClampTargetPosition(Vector3 targetPosition)
    {
        var xDistance = camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 8f)).x - camera.ViewportToWorldPoint(new Vector3(0, 0, 8)).x;
        var yDistance = camera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 8f)).z - camera.ViewportToWorldPoint(new Vector3(0, 0, 8)).z;

        if (targetPosition.x < -0.5f + xDistance) {
            targetPosition.x = -0.5f + xDistance;
        }
        if (targetPosition.x > 31f - xDistance) {
            targetPosition.x = 31f - xDistance;
        }
        if (targetPosition.z < -3.4f + yDistance) {
            targetPosition.z = -3.4f + yDistance;
        }
        if (targetPosition.z > 28 - yDistance) {
            targetPosition.z = 28 - yDistance;
        }
        return targetPosition;
    }
}
