using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObjectScript : MonoBehaviour
{
    [SerializeField] Transform highlightCursor;

    // start
    private void Start()
    {
        highlightCursor = GameObject.Find("Placeable Cursor").GetComponent<Transform>();
    }

    // Fixed Update is called once per frame
    void FixedUpdate()
    {
        transform.position = highlightCursor.position;
    }
}
