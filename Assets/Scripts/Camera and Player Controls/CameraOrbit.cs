using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Transform _xForm_Camera;
    private Transform _xForm_Parent;

    private Vector3 _LocalRotation;
    private float _CameraDistance = 10f;

    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;

    public bool cameraDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        // set our camera positions
        this._xForm_Camera = this.transform;
        this._xForm_Parent = this.transform.parent;
        _LocalRotation.y = 10;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            cameraDisabled = !cameraDisabled;
        }

        if (!cameraDisabled)
        {
            // Rotation of the camera based on Mouse Coordinates
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && Input.GetMouseButton(2))
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                // Clamp y rotation to horizon so it doesn't flip at top
                _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 10f, 90f);
            }

            // Zooming input from our mouse scroll wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                // Makes camera zoom faster the further away it is from the target
                scrollAmount *= (this._CameraDistance * 0.3f);

                this._CameraDistance += scrollAmount * -1f;

                // Camera will go no closer than 1.5 from the target, and no further than 100
                this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
            }

            // Actual camera rig rotations
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            this._xForm_Parent.rotation = Quaternion.Lerp(this._xForm_Parent.rotation, QT, Time.deltaTime * orbitDampening);

            if (this._xForm_Camera.localPosition.z != this._CameraDistance * -1f)
            {
                this._xForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._xForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * scrollDampening));
            }
        }
    }
}
