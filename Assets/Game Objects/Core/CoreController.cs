using UnityEngine;
using System.Collections;

public class CoreController : MonoBehaviour {

    public GameObject unitPrefab;

    UnitPointsManager unitPointsManager;
    Alignment alignment;

    [SerializeField]
    int queueCount = 0;

    void Awake() {
        alignment = GetComponent<Alignment>();

        if (alignment.IsAllyTo(Alignment.PLAYER)) {
            unitPointsManager = FindObjectOfType<PlayerResources>().unitPoints;
        } else {

        }
    }

    void OnGUI() {
        if (alignment.IsAllyTo(Alignment.ENEMY)) {
            return;
        }
        Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
        center.y = Screen.height - center.y;
        GUI.Label(new Rect(center.x - 10, center.y - 10, 20, 20), queueCount.ToString(), "box");
    }
    
    void OnDestroy() {
        if (unitPointsManager != null) {
            unitPointsManager.Increment(queueCount);
        }
    }

    public void GenerateUnits() {
        for (int i = 0; i < queueCount; i++) {
            GameObject newUnit = Instantiate<GameObject>(unitPrefab);
            newUnit.transform.position = transform.position;
            newUnit.GetComponent<Alignment>().SetAsAllyTo(alignment);
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