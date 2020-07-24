using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public ObjectTrackingClass objectTrackingClass; // our object tracking class
    public Text currentStatus; // what is the current status of the deer?
    public float maxExploreBoundsX; // max X explore?
    public float maxExploreBoundsZ; // max Z explore?
    public float startY; // our Y for logging
    public float movementSpeed; // how fast can we move?
    public Vector3 targetPosition; // where are we headed?
    public bool behaviourActive; // is our behaviour active?
    public string restingSpotType; // example: "Soft-Dry"
    public string feedingSpotType; // example: "Grass"
    public List<GameObject> restingSpotList;
    public List<FeedingSpotClass> feedingSpotList;

    void Start()
    {
        // set our Y
        startY = transform.position.y;
        // set our object tracking class
        objectTrackingClass = GameObject.Find("Object Tracking Manager").GetComponent<ObjectTrackingClass>();
        // set up feeding spot and resting spot types
        if (restingSpotType == "Soft-Dry") restingSpotList = objectTrackingClass.softDryRestingSpotList; // todo: make good
        if (feedingSpotType == "Grass") feedingSpotList = objectTrackingClass.grassFeedingSpotList;
    }
}
