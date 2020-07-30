using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JM.LinqFaster;

public class ActionScript : MonoBehaviour
{
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
        objectTrackingClass = WorldManager.objectTrackingClass;
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
        List<GameObject> tempList = restingSpotList.WhereF(x => !objectTrackingClass.objectsinUseTracking.Contains(x));
        if (tempList.Count > 0)
        {
            // get resting spot
            GameObject targetRestSpot = tempList[Random.Range(0, tempList.Count)];
            if (targetRestSpot == null)
            {
                Debug.Log("Resting spot not found"); // *** could change to keep looking for instead of leaving
                yield break;
            }
            // mark as in use
            objectTrackingClass.objectsinUseTracking.Add(targetRestSpot);
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
            // unmark as in use
            if (targetRestSpot != null) objectTrackingClass.objectsinUseTracking.Remove(targetRestSpot);
        }
        else Debug.Log("All resting spots in use");
    }

    protected IEnumerator Eat()
    {
        // we're finding food
        currentStatus.text = "Finding Food";
        List<FeedingSpotClass> tempList = feedingSpotList.WhereF(x => !objectTrackingClass.objectsinUseTracking.Contains(x.gameObject));
        if (tempList.Count > 0)
        {
            // get food spot
            GameObject targetFood = tempList[Random.Range(0, tempList.Count)].gameObject;
            if (targetFood == null)
            {
                Debug.Log("Target food not found");
                yield break;
            }
            // mark as in use
            objectTrackingClass.objectsinUseTracking.Add(targetFood);
            Debug.Log(targetFood.name + " is in use by " + this.gameObject.name);
            // get food spot position and move to it
            targetPosition = new Vector3(targetFood.transform.position.x, transform.position.y, targetFood.transform.position.z);
            yield return StartCoroutine(MoveTo());
            // if food is still there when you get there, eat
            if (targetFood != null)
            {
                currentStatus.text = "Eating";
                yield return new WaitForSeconds(Random.Range(5, 10));
                // unmark as in use
                if (targetFood != null) objectTrackingClass.objectsinUseTracking.Remove(targetFood);
            }
            else // if the food is gone start exploring
            {
                yield return StartCoroutine(Explore());
            }
        }
        else Debug.Log("All feeding spots in use");
    }

    protected IEnumerator MoveTo()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.000001) // go until in the middle of hex
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return 0;
        }
    }

    /*protected IEnumerator FlyAround() // has a little bounce to it, want to start going a random direction and then move within angles
    {
        //currentStatus.text = "Flying Around"; // just see
        // 1) Get position of trees
        int updown = 1;
        Vector3 targetAngle = Vector3.zero;
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY + 0.1f*updown, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
            updown*=-1; */
            // // todo: make it look semi-okay flying
            /*float movementIncrement = Vector3.Distance(transform.position, targetPosition) / movementSpeed;
            float currTime = 0;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1) // go until in the middle of hex
            {
                currTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPosition, currTime/movementIncrement); // (currtime * speed) / distance
                yield return 0;
            }*/ /*
            while (Vector3.Distance(transform.position, targetPosition) > 0.000001) // go until in the middle of hex
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return 0;
            }
        }
        yield return 0;
    }

    protected IEnumerator Hop() // has a lottle bounce to it :V
    {
        currentStatus.text = "Hopping";
        int hopNum = Random.Range(1,5); // how many hops
        for (int i = 0; i < Random.Range(2,7); i++)
        {
            // ** see if there is a rock in hoppable distance that is hoppable hop on it wait then hop off
            // 1) Go up
            // 2) Go down
            yield return new WaitForSeconds(Random.Range(0.5f, 4f)); // time between hops
        }
    }

    protected IEnumerator ChangeBetweenGroundAndSky(bool onGround)
    {
        if (onGround)
        {
            // get tree positions
        }
        else
        {
            // go to ground
        }
        yield return 0;
    } */
}