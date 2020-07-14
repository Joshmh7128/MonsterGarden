using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerpScript : MonoBehaviour
{
    // this script is for camera lerping
    GameObject targetPosition;
    // start
    private void Start()
    {
        targetPosition = GameObject.Find("Camera Lerper");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition.transform.TransformPoint(Vector3.zero), 0.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition.transform.rotation, Time.time * 0.1f);
    }
}
