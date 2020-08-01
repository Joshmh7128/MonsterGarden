using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableButtonScript : MonoBehaviour
{
    [SerializeField] Button thisButton;
    [SerializeField] GameObject highlightObject;
    [SerializeField] GameObject placeableObject;
    [SerializeField] ObjectPlacementScript objectPlacementScript;
    [SerializeField] CanvasController canvasController;

    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(SetObject);
    }

    void SetObject()
    {
        // destroy current highlighter if it is not the same as ours
        if (objectPlacementScript.highlightObject != highlightObject)
        {
            objectPlacementScript.DestroyHighlighter();
        }

        // set our new selection
        objectPlacementScript.highlightObject = highlightObject;
        objectPlacementScript.placeableObject = placeableObject;

        // close the build panel
        // canvasController.inBuildMode = false;
    }
}
