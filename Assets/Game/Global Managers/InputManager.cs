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

        WrappedTouch.Update();
    }
	
	void Update() {
        tapsOnMap.Clear();
        tapsOnObjects.Clear();

        HandleTouch(new WrappedTouch(0));
        foreach (Touch touch in Input.touches) {
            HandleTap(new WrappedTouch(touch));
        }

        WrappedTouch.Update();
    }

    void HandleTouch(WrappedTouch touch) {
        if (touch.OnUI()) {
            return;
        }
        if (touch.phase == TouchPhase.Ended) {
            HandleTap(touch);
        }
    }

    void HandleTap(WrappedTouch touch) {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
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

    class WrappedTouch {

        static Vector3 lastMousePosition;

        public bool isMouse;
        public int? id;
        public Vector3 position;
        public Vector3 deltaPosition;
        public TouchPhase phase;

        WrappedTouch() {
        }

        public WrappedTouch(Touch touch) {
            isMouse = false;
            id = touch.fingerId;
            position = touch.position;
            deltaPosition = touch.deltaPosition;
            phase = touch.phase;
        }

        public WrappedTouch(int mouseButton) {
            isMouse = true;
            id = mouseButton;
            position = Input.mousePosition;
            deltaPosition = position - lastMousePosition;
            if (Input.GetMouseButtonUp(mouseButton)) {
                phase = TouchPhase.Ended;
            } else if (!Input.GetMouseButton(mouseButton)) {
                // if the mouse button isn't down at all, consider the touch to be cancelled
                phase = TouchPhase.Canceled;
            } else if (Input.GetMouseButtonDown(mouseButton)) {
                phase = TouchPhase.Began;
            } else if (deltaPosition.sqrMagnitude > 0) {
                phase = TouchPhase.Stationary;
            } else {
                phase = TouchPhase.Moved;
            }
        }

        public bool OnUI() {
            return isMouse
                ? EventSystem.current.IsPointerOverGameObject() 
                : EventSystem.current.IsPointerOverGameObject(id.Value);
        }

        public static void Update() {
            lastMousePosition = Input.mousePosition;
        }

    }

}