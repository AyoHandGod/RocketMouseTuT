using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    // Public Fields
    public float jetpackForce = 75f;  // force applied to mouse when jet pack is on
    public float forwardMovementSpeed = 3.0f;  // forward movement force

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
        Vector2 newVelocity = rb.velocity;  // set a vector2 to the value of our objects rigidbody velocity
        newVelocity.x = forwardMovementSpeed;  // update the x value of newVelocity to equal our forwardMovementSpeed
        rb.velocity = newVelocity;  // Update objects rigidbody velocity using new newVelocity values
    }
}
