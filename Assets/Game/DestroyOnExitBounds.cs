using UnityEngine;
using System.Collections;

public class DestroyOnExitBounds : MonoBehaviour {

    public float sqrMaxDist = 1000;

	void Update() {
	    if (transform.position.sqrMagnitude > sqrMaxDist) {
            Destroy(gameObject);
        }
	}

}