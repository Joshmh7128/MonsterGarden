using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasslandSpawnerScript : MonoBehaviour
{
    // this script runs individual checks for each list of objects based upon it's criteria
    List<GameObject> spawnableMonsters;
    GameObject chosenMonster;
    ObjectTrackingClass objectTrackingClass;
    [SerializeField] float maxExploreBoundsX;
    [SerializeField] float maxExploreBoundsY;

    void Start()
    {
        objectTrackingClass = WorldManager.objectTrackingClass;
    }

    void DecideMonsters()
    {
        chosenMonster = spawnableMonsters[Random.Range(0, spawnableMonsters.Count)];
    }

    IEnumerator SpawnMonster()
    {
        Debug.Log("Spawning monster in t-30 seconds");
        yield return new WaitForSeconds(Random.Range(28, 32));
        Instantiate(chosenMonster, transform.position, Quaternion.identity);
    }

}
