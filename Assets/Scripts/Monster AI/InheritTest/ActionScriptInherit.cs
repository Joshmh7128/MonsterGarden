using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionScriptInherit : MonoBehaviour
{
    // this AI is for the Grassland Deer

    [SerializeField] protected ObjectTrackingClass objectTrackingClass; // our object tracking class
    [SerializeField] protected Text currentStatus; // what is the current status of the deer?
    [SerializeField] protected float maxExploreBoundsX; // max X explore?
    [SerializeField] protected float maxExploreBoundsZ; // max Z explore?
    [SerializeField] protected float startY; // our Y for logging
    [SerializeField] protected float movementSpeed; // how fast can we move?
    [SerializeField] protected Vector3 targetPosition; // where are we headed?
    [SerializeField] protected bool behaviourActive; // is our behaviour active?
    [SerializeField] protected string restingSpotType; // example: "Soft-Dry"
    [SerializeField] protected string feedingSpotType; // example: "Grass"
    [SerializeField] protected List<GameObject> restingSpotList;
    [SerializeField] protected List<FeedingSpotClass> feedingSpotList;

    // start
    public virtual void Start()
    {
        // set our Y
        startY = transform.position.y;
        // set our object tracking class
        objectTrackingClass = GameObject.Find("Object Tracking Manager").GetComponent<ObjectTrackingClass>();
        if (restingSpotType == "Soft-Dry") restingSpotList = objectTrackingClass.softDryRestingSpotList; // todo: make good
        if (feedingSpotType == "Grass") feedingSpotList = objectTrackingClass.grassFeedingSpotList;
    }

    // explore
    protected IEnumerator Explore()
    {
        // go to random location, explore!
        targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
        currentStatus.text = "Exploring";
        yield return StartCoroutine(MoveTo());
        yield return new WaitForSeconds(Random.Range(0, 8)); // 14
    }

    // required actions: eat at 3 grass spots, rest at one resting spot

    protected IEnumerator Rest()
    {
        // find a place to rest
        currentStatus.text = "Finding Resting Spot";
        if (restingSpotList.Count > 0)
        {
            // get resting spot
            GameObject targetRestSpot = restingSpotList[Random.Range(0, restingSpotList.Count)];
            if (targetRestSpot == null)
            {
                Debug.Log("Resting spot not found"); // *** could change to keep looking for instead of leaving
                yield break;
            }
            // get position of resting spot
            targetPosition = new Vector3(targetRestSpot.transform.position.x, transform.position.y, targetRestSpot.transform.position.z);
            // move to resting spot
            yield return MoveTo();
            // rest 3 - 5 times, checking if the resting spot is there in between
            for (int i = 0; i< Random.Range(3,6); i++)
            {
                if (targetRestSpot == null) // if spot has been deleted, start exploring
                {
                    yield break;
                }
                yield return new WaitForSeconds(5);
            }
        }
    }

    protected IEnumerator Eat()
    {
        // we're finding food
        currentStatus.text = "Finding Food";
        if (feedingSpotList.Count > 0)
        {
            // get food spot
            GameObject targetFood = feedingSpotList[Random.Range(0, feedingSpotList.Count)].gameObject;
            if (targetFood == null)
            {
                Debug.Log("Target food not found");
                yield break;
            }
            // get food spot position and move to it
            targetPosition = new Vector3(targetFood.transform.position.x, transform.position.y, targetFood.transform.position.z);
            yield return StartCoroutine(MoveTo());
            // if food is still there when you get there, eat
            if (targetFood != null)
            {
                currentStatus.text = "Eating";
                yield return new WaitForSeconds(Random.Range(5, 10));
            }
            else // if the food is gone start exploring
            {
                yield return StartCoroutine(Explore());
            }
        }
    }

    protected IEnumerator MoveTo()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.000001) // go until in the middle of hex
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return 0;
        }
    }
}