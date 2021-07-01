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
    [SerializeField] protected GameObject home = null; // claimed home of animal
    [SerializeField] protected List<GameObject> friendList = new List<GameObject>(3);
     public string restingSpotType; // example: "Soft-Dry"
    [SerializeField] protected string feedingSpotType; // example: "Grass"
    protected List<GameObject> restingSpotList; // list of resting spots pulling from
    protected List<FeedingSpotClass> feedingSpotList; // list of feeding spots pulling from
    [SerializeField] protected int powerNumber; // higher number, higher chance of winning
    [SerializeField] protected int aggressionPercent; // whole number out of 100; // ** TEMPORARY THING!!!
    protected bool ateOrRestedOnce = false;
    protected int happiness; // TODO: set up
    protected bool interested; // TODO: set up

    // ** Movement/Herding Stats
    [SerializeField] protected float maxExploreBoundsX; // max X explore?
    [SerializeField] protected float maxExploreBoundsZ; // max Z explore?
    [SerializeField] protected float startY; // our Y for logging // todo: change around what relys on this once heights can change
    [SerializeField] protected Vector3 targetPosition; // where are we headed?
    protected float minDistBetweenHerdAnimals; // based on width of animal to avoid overlapping
    protected AreaInfoScript areaInfoScript;
    public List<string> uninterruptableActions;
    

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
            if (!ateOrRestedOnce)
            {
                ateOrRestedOnce = true;
                yield return LookForHome();
            }
        }
        else
        {
            Debug.Log("All resting spots in use");
        }
    }

    protected IEnumerator RestAtHome()
    {
        yield return MoveTo(this.gameObject, targetPosition);
        // rest 3 - 5 times, checking if the resting spot is there in between
        currentStatus.text = "Resting";
        for (int i = 0; i< Random.Range(3,6); i++)
        {
            if (home == null) // if spot has been deleted, start exploring
            {
                yield break;
            }
            yield return new WaitForSeconds(5);
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
                if (!ateOrRestedOnce)
                {
                    ateOrRestedOnce = true;
                    yield return LookForHome();
                }
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
            yield return null;
        }
        yield return true;
    }

    // Todo: Zone of Clare needs to set up the requirements
    // ** If certain requirements are met; get what they are
    protected void ClaimHome(GameObject claimedHome)
    {
        objectTrackingClass.claimedHomes[claimedHome] = this.gameObject;
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
    
    protected IEnumerator LookForHome()
    {
        yield return null;
        List<GameObject> tempList = restingSpotList.WhereF(x => !objectTrackingClass.claimedHomes.ContainsKey(x));
        GameObject newHome = tempList[Random.Range(0, tempList.Count)];
        ClaimHome(newHome); // ******* delay issues? two monsters going for same home issue? 
        while (objectTrackingClass.objectsinUseTracking.Contains(newHome))
        {
            yield return null;
        }
        objectTrackingClass.objectsinUseTracking.Add(newHome);
        yield return RestAtHome();
    }

    protected IEnumerator Herding()
    {
        // todo: make sure position isnt on top of objects
        yield return new WaitForEndOfFrame();
        List<GameObject> herdingFollowers = new List<GameObject>(); // monsters following this monster
        foreach(GameObject go in friendList) // ** change to be other monsters outside of friendlist maybe?
        {
            ActionScript temp = areaInfoScript.MonsterList[go];
            if (!temp.uninterruptableActions.Contains(temp.currentStatus.text))
            {
                herdingFollowers.Add(go);
                temp.currentStatus.text = "Herding";
                temp.StopCurrentCoroutines();
            }
        }
        if (herdingFollowers.Count != 0)
        {    
            // List of coroutines currently taking place within this method, used to make sure the monsters wait for the other monsters to reach certain positions
            List<Coroutine> corList = new List<Coroutine>();
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
                corList.Add(StartCoroutine(MoveTo(follower, lerpCirclePosition)));
                posList.RemoveAt(0);
            }
            foreach(Coroutine cor in corList) // wait until all monsters are in herd position
            {
                yield return cor;
            }
            corList.Clear();
            // get random spot to travel as a pack to (explore together)
            targetPosition = new Vector3(Random.Range(-maxExploreBoundsX, maxExploreBoundsX), startY, Random.Range(-maxExploreBoundsZ, maxExploreBoundsZ));
            yield return new WaitForSeconds(1);
            // make monsters wait until they get to the randomly chosen spot ---
            foreach(GameObject follower in herdingFollowers)
            {
                corList.Add(StartCoroutine(MoveTo(follower, follower.transform.position + targetPosition - transform.position)));
            }
            corList.Add(StartCoroutine(MoveTo(this.gameObject, targetPosition)));
            foreach(Coroutine cor in corList)
            {
                yield return cor;
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

    
    protected IEnumerator TurfDispute()
    {
        // todo: make sure turf war doesn't start on top of objects

        // Choose an enemy -------------------------------------
        GameObject enemy;
        bool lookingForHome = false; // fighting for home?
        if (home != null)
        {
            List<Collider> monstersCloseBy = new List<Collider>(Physics.OverlapSphere(
                new Vector3(transform.position.x, startY, transform.position.z), maxExploreBoundsX/3, 1<<8));
            // Sees what monsters are around in a sphere of 1/3 the size of walkable area in area
            if (monstersCloseBy != null && monstersCloseBy.Count > 0) // if monsters around
            {
                enemy = monstersCloseBy[Random.Range(0,monstersCloseBy.Count)].gameObject; // choose a random monster
            }
            else // if no monsters around, exit action
            {
                yield break;
            }
        }
        else
        {
            List<GameObject> temp = new List<GameObject>(areaInfoScript.MonsterList.Keys).WhereF(
                x => areaInfoScript.MonsterList[x].restingSpotType == restingSpotType); // if monster has same house type as you
            if (temp != null && temp.Count > 0)
            {
                enemy = temp[Random.Range(0,temp.Count)]; // get random enemy of monster has same house type
                lookingForHome = true;
            }
            else // if no monsters around, exit action
            {
                yield break;
            }
        }
        ActionScript enemyActionScript = areaInfoScript.MonsterList[enemy];
        enemyActionScript.currentStatus.text = "Getting Fought";
        currentStatus.text = "Fighting";
        enemyActionScript.StopAllCoroutines();
        // -------------------------------------------------------------------

        // Walk over to at least 2 distance away from the monster // ** edit if needs to be move exact for animation purposes
        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 2)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        // Action Time
        if (lookingForHome)
        {
            yield return new WaitForSeconds(5); // ** placeholder for animation of fighting
            float chanceOfWinning = powerNumber / (enemyActionScript.powerNumber + powerNumber);
            if (Random.Range(0f,1f) <= chanceOfWinning) // if won, steal home
            {
                ClaimHome(enemyActionScript.home);
            }
            else // if lost
            {
                // Maybe be bummed for a little bit?
            }
        }
        else
        {
            if (Random.Range(0f,1f) <= (aggressionPercent / 100))
            {
                yield return new WaitForSeconds(5); // ** placeholder for animation of fighting
            }
            else
            {
                yield return new WaitForSeconds(3); // ** do animation of this animal trying to fight and that enemy walking away
            }
        }

        // Restart Choose Behavior by disabling/enabling
        enemy.SetActive(false);
        enemy.SetActive(true);
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
                yield return null;
            }*/ /*
            while (Vector3.Distance(transform.position, targetPosition) > 0.000001) // go until in the middle of hex
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }
        }
        yield return null;
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
        yield return null;
    } */
}