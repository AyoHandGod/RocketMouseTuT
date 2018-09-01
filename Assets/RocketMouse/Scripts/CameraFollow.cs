using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;  // Game object that will be our target

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float targetObjectX = target.transform.position.x; // grabs the x transform position of target
        Vector3 newCameraPosition = transform.position; // create a Vector3 holding current transform position of camera
        newCameraPosition.x = targetObjectX;    // updates x value of our newCameraPosition vector3 to that of our target object
        transform.position = newCameraPosition;  // updates camera transform position using newCameraPosition vector3
	}
}
