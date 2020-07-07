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

    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(SetObject);
    }

    void SetObject()
    {
        // destroy current highlighter
        objectPlacementScript.DestroyHighlighter();
        // set our new selection
        objectPlacementScript.highlightObject = highlightObject;
        objectPlacementScript.placeableObject = placeableObject;
    }

}
