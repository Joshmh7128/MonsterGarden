using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // our UI elements
    [SerializeField] Animator buildPanel;
    [SerializeField] Animator breakPanel;
    [SerializeField] Animator buildButtonUI;
    [SerializeField] Animator breakButtonUI;
    [SerializeField] Button buildButton;
    [SerializeField] Button breakButton;
    [SerializeField] Button closePanelButton;

    // gameplay variables
    public bool inBuildMode;
    public bool showBuildPanel;
    public bool inBreakMode;

    // mouse variables
    public bool mousePressed;

    // helper script holding
    [SerializeField] ObjectPlacementScript objectPlacementScript;
    public ObjectTrackingClass objectTrackingClass; // !! must be set !!

    // Start is called before the first frame update
    void Start()
    {
        // setup our buttons
        buildButton.onClick.AddListener(ShowBuildPanel);
        breakButton.onClick.AddListener(ToggleBreak);
        closePanelButton.onClick.AddListener(EscapeConstruction);
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

        // check to see if esc is pressed to leave build mode or break mode
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetMouseButtonDown(1)))
        {
            EscapeConstruction();
        }

        if (showBuildPanel == true)
        {
            buildPanel.Play("Build Panel In");
            buildButtonUI.Play("BuildButtonOut");
            breakButtonUI.Play("BreakButtonOut");
        }

        if (showBuildPanel == false)
        {
            buildPanel.Play("Build Panel Out");
            inBuildMode = false;
        }

        if (inBreakMode == true)
        {
            breakPanel.Play("Break Panel In");
            buildButtonUI.Play("BuildButtonOut");
        }

        if ((inBreakMode == false) && (showBuildPanel == false))
        {
            breakPanel.Play("Break Panel Out");
            buildButtonUI.Play("BuildButtonIn");
            breakButtonUI.Play("BreakButtonIn");
        }
    }

    // show our build panel
    void ShowBuildPanel()
    {
        Debug.Log("Show Panel Requested");
        if (inBreakMode == false)
        {
            // toggle build mode
            inBuildMode = true;
            showBuildPanel = true;
            // set our object Placement Script to be in build mode
            objectPlacementScript.inBuildMode = inBuildMode;
        }
    }

    void CloseBuildPanel()
    {
        showBuildPanel = false;
        inBuildMode = false;
        // set our object Placement Script to be in build mode
        objectPlacementScript.inBuildMode = inBuildMode;
    }

    void ToggleBreak()
    {
        Debug.Log("Break Mode Toggle Requested");
        if (inBuildMode == false)
        {
            // toggle break mode
            inBreakMode = !inBreakMode;
            // set our orb cursor to break mode
        }
    }

    void EscapeConstruction()
    {
        if (showBuildPanel == false)
        {
            inBuildMode = false;
            showBuildPanel = false;
            inBreakMode = false;
            objectPlacementScript.inBuildMode = inBuildMode;
        }

        if (inBuildMode == true)
        {
            showBuildPanel = false;
            inBuildMode = false;
            objectPlacementScript.inBuildMode = inBuildMode;
        }

        if (inBreakMode == true)
        {
            inBreakMode = false;
            objectPlacementScript.inBuildMode = inBuildMode;
        }
    }
}
