using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementScript : MonoBehaviour
{
    // objects
    public GameObject placeableObject;
    public GameObject highlightObject;
    public GameObject previousObject;
    public GameObject currentHighlighter;
    // UI
    [SerializeField] private GameObject placementCursor;
    // bools
    public bool inBuildMode;
    public bool isUIOverlapping;
    // canvas
    [SerializeField] CanvasController canvasController; // MUST BE SET IN EDITOR OTHERWISE OBJECTS WILL NOT INSTANTIATE

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

            // make sure our highlight object exists, and then move it to the cursor
            if (highlightObject != null)
            {
                highlightObject.transform.position = placementCursor.transform.position;
            }
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

        if ((inBuildMode == true) && (isUIOverlapping == false))
        {

            if (Input.GetMouseButtonDown(0))
            {
                // place the object
                GameObject placedObject = Instantiate(placeableObject, placementCursor.transform.position, Quaternion.identity);
                placedObject.GetComponent<PrefabClass>().canvasController = canvasController;
            }
        }
    }

    public void DestroyHighlighter()
    {
        Destroy(currentHighlighter);
    }
}
