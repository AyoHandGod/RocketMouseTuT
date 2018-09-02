using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    // public fields | Laser on and Off sprites
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    public float toggleInterval = 0.5f; // the interval for toggling on/off sprites
    public float rotationSpeed = 0.0f;  // rotation speed. Lasers will rotate in scene

    // private fields
    private bool isLaserOn = true;     // boolean to check if laser on/off
    private float timeUntilNextToggle;  // float holding time until next toggle switch
    private Collider2D laserCollider;    // collider2d of the laser object
    private SpriteRenderer laserRenderer;  // Sprite renderer of laser object
                                           // Use this for initialization
    void Start () {
        timeUntilNextToggle = toggleInterval;  //Sets timeUntilNextTogglet to our togggleInterval value
        laserCollider = gameObject.GetComponent<Collider2D>(); // grab Collider 2D component
        laserRenderer = gameObject.GetComponent<SpriteRenderer>(); // grab sprite renderer component
	}
	
	// Update is called once per frame
	void Update () {

        timeUntilNextToggle -= Time.deltaTime; // use time change to lower timeUntilNextToggle accordingly
        if(timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;  // update isLaserOn boolean accordingly
            laserCollider.enabled = isLaserOn;  // use isLaserOn to determine whether or not collider enabled

            // swap our our laser animations according to on or off status
            if (isLaserOn)
            {
                laserRenderer.sprite = laserOnSprite;
            }
            else
            {
                laserRenderer.sprite = laserOffSprite;
            }
            timeUntilNextToggle = toggleInterval; // reset time until our next toggle
        }

        // rotates laser around z-axis using delta time and rotationSpeed
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
