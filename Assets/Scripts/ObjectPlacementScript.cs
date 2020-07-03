using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementScript : MonoBehaviour
{
    public GameObject placeableObject;
    public GameObject highlightObject;
    public GameObject previousObject;
    public GameObject currentHighlighter;
    [SerializeField] private GameObject placementCursor;
    public bool inBuildMode;

    private void Start()
    {
        previousObject = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // get our mouses world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // cast a ray and check out the floor
        if (Physics.Raycast(ray, out hitInfo))
        {
            placementCursor.transform.position = hitInfo.point;
            placementCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

        if (inBuildMode == true)
        {

            if (currentHighlighter != null)
            {
                // set our object to be active
                currentHighlighter.SetActive(true);
            }

            if (highlightObject != previousObject)
            {
                currentHighlighter = Instantiate(highlightObject, placementCursor.transform.position, Quaternion.identity);
                // highlightObjectLocal.transform.position = placementCursor.transform.position;
                previousObject = highlightObject;
            }

            highlightObject.transform.position = placementCursor.transform.position;
        }

        if (inBuildMode == false)
        {
            // set our object to be inactive
            if (currentHighlighter != null)
            {
                currentHighlighter.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // when we're moving the camera, turn off the renderer of our icon
        if (Input.GetMouseButtonDown(2) || (Input.GetMouseButtonDown(1)))
        {
            placementCursor.GetComponent<Renderer>().enabled = false;
        }

        if (Input.GetMouseButtonUp(2) || (Input.GetMouseButtonUp(1)))
        {
            placementCursor.GetComponent<Renderer>().enabled = true;
        }

        if (inBuildMode == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(placeableObject, placementCursor.transform.position, Quaternion.identity);
            }
        }
    }
}
