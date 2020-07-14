using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterAI : MonoBehaviour
{
    // Basic Monster AI Script
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float averageY;
    [SerializeField] float movementRange;
    [SerializeField] float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // set our average Y so that our monsters don't awkwardly move around if they are animated
        averageY = transform.position.y;
        StartCoroutine("RandomMovement");
    }

    // find a random place on the map and move to it
    IEnumerator RandomMovement()
    {
        yield return new WaitForSeconds(Random.Range(10,30));
        targetPosition = new Vector3(Random.Range(-movementRange, movementRange), transform.position.y, Random.Range(-movementRange, movementRange));
        StartCoroutine("RandomMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move to our position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
