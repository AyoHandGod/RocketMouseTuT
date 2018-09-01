using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

    public GameObject[] availableRooms; // Array of rooms to be used
    public List<GameObject> currentRooms; // List of Currently active rooms. Used to check where last room ends
    private float screenWidthInPoints;  // stores screen length to check if we are running our of rooms
	// Use this for initialization
	void Start () {
        float height = 2.0f * Camera.main.orthographicSize;  // Screen size calculation
        screenWidthInPoints = height * Camera.main.aspect;  //  used to determine if we need to generate a room

        StartCoroutine(GeneratorCheck());   // launch the GeneratorCheck() coroutine function
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Adds a room to the furthest end of the scene
    void AddRoom(float farthestRoomEndX)
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length); // grabs the index of a random room piece
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);  // instantiates a random room object
        float roomWidth = room.transform.Find("floor").localScale.x;   // get the size of the room inside the room, as the room is an 
                                                                       // empyt GameObject we can't grab from it. 

        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;        // takes the furthest edge of the level add half the new rooms width,
                                                                       // this starts new room exactly at end of previous

        room.transform.position = new Vector3(roomCenter, 0, 0);       // sets the position of the room object. Only x needs changing as y and z are 0

        currentRooms.Add(room);  // Add room to the end of the list of currentRooms                                             
    }

    private void GenerateRoomIfRequired()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();   // creates a list of rooms to be removed
        bool addRooms = true;   // Checks if we need to add a room
        float playerX = transform.position.x;  // gets the transform x position of the player
        float removeRoomX = playerX - screenWidthInPoints;  // Calculates the point at which a room needs to removed
        float addRoomX = playerX + screenWidthInPoints;     // If there is no room after this point, we need to add a room
        float farthestRoomEndX = 0;
        foreach (var room in currentRooms)
        {
            // Use the floor to calculate the roomStart and roomEnd x positions
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if(roomStartX > addRoomX) // checks if the startX value is greater than add room. If so, no room needed yet.
            {
                addRooms = false;
            }

            if(roomEndX < removeRoomX) // checks if the rooms end X is less than the necessary value to remove it. If so, remove it
            {
                roomsToRemove.Add(room);
            }

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);  // finds the right most points of the level
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room); // removes room from list of rooms
            Destroy(room);             // Destroys the room object that has been removed
        }

        if (addRooms) // checks if additional room needed and adds it. 
        {
            AddRoom(farthestRoomEndX);
        }
    }

    // Method to configure script as coroutine 
    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f); // adds a pause in execution to minimize resource req. due to list use
        }
    }
}
