using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    static InputManager instance;

    public RectTransform buttonBar;

    List<RaycastHit> tapsOnMap;
    List<RaycastHit> tapsOnObjects;
    LayerMask ignoreTriggers;

    void Awake() {
        instance = this;
    }

    void Start() {
        tapsOnMap = new List<RaycastHit>();
        tapsOnObjects = new List<RaycastHit>();
        ignoreTriggers = ~LayerMask.GetMask("Territory", "No Build");
    }
	
	void Update() {
        tapsOnMap.Clear();
        tapsOnObjects.Clear();

        if (Input.GetMouseButtonDown(0)) {
            HandleTap(Input.mousePosition);
        }
        foreach (Touch touch in Input.touches) {
            HandleTap(touch.position);
        }
    }

    void HandleTap(Vector2 position) {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreTriggers.value)) {
            if (hit.collider.CompareTag("Map")) {
                tapsOnMap.Add(hit);
            } else {
                tapsOnObjects.Add(hit);
            }
        }
    }

    public static List<RaycastHit> GetTapsOnObjects() {
        return instance.tapsOnObjects;
    }

    public static List<RaycastHit> GetTapsOnMap() {
        return instance.tapsOnMap;
    }

}