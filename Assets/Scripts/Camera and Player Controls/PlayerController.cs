using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This script is going to move the player's camera around
    private float rotationX = 0.0f;
    [SerializeField] private float normalMoveSpeed;
    [SerializeField] private float fastMoveFactor;
    [SerializeField] private float slowMoveFactor;
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private Transform lerperObject;
    [SerializeField] private Transform cameraObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

        if (Input.GetKey(KeyCode.E))
        {
            rotationX -= (cameraSensitivity / 2) * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotationX += (cameraSensitivity / 2) * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        }


        // camera rotation
        if (Input.GetMouseButton(2) || (Input.GetMouseButton(1)))
        {
            // mouse movement left
            if (Input.GetAxis("Mouse X") < 0)
            {
                rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            }

            // mouse movement right
            if (Input.GetAxis("Mouse X") > 0)
            {
                rotationX -= Input.GetAxis("Mouse X") * -cameraSensitivity * Time.deltaTime;
            }

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (cameraObject.position.y > 2)
        {
            //Debug.Log("Y over 2");
        }*/

            // camera zooming in / out
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (cameraObject.position.y < 9)
            {
                lerperObject.localPosition -= new Vector3(0, -0.2f, 1);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (cameraObject.position.y > 2)
            {
                lerperObject.localPosition += new Vector3(0, -0.2f, 1);
            }
        }
    }
}
