using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 focusPoint = Vector3.zero;
    public float distanceFromFocusPoint = 30f;

    public float zTiltSpeed = 15;
    public float zTiltSmoothTime = 0.1f;
    public float zTiltMinimumMagnitude = 0.25f;

    [Range(0, 90)]
    public float xTiltMinAngle = 15;
    [Range(0, 1)]
    public float xTiltDefaultValue = 0.25f;
    public float xTiltSmoothTime = 0.2f;

    Vector3 vAxis = Vector3.down;
    Vector3 hAxis = Vector3.forward;

    void Update() {
        // sideways device tilting
        hAxis = Quaternion.Euler(0, GetDeviceZTilt(), 0) * hAxis;

        // back and forth device tilting
        Vector3 minAngleVector = hAxis;
        minAngleVector.y = -Mathf.Tan(Mathf.Deg2Rad * xTiltMinAngle);
        minAngleVector.Normalize();

        Quaternion minRot = Quaternion.LookRotation(minAngleVector);
        Quaternion maxRot = Quaternion.LookRotation(vAxis, hAxis);

        float value = GetDeviceXTilt();
        transform.rotation = Quaternion.Slerp(minRot, maxRot, value);
        transform.position = transform.rotation * Vector3.back * distanceFromFocusPoint;
        transform.position += focusPoint;
    }

    float oldXTilt = 0;
    float xTiltSmoothVelocity = 0;
    float GetDeviceXTilt() {
        if (Input.acceleration.sqrMagnitude == 0) {
            return xTiltDefaultValue;
        } else {
            float newXTilt = 1 - Mathf.Abs(Mathf.Atan(Input.acceleration.y / Input.acceleration.z)) / (Mathf.PI / 2);
            oldXTilt = Mathf.SmoothDamp(oldXTilt, newXTilt, ref xTiltSmoothVelocity, xTiltSmoothTime);
            return oldXTilt;
        }
    }

    float oldZTilt = 0;
    float zTiltSmoothVelocity = 0;
    float GetDeviceZTilt() {
        float newZTilt = Mathf.Abs(-Input.acceleration.x) < zTiltMinimumMagnitude
            ? 0
            : Input.acceleration.x;
        oldZTilt = Mathf.SmoothDamp(oldZTilt, newZTilt, ref zTiltSmoothVelocity, zTiltSmoothTime);
        return oldZTilt;
    }

}
