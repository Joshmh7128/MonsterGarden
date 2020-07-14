using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrasslandDeerAI : MonoBehaviour
{
    // this AI is for the Grassland Deer

    [SerializeField] ObjectTrackingClass objectTrackingClass; // our object tracking class
    [SerializeField] Text currentStatus; // what is the current status of the deer?
    [SerializeField] float maxExploreBoundsX; // max X explore?
    [SerializeField] float maxExploreBoundsZ; // max Z explore?
    [SerializeField] float startY; // our Y for logging
    [SerializeField] float movementSpeed; // how fast can we move?
    [SerializeField] Vector3 targetPosition; // where are we headed?
    [SerializeField] bool behaviourActive; // is our behaviour active?

    // start
    private void Start()
    {
        // set our Y
        startY = transform.position.y;
        // set our object tracking class
        objectTrackingClass = GameObject.Find("Object Tracking Manager").GetComponent<ObjectTrackingClass>();
        // start our AI up
        ChooseBehaviour();
    }

    private void FixedUpdate()
    {
        // move to our target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }

    private void ChooseBehaviour()
    {
        int i = Random.Range(0, 3);
        
        switch (i)
        {
            case 0:
                StartCoroutine("Explore");
                break;

            case 1:
                StartCoroutine(RestFind(null));
                break;
                
            case 2:
                StartCoroutine("EatFind");
                break;
        }
    }

    // explore
    IEnumerator Explore()
    {
        // go to random location, explore!
        targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
        currentStatus.text = "Exploring";
        yield return new WaitForSeconds(Random.Range(8, 14));
        ChooseBehaviour();
    }

    // required actions: eat at 3 grass spots, rest at one resting spot

    // find a place to rest
    IEnumerator RestFind(GameObject optionalTargetSpot)
    {
        // finding a place to relax
        currentStatus.text = "Finding Resting Spot";
        // find a place to relax
        if (objectTrackingClass.softDryRestingSpotList.Count > 0)
        {
            // Debug.Log("begin restfind");
            // init var
            GameObject targetRestSpot = optionalTargetSpot;
            // get our game object to pass to the RestCheck if we are not given one
            if (targetRestSpot == null)
            {
                //  Debug.Log("target was NULL finding new");
                targetRestSpot = objectTrackingClass.softDryRestingSpotList[Random.Range(0, objectTrackingClass.softDryRestingSpotList.Count)];
                // Debug.Log("new target found");
            }
            targetPosition = targetRestSpot.GetComponent<Transform>().position;
            targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            yield return new WaitForSeconds(5);
            // check to see if we can rest here
            // Debug.Log("Starting Walking Rest Check");
            // if our object still exists, pass it to the check
            localTargetCheck(targetRestSpot);
            yield break;
        }
        else
        {
            ChooseBehaviour();
            yield break;
        }

        // check our target
        void localTargetCheck(GameObject targetObject)
        {
            // Debug.Log("Walking test started");
            // if our position is not that of our target...
            if (transform.position != targetPosition)
            {
                // if the target is null...
                if (targetObject == null)
                {
                    // Debug.Log("out, new behaviour");
                    ChooseBehaviour();
                }
                else
                {
                    // iterate with the previously chosen object
                    StartCoroutine(RestFind(targetObject));
                    // Debug.Log("out, iteration");
                }
            }

            // if our position is that of our target...
            if (transform.position == targetPosition)
            {
                // if the object exists
                if (targetObject != null)
                {
                    // run the rest check
                    // Debug.Log("Rest Check Coroutine Started");
                    StartCoroutine(RestCheck(targetObject));
                }
                else if (targetObject == null)
                {
                    // sDebug.Log("out, new behaviour");
                    ChooseBehaviour();
                }
            }
        }
    }

    // check to see if we can rest here
    IEnumerator RestCheck(GameObject targetRestSpot)
    {
        currentStatus.text = "Getting Ready to Rest";
        // can we rest?
        bool canRest = false;

        // wait 5 seconds for the check
        yield return new WaitForSeconds(5);

        // how many times should we localRest before getting up?
        int localRestCount;

        localRestCount = Random.Range(3, 5);

        // check our resting spot
        checkRestSpot();

        // see if our rest spot exists
        void checkRestSpot()
        {
            // if our target rest object exists
            if (targetRestSpot != null)
            {
                canRest = true;
            }
            else if (targetRestSpot == null)
            {
                canRest = false;
                // exit and choose behaviour
                ChooseBehaviour();
                // return 
                return;
            }

            // check our resting spot exists
            if (canRest == true)
            {
                StartCoroutine(localRest());
            }
        }

        IEnumerator localRest()
        {
            currentStatus.text = "Resting";
            yield return new WaitForSeconds(5);
            if (localRestCount > 0)
            {
                // lower our rest count
                localRestCount -= 1;
                // check for another rest count
                checkRestSpot();
            }
            else if (localRestCount <= 0)
            {
                // SAY WE RESTED IN THE COUNTING
                // *****************************//
                // Choose a new behaviour and exit
                ChooseBehaviour();
            }
        }


    }

    // find food
    IEnumerator EatFind()
    {
        // we're finding food
        currentStatus.text = "Finding Food";
        // store the food
        GameObject targetFood;
        // find some food
        if (objectTrackingClass.grassFeedingSpotList.Count > 0)
        {
            targetFood = objectTrackingClass.grassFeedingSpotList[Random.Range(0, objectTrackingClass.grassFeedingSpotList.Count)].gameObject;
            // move to the food
            targetPosition = targetFood.GetComponent<Transform>().position;
            targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            yield return new WaitForSeconds(1);
            // begin running a check loop
            StartCoroutine(EatCheck());
        }
        else
        {
            ChooseBehaviour();
        }
    }

    // eat check
    IEnumerator EatCheck()
    {
        yield return new WaitForSeconds(5);
        if (transform.position != targetPosition)
        {
            StartCoroutine(EatCheck());
            yield break;
        }
        else if (transform.position == targetPosition)
        {
            // spend some time eating
            currentStatus.text = "Eating";
            yield return new WaitForSeconds(Random.Range(5, 10));
            // choose a new behaviour
            ChooseBehaviour();
        }
    }
}
