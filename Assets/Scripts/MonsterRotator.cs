using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRotator : MonoBehaviour
{
    // this script is for rotating monsters
    GameObject CameraLerper;

    // Start is called before the first frame update
    void Start()
    {
        CameraLerper = GameObject.Find("Camera Lerper");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = CameraLerper.transform.rotation;
    }
}
