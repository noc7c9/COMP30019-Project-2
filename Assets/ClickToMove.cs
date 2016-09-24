using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

	void Update () {
	    if (Input.GetMouseButtonDown(0)) {
            Vector3 position = ScreenRayCast(Input.mousePosition);
            MoveToPosition(position);
        }
	}

    Vector3 ScreenRayCast(Vector2 screenPosition) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit.point;
    }

    void MoveToPosition(Vector3 position) {
        transform.position = position;
    }

}
