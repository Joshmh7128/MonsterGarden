using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObjectScript : MonoBehaviour
{
    [SerializeField] Transform highlightCursor;
    [SerializeField] ObjectPlacementScript objectPlacementScript;
    [SerializeField] float rot = 0; // our rotation

    // start
    private void Start()
    {
        // get our required objects and scripts
        // where is our highlight cursor?
        highlightCursor = GameObject.Find("Placeable Cursor").GetComponent<Transform>();
        // where is our objectplacementscript?
        objectPlacementScript = GameObject.Find("Main Camera").GetComponent<ObjectPlacementScript>();
    }

    // Fixed Update is called once per frame
    void FixedUpdate()
    {
        transform.position = highlightCursor.position;

        objectPlacementScript.highlightObejctTransform = transform;

        // rotate right
        if (Input.GetKey(KeyCode.E))
        {
            rot += 1;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);
        }

        // rotate left
        if (Input.GetKey(KeyCode.Q))
        {
            rot -= 1;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        // check the tag of an object that's been placed, if it's overlapping, set placing to false
        if (col.tag == "PlaceableObject")
        {
            objectPlacementScript.isObjectOverlapping = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        // check the tag of an object that's been placed, if it's overlapping, set placing to false
        if (col.tag == "PlaceableObject")
        {
            objectPlacementScript.isObjectOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // check the tag of an object that's been placed, if it's overlapping, set placing to false
        if (col.tag == "PlaceableObject")
        {
            objectPlacementScript.isObjectOverlapping = false;
        }
    }
}
