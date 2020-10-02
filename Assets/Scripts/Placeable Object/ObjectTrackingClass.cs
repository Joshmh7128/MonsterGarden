using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectTrackingClass : MonoBehaviour
{
    [Header("Grassland Object Tracking")]
    // worker list
    private List<List<FeedingSpotClass>> feedingWorkerList = new List<List<FeedingSpotClass>>();
    private List<List<GameObject>> gameObjectWorkerList = new List<List<GameObject>>();

    // this script is used to track all of our feeding spots
    [Header("Grassland Feeding Spot Tracking")]
    public List<FeedingSpotClass> grainFeedingSpotList = new List<FeedingSpotClass>();
    public List<FeedingSpotClass> grassFeedingSpotList = new List<FeedingSpotClass>();
    public List<FeedingSpotClass> leavesFeedingSpotList = new List<FeedingSpotClass>();

    [Header("Grassland Hard Resting Spot Tracking")]
    public List<GameObject> hardDryRestingSpotList = new List<GameObject>();
    public List<GameObject> hardCoolRestingSpotList = new List<GameObject>();
    public List<GameObject> hardWetRestingSpotList = new List<GameObject>();

    [Header("Grassland Soft Resting Spot Tracking")]
    public List<GameObject> softDryRestingSpotList = new List<GameObject>();
    public List<GameObject> softCoolRestingSpotList = new List<GameObject>();
    public List<GameObject> softWetRestingSpotList = new List<GameObject>();

    [Header("Grassland Livable Spot Tracking")]
    public List<GameObject> rockOpenLivableSpotTracking = new List<GameObject>();
    public List<GameObject> rockEnclosedLivableSpotTracking = new List<GameObject>();
    public List<GameObject> rockElevatedLivableSpotTracking = new List<GameObject>();
    public List<GameObject> plantOpenLivableSpotTracking = new List<GameObject>();
    public List<GameObject> plantEnclosedLivableSpotTracking = new List<GameObject>();
    public List<GameObject> plantElevatedLivableSpotTracking = new List<GameObject>();

    [Header("Objects in Use Tracking")]
    public HashSet<GameObject> objectsinUseTracking = new HashSet<GameObject>();
    public Dictionary<GameObject, GameObject> claimedHomes = new Dictionary<GameObject, GameObject>(); // Key: Home, Value: Monster


    public void Start()
    {
        WorldManager.objectTrackingClass = this; // TODO: FIX
        // manually add our working lists so that we don't have any issues in runtime

        // feeding
        feedingWorkerList.Add(grainFeedingSpotList);
        feedingWorkerList.Add(grassFeedingSpotList);
        feedingWorkerList.Add(leavesFeedingSpotList);

        // gameobjects
        // hard
        gameObjectWorkerList.Add(hardDryRestingSpotList);
        gameObjectWorkerList.Add(hardCoolRestingSpotList);
        gameObjectWorkerList.Add(hardWetRestingSpotList);
        // soft
        gameObjectWorkerList.Add(softDryRestingSpotList);
        gameObjectWorkerList.Add(softCoolRestingSpotList);
        gameObjectWorkerList.Add(softWetRestingSpotList);
        // rock
        gameObjectWorkerList.Add(rockOpenLivableSpotTracking);
        gameObjectWorkerList.Add(rockEnclosedLivableSpotTracking);
        gameObjectWorkerList.Add(rockElevatedLivableSpotTracking);
        // plant
        gameObjectWorkerList.Add(plantOpenLivableSpotTracking);
        gameObjectWorkerList.Add(plantEnclosedLivableSpotTracking);
        gameObjectWorkerList.Add(plantElevatedLivableSpotTracking);
    }

    // check out lists and remove anything from them we don't need. Only call when modifying or changing the lists.
    public void ListCheck()
    {
        // check all feeding spot lists
        foreach (List<FeedingSpotClass> list in feedingWorkerList)
        {
            // remove all items from the list
            list.RemoveAll(item => item == null);
        }

        // check all gameobject lists
        foreach (List<GameObject> list in gameObjectWorkerList)
        {
            // remove all items from the list
            list.RemoveAll(item => item == null);
        }
    }

    public IEnumerator WaitCheck()
    {
        yield return new WaitForEndOfFrame();
        ListCheck();
    }
}
