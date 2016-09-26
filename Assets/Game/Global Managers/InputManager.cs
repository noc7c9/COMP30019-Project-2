using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public float maxTapDuration = 0.15f;
    public float maxTapOffset = 15;

    static InputManager instance;

    List<Vector3> drags;
    List<RaycastHit> tapsOnMap;
    List<RaycastHit> tapsOnObjects;
    LayerMask ignoreTriggers;

    Dictionary<int, float> touchTotalHoldTime;
    Dictionary<int, Vector3> touchTotalOffset;

    void Awake() {
        instance = this;
    }

    void Start() {
        drags = new List<Vector3>();
        tapsOnMap = new List<RaycastHit>();
        tapsOnObjects = new List<RaycastHit>();
        ignoreTriggers = ~LayerMask.GetMask("Territory", "No Build");

        touchTotalHoldTime = new Dictionary<int, float>();
        touchTotalOffset = new Dictionary<int, Vector3>();

        WrappedTouch.Update();
    }
	
	void Update() {
        drags.Clear();
        tapsOnMap.Clear();
        tapsOnObjects.Clear();

        if (!Input.touchSupported) {
            HandleTouch(new WrappedTouch(0));
        } else {
            foreach (Touch touch in Input.touches) {
                HandleTouch(new WrappedTouch(touch));
            }
        }

        WrappedTouch.Update();
    }

    void HandleTouch(WrappedTouch touch) {
        // ignore ui hits and cancelled touches
        if (touch.OnUI() || touch.phase == TouchPhase.Canceled) {
            if (touch.phase != TouchPhase.Canceled) {
                OnScreenDebug.Log("ON UI");
            }
            return;
        }
        OnScreenDebug.Log("NOT ON UI");

        // update held time for the touch
        if (touchTotalHoldTime.ContainsKey(touch.id)) {
            touchTotalHoldTime[touch.id] += touch.deltaTime;
        } else {
            touchTotalHoldTime[touch.id] = touch.deltaTime;
        }

        // update offset for the touch
        if (touchTotalOffset.ContainsKey(touch.id)) {
            touchTotalOffset[touch.id] += touch.deltaPosition;
        } else {
            touchTotalOffset[touch.id] = touch.deltaPosition;
        }

        float totalHoldTime = touchTotalHoldTime[touch.id];
        Vector3 totalOffset = touchTotalOffset[touch.id];

        //OnScreenDebug.Log(touch.id.ToString(), touch.deltaPosition.ToString(), totalOffset.ToString(), totalOffset.magnitude.ToString(), maxTapOffset.ToString());

        if (touch.phase == TouchPhase.Moved) {
            drags.Add(-touch.deltaPosition);
        } else if (touch.phase == TouchPhase.Ended) {
            // not considered a tap if the touch has been held for too long or moved too far
            //if (totalHoldTime < maxTapDuration && totalOffset.magnitude < maxTapOffset) {
            if (totalHoldTime < maxTapDuration) {
                HandleTap(touch.position);
            }

            // finished touch, so remove
            touchTotalHoldTime.Remove(touch.id);
            touchTotalOffset.Remove(touch.id);
        }
    }

    void HandleTap(Vector3 position) {
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
            // source: http://forum.unity3d.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/#post-1876138 
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(position.x, position.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static void Update() {
            lastMousePosition = Input.mousePosition;
        }

    }

}