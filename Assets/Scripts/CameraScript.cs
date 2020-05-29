using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject player;
    private Vector3 initialOffset;

    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Vector3 offSet;

    [Range(1f, 10f)]
    public float moveSpeed = 10f;

    public new Camera camera;
    // Use this for initialization
    public void Start ()
    {
        player = GameObject.Find("Player");
        camera=GetComponent<Camera>();

        offSet.y =  transform.position.y;
        offSet.z -= 4f;
        transform.position = player.transform.position + offSet;
	}
	
	public void FixedUpdate ()
    {
        targetPosition = player.transform.position + offSet;
        targetPosition = ClampTargetPosition(targetPosition);
        currentPosition = transform.position;
        
        if(currentPosition != targetPosition)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * moveSpeed);
        }
	}

    public Vector3 ClampTargetPosition(Vector3 targetPosition){
        //Debug.Log(targetPosition.ToString());
        //w 1000 x7
        //w 500 x3.5
        //w 200 x2
        float xdist = camera.ViewportToWorldPoint(new Vector3(0.5f,0, 8)).x - camera.ViewportToWorldPoint(new Vector3(0, 0, 8)).x;
        float ydist = camera.ViewportToWorldPoint(new Vector3(0,0.5f, 8)).z - camera.ViewportToWorldPoint(new Vector3(0, 0, 8)).z;
        //Debug.Log(xdist); 
        if (targetPosition.x<-0.5f+xdist)
        {
            targetPosition.x = -0.5f + xdist;
        }
        if (targetPosition.x > 31f-xdist)
        {
            targetPosition.x = 31f-xdist;
        }
        if (targetPosition.z < -3.4f+ydist)
        {
            targetPosition.z = -3.4f+ydist;
        }
        if (targetPosition.z >28-ydist)
        {
            targetPosition.z = 28-ydist;
        }
        return targetPosition;
    }
}
