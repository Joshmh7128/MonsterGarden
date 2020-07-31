using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This script is going to move the player's camera around
    private float rotationX = 0.0f;
    private float startY;
    [SerializeField] private float normalMoveSpeed;
    [SerializeField] private float fastMoveFactor;
    [SerializeField] private float slowMoveFactor;
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private Transform lerperObject;
    [SerializeField] private Transform cameraObject;
    
    // Start is called before the first frame update
    void Start()
    {
        // set our start y
        startY = transform.position.y;
    }

    private void FixedUpdate()
    {
        // get our movement
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // apply our start y
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
}
