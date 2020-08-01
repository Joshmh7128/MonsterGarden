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
    [SerializeField] Button buildButton;
    [SerializeField] Button breakButton;
    [SerializeField] Button clostPanelButton;

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
        clostPanelButton.onClick.AddListener(EscapeConstruction);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeConstruction();
        }

        if (showBuildPanel == true)
        {
            buildPanel.Play("Build Panel In");
            buildButtonUI.Play("BuildButtonOut");
        }

        if (showBuildPanel == false)
        {
            buildPanel.Play("Build Panel Out");
            buildButtonUI.Play("BuildButtonIn");
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
            inBreakMode = false;
            objectPlacementScript.inBuildMode = inBuildMode;
        }

        if (inBreakMode == true)
        {
            inBreakMode = false;
            objectPlacementScript.inBuildMode = inBuildMode;
        }
    }
}
