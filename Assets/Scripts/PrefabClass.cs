using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabClass : MonoBehaviour
{
    // public variables
    public GameObject destructableIndicator; // thing that is activated when out object is able to be destroyed
    public CanvasController canvasController; // our canvas controller. set in the instantation of the object
    bool localBreakable;

    // when the cursor enters the trigger
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Cursor")
        {
            if (canvasController.inBreakMode == true)
            {
                destructableIndicator.SetActive(true);
                localBreakable = true;
            }
        }
    }

    // when the cursor exits the trigger
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Cursor")
        {
            destructableIndicator.SetActive(false);
        }
    }

    // when the cursor is overlapping
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Cursor")
        {
            // is the object breakable, and the mouse pressed?
            if ((localBreakable == true) && (canvasController.mousePressed == true))
            {
                canvasController.mousePressed = false;
                Destroy(gameObject);
            }
        }
    }

}
