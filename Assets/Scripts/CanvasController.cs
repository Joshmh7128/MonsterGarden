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

    // Start is called before the first frame update
    void Start()
    {
        // setup our buttons
        buildButton.onClick.AddListener(ShowBuildPanel);
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
        // set our object Placement Script to be in build mode
        objectPlacementScript.inBuildMode = inBuildMode;
    }
}
