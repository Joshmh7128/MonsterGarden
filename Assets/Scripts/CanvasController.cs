using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // our UI elements
    [SerializeField] Animator buildPanel;
    [SerializeField] Button buildButton;
    [SerializeField] Button breakButton;

    // gameplay variables
    public bool inBuildMode;
    [SerializeField] ObjectPlacementScript objectPlacementScript;

    public GameObject[] placeableObjectsGrassland;
    public GameObject[] highlightObjectsGrassland;
    public Button[] placeableButtonsGrassland;

    // Start is called before the first frame update
    void Start()
    {
        // setup our buttons
        buildButton.onClick.AddListener(ShowBuildPanel);

        int i = -1;

        foreach (Button button in placeableButtonsGrassland)
        {
            button.onClick.AddListener(delegate { SetPlaceableObjectGrassland(i); });
            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuildMode == true)
        {
            buildPanel.Play("Build Panel In");
        }

        if (inBuildMode == false)
        {
            buildPanel.Play("Build Panel Out");
        }
    }

    // show our build panel
    void ShowBuildPanel()
    {
        // toggle build mode
        inBuildMode = !inBuildMode;
        objectPlacementScript.inBuildMode = inBuildMode;
    }

    void SetPlaceableObjectGrassland(int setObject)
    {
        // set the placeable object to the same value as the object's position in it's own array
        objectPlacementScript.placeableObject = placeableObjectsGrassland[setObject];
        objectPlacementScript.highlightObject = highlightObjectsGrassland[setObject];
    }
}
