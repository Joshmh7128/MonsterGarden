using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionScript : MonoBehaviour
{
    void Start()
    {
        WorldManager.actionScript = this;
    }
    
    // explore
    public IEnumerator Explore(GameObject monster, Stats monsterStats)
    {
        // go to random location, explore!
        monsterStats.targetPosition = new Vector3(Random.Range(-monsterStats.maxExploreBoundsX, monsterStats.maxExploreBoundsX), 
            monster.transform.position.y, Random.Range(-monsterStats.maxExploreBoundsZ, monsterStats.maxExploreBoundsZ));
        monsterStats.currentStatus.text = "Exploring";
        yield return StartCoroutine(MoveTo(monster, monsterStats));
        yield return new WaitForSeconds(Random.Range(0, 8)); // 14
    }

    // required actions: eat at 3 grass spots, rest at one resting spot

    public IEnumerator Rest(GameObject monster, Stats monsterStats)
    {
        // find a place to rest
        monsterStats.currentStatus.text = "Finding Resting Spot";
        if (monsterStats.restingSpotList.Count > 0)
        {
            // get resting spot
            GameObject targetRestSpot = monsterStats.restingSpotList[Random.Range(0, monsterStats.restingSpotList.Count)];
            if (targetRestSpot == null)
            {
                Debug.Log("Resting spot not found"); // *** could change to keep looking for instead of leaving
                yield break;
            }
            // get position of resting spot
            monsterStats.targetPosition = new Vector3(targetRestSpot.transform.position.x, monster.transform.position.y, targetRestSpot.transform.position.z);
            // move to resting spot
            yield return MoveTo(monster, monsterStats);
            // rest 3 - 5 times, checking if the resting spot is there in between
            for (int i = 0; i< Random.Range(3,6); i++)
            {
                if (targetRestSpot == null) // if spot has been deleted, start exploring
                {
                    yield return StartCoroutine(Explore(monster, monsterStats));
                    yield break;
                }
                monsterStats.currentStatus.text = "Resting";
                yield return new WaitForSeconds(5);
            }
        }
    }

    public IEnumerator Eat(GameObject monster, Stats monsterStats)
    {
        // we're finding food
        monsterStats.currentStatus.text = "Finding Food";
        if (monsterStats.feedingSpotList.Count > 0)
        {
            // get food spot
            GameObject targetFood = monsterStats.feedingSpotList[Random.Range(0, monsterStats.feedingSpotList.Count)].gameObject;
            if (targetFood == null)
            {
                Debug.Log("Target food not found");
                yield break;
            }
            // get food spot position and move to it
            monsterStats.targetPosition = new Vector3(targetFood.transform.position.x, monster.transform.position.y, targetFood.transform.position.z);
            yield return StartCoroutine(MoveTo(monster, monsterStats));
            // if food is still there when you get there, eat
            if (targetFood != null)
            {
                monsterStats.currentStatus.text = "Eating";
                yield return new WaitForSeconds(Random.Range(5, 10));
            }
            else // if the food is gone start exploring
            {
                yield return StartCoroutine(Explore(monster, monsterStats));
            }
        }
    }

    IEnumerator MoveTo(GameObject monster, Stats monStats)
    {
        while (Vector3.Distance(monster.transform.position, monStats.targetPosition) > 0.000001) // go until in the middle of hex
        {
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, monStats.targetPosition, monStats.movementSpeed * Time.deltaTime);
            yield return 0;
        }
    }
}