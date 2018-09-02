using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

    // Public Object Fields (Lasers / Coins/ Etc.)
    public GameObject[] availableObjects;  // Array of available Game objects
    public List<GameObject> objects;       // list of current objects

    public float objectsMinDistance = 5.0f;      // minimum distance from last object
    public float objectsMaxDistance = 10.0f;    // maximum distance from last object

    public float objectsMinY = -1.4f;      // minimum Y position of object
    public float objectsMaxY = 1.4f;       // maximum Y position of object

    public float objectsMinRotation = -45.0f;  // minimum rotation of objects
    public float objectsMaxRotation = 45.0f;   // maximum rotation of objects


    // Public Room Fields
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
        float roomWidth = room.transform.Find("Floor").localScale.x;   // get the size of the room inside the room, as the room is an 
                                                                       // empyt GameObject we can't grab from it. 
                                                                       // Be sure to set the "find" parameter to the name you've set for your Floor object in game

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
            float roomWidth = room.transform.Find("Floor").localScale.x;
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

    // Add objects to room. Generates new objects
    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);  // random number used with array to generate random object
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);   // Instantiate an object using the array of availble object and index
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance); // calculates a position for new object between min and max distance
        float randomY = Random.Range(objectsMinY, objectsMaxY);  // generates a random y positions using min and max Y values
        obj.transform.position = new Vector3(objectPositionX, randomY, 0); // set objects transfrom using above settings

        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation); // calculates a random rotation betwen min and max
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); // use random rotation to manipulate object rotation

        objects.Add(obj); // Adds the new object to the objects list
    }

    // Determine if new object needed
    void GenerateObjectsIfRequired()
    {
        // calculates key points ahead and behind player. If object is left of removeObjectX, we will remove it. 
        // If there isn't an object after the addObjectX position, we will add an objects
        // we use the farthestObjectX to compare to addObjectX and see if its necessary
        float playerX = transform.position.x;
        float removeObjectX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>(); // Creates a list to hold objects to be destroyed
        foreach (var obj in objects)
        {
            float objX = obj.transform.position.x; // gets the x position of the object

            farthestObjectX = Mathf.Max(farthestObjectX, objX); // set farthestObjectX to the farthest object our of current farthest and objX

            // if the objX is farther to the left than the removeObjectX, add it to the object to be remove list
            if(objX < removeObjectX)
            {
                objectsToRemove.Add(obj);
            }
        }

        // for each item in the objects to remove list, we remove it from list and then destroy it
        foreach(var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        // If the farthestObjectX is less than the addObjectX position, we add an object using the addObject function and farthestX as the position
        if(farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }

    }

    // Method to configure script as coroutine, allows object and rooms to be added as coroutine
    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            
            GenerateRoomIfRequired();
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.25f); // adds a pause in execution to minimize resource req. due to list use
      
        }
    }
}
