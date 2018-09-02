using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour {

    // public fields. Renderer for background and foreground
    public Renderer background;
    public Renderer foreground;

    // speeds for scrolls
    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;

    public float offset = 0.0f; // Used to tie Mouse movement to the parallax background, reacts to player changes

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // calculate background and foreground offsets
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;

        // Set the background and foreground offsets using Vector2 and above values
        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);

    }
}
