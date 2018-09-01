using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;  // Game object that will be our target

    private float distanceToTarget; // float to hold initial distance from camera to target

	// Use this for initialization
	void Start () {
        distanceToTarget = transform.position.x - target.transform.position.x;  // calculates distance from target at start
	}
	
	// Update is called once per frame
	void Update () {
        float targetObjectX = target.transform.position.x; // grabs the x transform position of target
        Vector3 newCameraPosition = transform.position; // create a Vector3 holding current transform position of camera
        newCameraPosition.x = targetObjectX + distanceToTarget;    // updates x value of our newCameraPosition vector3 to that of our target object
                                                                   // and maintains initial distance from target. (Remove distanceToTarget to have camera
                                                                   // follow on top of object. This code will maintain a distance.
        transform.position = newCameraPosition;  // updates camera transform position using newCameraPosition vector3
	}
}
