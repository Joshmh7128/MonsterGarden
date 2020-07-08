using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // our UI elements
    [SerializeField] Animator buildPanel;
    [SerializeField] Animator breakPanel;
    [SerializeField] Button buildButton;
    [SerializeField] Button breakButton;

    // gameplay variables
    public bool inBuildMode;
    public bool inBreakMode;

    // mouse variables
    public bool mousePressed;

    [SerializeField] ObjectPlacementScript objectPlacementScript;

    // Start is called before the first frame update
    void Start()
    {
        // setup our buttons
        buildButton.onClick.AddListener(ShowBuildPanel);
        breakButton.onClick.AddListener(ToggleBreak);
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if our mouse is pressed or not
        if (Input.GetMouseButtonDown(0))
        {
            mousePressed = true;
        }        
       
        if (Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
        }


        if (inBuildMode == true)
        {
            buildPanel.Play("Build Panel In");
        }

        if (inBuildMode == false)
        {
            buildPanel.Play("Build Panel Out");
        }

        if (inBreakMode == true)
        {
            breakPanel.Play("Break Panel In");
        }

        if (inBreakMode == false)
        {
            breakPanel.Play("Break Panel Out");
        }
    }

    // show our build panel
    void ShowBuildPanel()
    {
        if (inBreakMode == false)
        {
            // toggle build mode
            inBuildMode = !inBuildMode;
            // set our object Placement Script to be in build mode
            objectPlacementScript.inBuildMode = inBuildMode;
        }
    }

    void ToggleBreak()
    {
        if (inBuildMode == false)
        {
            // toggle break mode
            inBreakMode = !inBreakMode;
            // set our orb cursor to break mode
        }
    }
}
