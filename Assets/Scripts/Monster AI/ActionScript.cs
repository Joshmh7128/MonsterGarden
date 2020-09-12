using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JM.LinqFaster;

public class ActionScript : MonoBehaviour
{
    // ** Scripts
    [SerializeField] protected ObjectTrackingClass objectTrackingClass; // our object tracking class

    // ** Monster Info
    [SerializeField] public Text currentStatus; // what is the current status of the deer?
    protected bool wasDisabled = false; // check to see if animal was disabled then enabled
    [SerializeField] protected float movementSpeed; // how fast can we move?
    [SerializeField] protected bool behaviourActive; // is our behaviour active?
    [SerializeField] protected float widthOfAnimal; // width of animal // todo: make sure works well with rotating
    [SerializeField] protected GameObject home; // claimed home of animal
    [SerializeField] protected List<GameObject> friendList = new List<GameObject>(3);
    [SerializeField] protected string restingSpotType; // example: "Soft-Dry"
    [SerializeField] protected string feedingSpotType; // example: "Grass"
    protected List<GameObject> restingSpotList; // list of resting spots pulling from
    protected List<FeedingSpotClass> feedingSpotList; // list of feeding spots pulling from

    // ** Movement/Herding Stats
    [SerializeField] protected float maxExploreBoundsX; // max X explore?
    [SerializeField] protected float maxExploreBoundsZ; // max Z explore?
    [SerializeField] protected float startY; // our Y for logging // todo: change around what relys on this once heights can change
    [SerializeField] protected Vector3 targetPosition; // where are we headed?
    protected float minDistBetweenHerdAnimals; // based on width of animal to avoid overlapping
    protected AreaInfoScript areaInfoScript;
    protected List<string> uninterruptableActions;
    

    public virtual void Start()
    {
        // set our Y
        startY = transform.position.y;
        // set our object tracking class
        objectTrackingClass = WorldManager.objectTrackingClass;
        minDistBetweenHerdAnimals = widthOfAnimal/Mathf.Sqrt(2);
        if (restingSpotType == "Soft-Dry") restingSpotList = objectTrackingClass.softDryRestingSpotList; // todo: make good
        if (feedingSpotType == "Grass") feedingSpotList = objectTrackingClass.grassFeedingSpotList;
        areaInfoScript = WorldManager.currentAreaInfoScript;
        areaInfoScript.MonsterList.Add(this.gameObject, this);
    }

    // explore
    protected IEnumerator Explore()
    {
        // go to random location, explore!
        targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
        currentStatus.text = "Exploring";
        yield return MoveTo(this.gameObject, targetPosition);
        yield return new WaitForSeconds(Random.Range(0, 8)); // 14
        // Debug.Log(gameObject.name + " done Exploring");
    }

    // required actions: eat at 3 grass spots, rest at one resting spot

    protected IEnumerator Rest()
    {
        // find a place to rest
        // Debug.Log(gameObject.name + " starting to rest");
        List<GameObject> tempList = restingSpotList.WhereF(x => !objectTrackingClass.objectsinUseTracking.Contains(x));
        if (tempList.Count > 0)
        {
            // get resting spot
            currentStatus.text = "Finding Resting Spot";
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
            yield return MoveTo(this.gameObject, targetPosition);
            // rest 3 - 5 times, checking if the resting spot is there in between
            currentStatus.text = "Resting";
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
        else
        {
            Debug.Log("All resting spots in use");
        }
    }

    protected IEnumerator Eat()
    {
        // we're finding food
        // Debug.Log(gameObject.name + " starting to eat");
        List<FeedingSpotClass> tempList = feedingSpotList.WhereF(x => !objectTrackingClass.objectsinUseTracking.Contains(x.gameObject) && x.hasFood);
        if (tempList != null && tempList.Count > 0)
        {
            // get food spot
            currentStatus.text = "Finding Food";
            GameObject targetFood = tempList[Random.Range(0, tempList.Count)].gameObject;
            if (targetFood == null)
            {
                Debug.Log("Target food not found");
                yield break;
            }
            // mark as in use
            objectTrackingClass.objectsinUseTracking.Add(targetFood);
            //Debug.Log(targetFood.name + " is in use by " + this.gameObject.name);
            // get food spot position and move to it
            targetPosition = new Vector3(targetFood.transform.position.x, transform.position.y, targetFood.transform.position.z);
            yield return MoveTo(this.gameObject, targetPosition);
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
                yield return Explore();
            }
        }
        else
        {
            Debug.Log("All feeding spots in use");
        }
    }

    protected IEnumerator MoveTo(GameObject gameObject, Vector3 targetPosition)
    {
        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.000001)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return 0;
        }
        yield return true;
    }

    // Todo: Zone of Clare needs to set up the requirements
    // ** If certain requirements are met; get what they are
    protected void ClaimHome(GameObject claimedHome)
    {
        objectTrackingClass.objectsinUseTracking.Add(claimedHome);
        home = claimedHome;
    }

    protected void LeaveGarden()
    {

    }

    protected void MakeFriend(GameObject newFriend)
    {
        if (!friendList.Contains(newFriend)) friendList.Add(newFriend);
    }

    protected void RemoveFriend(GameObject friend)
    {
        friendList.Remove(friend);
    }

    protected IEnumerator Herding(List<GameObject> herdingFollowers)
    {
        // todo: make sure position isnt on top of objects
        if (herdingFollowers.Count != 0)
        {    
            // List of coroutines currently taking place within this method, used to make sure the monsters wait for the other monsters to reach certain positions
            List<CoroutineWithData> waitList = new List<CoroutineWithData>();
            // List of 4 positions in a square around the lead monster
            List<Vector3> posList = new List<Vector3>()
            {
                new Vector3(transform.position.x + minDistBetweenHerdAnimals, transform.position.y, transform.position.z + minDistBetweenHerdAnimals),
                new Vector3(transform.position.x - minDistBetweenHerdAnimals, transform.position.y, transform.position.z + minDistBetweenHerdAnimals),
                new Vector3(transform.position.x + minDistBetweenHerdAnimals, transform.position.y, transform.position.z - minDistBetweenHerdAnimals),
                new Vector3(transform.position.x - minDistBetweenHerdAnimals, transform.position.y, transform.position.z - minDistBetweenHerdAnimals)
            };
            currentStatus.text = "Leading Herd";
            foreach (GameObject follower in herdingFollowers)
            {
                // get the closest spot in the position list
                posList.OrderByF(x => Vector3.Distance(x, follower.transform.position));
                // choose a random position within a sphere (big enough for no overlap) around the chosen spot 
                Vector3 lerpCirclePosition = posList[0] + (Random.insideUnitSphere*widthOfAnimal/2);
                // since its a random spot in a sphere, need to fix y
                lerpCirclePosition.y = transform.position.y;
                // add coroutine to waitlist so the monsters wait until they are in herd position
                waitList.Add(new CoroutineWithData(this, MoveTo(follower, lerpCirclePosition)));
                posList.RemoveAt(0);
            }
            foreach(CoroutineWithData c in waitList) // wait until all monsters are in herd position // todo: see if the whole coroutine with data can be improved
            {
                yield return new WaitUntil(() => c.done);
            }
            waitList.Clear();
            // get random spot to travel as a pack to (explore together)
            targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
            yield return new WaitForSeconds(1);
            // make monsters wait until they get to the randomly chosen spot ---
            foreach(GameObject follower in herdingFollowers)
            {
                waitList.Add(new CoroutineWithData(this, MoveTo(follower, follower.transform.position + targetPosition - transform.position)));
            }
            waitList.Add(new CoroutineWithData(this, MoveTo(this.gameObject, targetPosition)));
            foreach(CoroutineWithData c in waitList)
            {
                yield return new WaitUntil(() => c.done);
            }
            yield return new WaitForSeconds(2); // ** edit timing as needed
            // -------------------------------------------------------------------
            foreach(GameObject friend in herdingFollowers)
            {
                // Restart Choose Behavior by disabling/enabling
                friend.SetActive(false);
                friend.SetActive(true);
            }
        }
        else
        {
            Debug.Log("No followers available");
        }
    }

    
    protected IEnumerator TurfDispute(GameObject enemy) // TODO: do it
    {
        yield return 0;
        // Do actions then...
        // Restart Choose Behavior by disabling/enabling
        // todo: make sure turf war doesn't start on top of objects
        targetPosition = new Vector3(Random.Range(-transform.position.x+5, transform.position.x+5), startY, Random.Range(-transform.position.z+5, transform.position.z+5));

        enemy.SetActive(false);
        enemy.SetActive(true);
    }


    public class CoroutineWithData // https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
    {
        public Coroutine coroutine { get; private set; }
        public IEnumerator target;
        public bool done;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            done = false;
            while(target.MoveNext())
            {
                yield return target.Current;
            }
        done = true;
        }
    }

    public void StopCurrentCoroutines()
    {
        StopAllCoroutines();
    }

    void OnDisable()
    {
        if (!wasDisabled) wasDisabled = true;
    }

    protected List<GameObject> InterruptableMonsters(List<GameObject> list)
    {
        return list.WhereF(x => !areaInfoScript.MonsterList[x].uninterruptableActions.Contains(areaInfoScript.MonsterList[x].currentStatus.text));
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