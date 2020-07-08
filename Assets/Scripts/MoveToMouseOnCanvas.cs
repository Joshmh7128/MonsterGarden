using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouseOnCanvas : MonoBehaviour
{
    // public variables
    public bool isUIOverlapping;
    [SerializeField] ObjectPlacementScript objectPlacementScript;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isUIOverlapping = true;
        objectPlacementScript.isUIOverlapping = isUIOverlapping;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isUIOverlapping = false;
        objectPlacementScript.isUIOverlapping = isUIOverlapping;
    }
}
