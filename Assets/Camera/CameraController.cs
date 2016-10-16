using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public float panSpeed = 0.1f;

    public Transform focusPoint;
    public float distanceFromFocusPoint = 40f;

    public float zTiltSpeed = 15;
    public float zTiltSmoothTime = 0.1f;
    public float zTiltMinimumMagnitude = 0.25f;

    [Range(0, 90)]
    public float xTiltMinAngle = 15;
    [Range(0, 1)]
    public float xTiltDefaultValue = 0.25f;
    public float xTiltSmoothTime = 0.5f;
    public float xTileDeviceFlatMin = 0.8f;

    Vector3 vAxis = Vector3.down;
    Vector3 hAxis = Vector3.forward;

    void Update() {
        #if UNITY_EDITOR
        fauxAccelerationUpdate();
        #endif

        // panning on touch drag
        List<Vector3> drags = InputManager.GetDrags();
        if (drags.Count > 0) {
            Vector3 offset = Vector3.zero;
            foreach (Vector3 pan in drags) {
                offset.x += pan.x * panSpeed;
                offset.z += pan.y * panSpeed;
            }
            focusPoint.position += offset / drags.Count;
        }
        
        // get device tilt values
        float xTilt = GetDeviceXTilt();
        float zTilt = GetDeviceZTilt();

        // sideways device tilting
        hAxis = Quaternion.Euler(0, zTilt, 0) * hAxis;

        // back and forth device tilting
        Vector3 minAngleVector = hAxis;
        minAngleVector.y = -Mathf.Tan(Mathf.Deg2Rad * xTiltMinAngle);
        minAngleVector.Normalize();

        Quaternion minRot = Quaternion.LookRotation(minAngleVector);
        Quaternion maxRot = Quaternion.LookRotation(vAxis, hAxis);

        transform.rotation = Quaternion.Slerp(minRot, maxRot, xTilt);
        transform.position = transform.rotation * Vector3.back * distanceFromFocusPoint;
        transform.position += focusPoint.position;

        // rotate focus point as well
        focusPoint.localRotation = transform.localRotation;
        Vector3 originalRotation = focusPoint.localEulerAngles;
        originalRotation.x = 0;
        focusPoint.localEulerAngles = originalRotation;
    }

    #if UNITY_EDITOR
    Vector3 fauxAcceleration = new Vector3(0, 1, 0.25f);
    float speed = 1;
    void fauxAccelerationUpdate() {
        fauxAcceleration += new Vector3(
            (Input.GetKey(KeyCode.U) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0),
            (Input.GetKey(KeyCode.O) ? 1 : 0) - (Input.GetKey(KeyCode.L) ? 1 : 0)
        ) * speed * Time.deltaTime;

        fauxAcceleration.x = Mathf.Clamp(fauxAcceleration.x, -1.0f, 1.0f);
        fauxAcceleration.y = Mathf.Clamp(fauxAcceleration.y, -1.0f, 1.0f);
        fauxAcceleration.z = Mathf.Clamp(fauxAcceleration.z, -1.0f, 1.0f);
    }
    #endif

    float oldXTilt = 0;
    float xTiltSmoothVelocity = 0;
    float GetDeviceXTilt() {
        Vector3 accel = GetAcceleration();

        if (accel.sqrMagnitude == 0) {
            return xTiltDefaultValue;
        } else {
            bool isDeviceFlat = (1 - Mathf.Abs(Mathf.Atan(accel.y / accel.z)) / (Mathf.PI / 2)) >= xTileDeviceFlatMin;
            float newXTilt = isDeviceFlat ? 1 : xTiltDefaultValue;
            oldXTilt = Mathf.SmoothDamp(oldXTilt, newXTilt, ref xTiltSmoothVelocity, xTiltSmoothTime);
            return oldXTilt;
        }
    }

    float oldZTilt = 0;
    float zTiltSmoothVelocity = 0;
    float GetDeviceZTilt() {
        Vector3 accel = GetAcceleration();

        float newZTilt = Mathf.Abs(-accel.x) < zTiltMinimumMagnitude
            ? 0
            : accel.x;
        oldZTilt = Mathf.SmoothDamp(oldZTilt, newZTilt, ref zTiltSmoothVelocity, zTiltSmoothTime);
        return oldZTilt;
    }
    
    Vector3 GetAcceleration() {
        #if UNITY_EDITOR
        //UILogger.Log("accel:", fauxAcceleration);
        return fauxAcceleration;
        #else
        //UILogger.Log("accel:", Input.acceleration);
        return Input.acceleration;
        #endif
    }

}
