using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    // Public Fields
    public float jetpackForce = 75f;  // force applied to mouse when jet pack is on

    // Private Fields
    private Rigidbody2D rb;         // rigidbody 


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>(); // get the rigidbody component of the object script is applied to
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // use fixedUpdate to check for input constantly
    private void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1"); // creates a boolean to check for input
        if (jetpackActive)                             // checks if boolean above true
        {
            rb.AddForce(new Vector2(0, jetpackForce));  // Apply jetpack force to rigidbody causing movement. vector to specifies x, and y movements applied. 
        }
    }
}
