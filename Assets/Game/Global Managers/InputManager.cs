using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public float maxTapDuration = 0.25f;

    static InputManager instance;

    public RectTransform buttonBar;

    List<Vector3> drags;
    List<RaycastHit> tapsOnMap;
    List<RaycastHit> tapsOnObjects;
    LayerMask ignoreTriggers;

    Dictionary<int, float> touchTotalOffset;

    void Awake() {
        instance = this;
    }

    void Start() {
        drags = new List<Vector3>();
        tapsOnMap = new List<RaycastHit>();
        tapsOnObjects = new List<RaycastHit>();
        ignoreTriggers = ~LayerMask.GetMask("Territory", "No Build");

        touchTotalOffset = new Dictionary<int, float>();

        WrappedTouch.Update();
    }
	
	void Update() {
        drags.Clear();
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

        // update held time for the touch
        if (touch.phase != TouchPhase.Canceled) {
            if (touchTotalOffset.ContainsKey(touch.id)) {
                touchTotalOffset[touch.id] += touch.deltaTime;
            } else {
                touchTotalOffset[touch.id] = touch.deltaTime;
            }
        }

        float totalHoldTime = touchTotalOffset.ContainsKey(touch.id)
            ? touchTotalOffset[touch.id]
            : 0;

        if (touch.phase == TouchPhase.Moved) {
            // considered a drag if the touch has held for a time
            if (totalHoldTime > maxTapDuration) {
                drags.Add(-touch.deltaPosition);
            }
        } else if (touch.phase == TouchPhase.Ended) {
            // not considered a tap if the touch has been held for too long
            if (totalHoldTime < maxTapDuration) {
                HandleTap(touch);
            }

            // finished touch, so remove
            touchTotalOffset.Remove(touch.id);
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

    public static List<Vector3> GetDrags() {
        return instance.drags;
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
        public int id;
        public Vector3 position;
        public Vector3 deltaPosition;
        public float deltaTime;
        public TouchPhase phase;

        public WrappedTouch(Touch touch) {
            isMouse = false;
            id = touch.fingerId;
            position = touch.position;
            deltaPosition = touch.deltaPosition;
            deltaTime = touch.deltaTime;
            phase = touch.phase;
        }

        public WrappedTouch(int mouseButton) {
            isMouse = true;
            id = -(mouseButton + 100000); // hopefully won't class with a fingerid 
            position = Input.mousePosition;
            deltaPosition = position - lastMousePosition;
            deltaTime = Time.deltaTime;
            if (Input.GetMouseButtonUp(mouseButton)) {
                phase = TouchPhase.Ended;
            } else if (!Input.GetMouseButton(mouseButton)) {
                // if the mouse button isn't down at all, consider the touch to be cancelled
                phase = TouchPhase.Canceled;
            } else if (Input.GetMouseButtonDown(mouseButton)) {
                phase = TouchPhase.Began;
            } else if (deltaPosition.sqrMagnitude > 0) {
                phase = TouchPhase.Moved;
            } else {
                phase = TouchPhase.Stationary;
            }
        }

        public bool OnUI() {
            return isMouse
                ? EventSystem.current.IsPointerOverGameObject() 
                : EventSystem.current.IsPointerOverGameObject(id);
        }

        public static void Update() {
            lastMousePosition = Input.mousePosition;
        }

    }

}