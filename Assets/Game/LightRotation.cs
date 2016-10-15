using UnityEngine;
using System.Collections;

public class LightRotation : MonoBehaviour {

    public float speed = 0.1f;
        
    void Start() {
        transform.Rotate(Vector3.up, Random.Range(0, 360), Space.World);
    }

	void Update() {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
    }

}