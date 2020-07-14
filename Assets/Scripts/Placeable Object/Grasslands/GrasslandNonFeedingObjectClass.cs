using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasslandNonFeedingObjectClass : MonoBehaviour
{
    // what is our Object Tracking Manager?
    ObjectTrackingClass objectTrackingClass;

    // accessible variables
    public Transform HomeSpot;
    public bool isHomeTaken;
    public Transform RestingSpot;

    // what kind of objects are there?
    enum ObjectTypes {
        // resting
        restingHardDry, restingHardCool, restingHardWet,
        restingSoftDry, restingSoftCool, restingSoftWet,
        // livable
        livableRockOpen, livableRockEnclosed, livableRockElevated,
        livablePlantOpen, livablePlantEnclosed, liveablePlantElevated
    };

    [Header("Set Object Type")]
    [SerializeField] ObjectTypes objectType;

    // Start is called before the first frame update
    void Start()
    {
        // find our object tracking class
        objectTrackingClass = GameObject.Find("Object Tracking Manager").GetComponent<ObjectTrackingClass>();
        // add our obejct to the correct list
        switch (objectType)
        {
            case ObjectTypes.restingHardDry:
                objectTrackingClass.hardDryRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.restingHardCool:
                objectTrackingClass.hardCoolRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.restingHardWet:
                objectTrackingClass.hardWetRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.restingSoftDry:
                objectTrackingClass.softDryRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.restingSoftCool:
                objectTrackingClass.softCoolRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.restingSoftWet:
                objectTrackingClass.softWetRestingSpotList.Add(gameObject);
                break;

            case ObjectTypes.livableRockOpen:
                objectTrackingClass.rockOpenLivableSpotTracking.Add(gameObject);
                break;

            case ObjectTypes.livableRockEnclosed:
                objectTrackingClass.rockEnclosedLivableSpotTracking.Add(gameObject);
                break;

            case ObjectTypes.livableRockElevated:
                objectTrackingClass.rockElevatedLivableSpotTracking.Add(gameObject);
                break;

            case ObjectTypes.livablePlantOpen:
                objectTrackingClass.plantOpenLivableSpotTracking.Add(gameObject);
                break;

            case ObjectTypes.livablePlantEnclosed:
                objectTrackingClass.plantEnclosedLivableSpotTracking.Add(gameObject);
                break;

            case ObjectTypes.liveablePlantElevated:
                objectTrackingClass.plantElevatedLivableSpotTracking.Add(gameObject);
                break;



        }
    }
}
