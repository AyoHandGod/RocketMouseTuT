using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // import UI classes

public class MouseController : MonoBehaviour {

    // Public Fields
    public float jetpackForce = 75f;  // force applied to mouse when jet pack is on
    public float forwardMovementSpeed = 3.0f;  // forward movement force
    public ParticleSystem jetpack;       // our jetpack particle system
    public Transform groundCheckTransform;     // groundCheck child object we will be using
    public LayerMask groundCheckLayerMask;     // Layer Mask of the layer we wish to check collision with
    public Text coinsCollectedLabel;  // use the 'Text' class from UnityEngine.UI. This will hold the value of collected coins

    // Private Fields
    private Rigidbody2D rb;         // rigidbody 
    private bool isGrounded;       // is groundCheck touching the floor?
    private Animator animator;     // grab our animator
    private bool isDead = false;   // boolean to check whether mouse is dead or not
    private uint coins = 0;        // holds the amount of collected coins

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // get the rigidbody component of the object script is applied to
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // use fixedUpdate to check for input constantly
    private void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1"); // creates a boolean to check for input
        jetpackActive = jetpackActive && !isDead;      // makes sure mouse isn't dead and jet pack active

        if (jetpackActive)                             // checks if boolean above true
        {
            rb.AddForce(new Vector2(0, jetpackForce));  // Apply jetpack force to rigidbody causing movement. vector to specifies x, and y movements applied. 
        }

        // makes sure mouse is not dead before running following code
        if (!isDead)
        {
            Vector2 newVelocity = rb.velocity;  // set a vector2 to the value of our objects rigidbody velocity
            newVelocity.x = forwardMovementSpeed;  // update the x value of newVelocity to equal our forwardMovementSpeed
            rb.velocity = newVelocity;  // Update objects rigidbody velocity using new newVelocity values
        }

        UpdateGroundedStatus();    // calls updatedGroundedStatus check function
        AdjustJetpack(jetpackActive);   // Adjust display of jetpack emissions 
    }

    void UpdateGroundedStatus()
    {
        // if the groundCheck object touches something on the specified layer, is grounded becomes true
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("isGrounded", isGrounded); // uses isGrounded to switch animation condition isGrounded
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission; // variable holding the emission status from our jetpack particley
        jetpackEmission.enabled = !isGrounded; // jetpack Emission is enabled or disabled in opposite to isGrounded
        // increase and decrease jetpack emission depending on status
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 75.0f;
        }
    }

    // Trigger response to collision
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the collision gameObject tag is coin, if so runs CollectCoin, if not HitByLaser
        if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision);
        }
        else
        {
            HitByLaser(collision);
        }

    }

    // Changes mouse to dead and set animator bool if function called
    void HitByLaser(Collider2D laserCollider)
    {
        isDead = true;
        animator.SetBool("isDead", true);
    }

    // Function to collect coin tag objects
    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();  // applies the values of coins to our text ui element
        Destroy(coinCollider.gameObject);
        
    }
}
