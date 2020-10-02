using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocalUIManager : MonoBehaviour
{
    // this script will show and hide the monster panel, and highlight it when the mouse is over it
    [SerializeField] MeshRenderer monsterSelectionHighlight;
    [SerializeField] Canvas monsterPanelCanvas;
    [SerializeField] bool canToggle;
    [SerializeField] ActionScript ourAI;

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
            // set our monster's movement speed to 0 so that the player can access their panel
            ourAI.movementSpeed = 0;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        canToggle = true;
        ourAI.movementSpeed = 0;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Cursor"))
        {
            monsterSelectionHighlight.enabled = false;
            canToggle = false;
            ourAI.movementSpeed = 1;
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
