using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocalUIManager : MonoBehaviour
{
    // this script will show and hide the monster panel, and highlight it when the mouse is over it
    [SerializeField] MeshRenderer monsterSelectionHighlight;
    [SerializeField] Canvas monsterPanelCanvas;
    [SerializeField] bool canToggle;

    private void Start()
    {
        monsterPanelCanvas.enabled = false;
        canToggle = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleDisplay();
        }
    }

    // when the cursor is overlapping the monster...
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Cursor"))
        {
            // while the cursor is over our monster, make the highlight appear
            monsterSelectionHighlight.enabled = true;
            canToggle = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        // if the player clicks, toggle the UI panel
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Toggling Monster Display");
            ToggleDisplay();
            canToggle = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Cursor"))
        {
            monsterSelectionHighlight.enabled = false;
            canToggle = false;
        }
    }
    
    void ToggleDisplay()
    {
        if (canToggle == true)
        {
            monsterPanelCanvas.enabled = !monsterPanelCanvas.enabled;
        }
    }

}
