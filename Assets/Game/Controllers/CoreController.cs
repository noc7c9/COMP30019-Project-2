using UnityEngine;
using System.Collections;

public class CoreController : MonoBehaviour {

    GameObject unitPrefab;
    Selectable selectable;
    Alignment alignment;

    public int queueCount = 0;

    void Awake() {
        unitPrefab = (GameObject) Resources.Load("Unit");

        alignment = GetComponent<Alignment>();
        selectable = GetComponent<Selectable>();
    }

    void Start() {
        if (!alignment.IsPlayerOwned()) {
            selectable.enabled = false;
        }
    }

    void OnGUI() {
        if (!alignment.IsPlayerOwned()) {
            return;
        }
        Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
        center.y = Screen.height - center.y;
        GUI.Label(new Rect(center.x - 10, center.y - 10, 20, 20), queueCount.ToString(), "box");
    }
    /*
    void Update() {
        if (h.healthPoints <= 0) {
            ps.Emit(100);
            Destroy(this.gameObject,ps.duration);
        }
    }*/
    
    void OnDestroy() {
        
        UnitPointsManager.Increment(queueCount);
    }

    public void GenerateUnits() {
        bool isPlayerOwned = alignment.IsPlayerOwned();
        for (int i = 0; i < queueCount; i++) {
            GameObject newUnit = Instantiate<GameObject>(unitPrefab);
            newUnit.transform.position = transform.position;
            newUnit.GetComponent<Alignment>().IsPlayerOwned(isPlayerOwned);
        }
    }

    public void QueueUnit() {
        queueCount++;
    }

    public void UnqueueUnit() {
        queueCount--;
    }

    public int GetQueueSize() {
        return queueCount;
    }

}