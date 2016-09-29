using UnityEngine;
using System.Collections;

public class HealthPoints : MonoBehaviour {

    public int healthPoints = 10;
    public int maxHealthPoints = 10;
    GameObject unitPrefab;
    private bool displayHP = true;
    private ParticleSystem ps;

    void Awake() {
        unitPrefab = (GameObject)Resources.Load("Unit");
    }
    
    void Update() {
        if (healthPoints <= 0) {
            //Debug.Log("Destroy");
            if (!GetComponent<Alignment>().IsPlayerOwned()) {
                BuildPointsManager.Increment();
            }
            displayHP = false;
            ps = GetComponentInChildren<ParticleSystem>();
            ps.Emit(500);
            Destroy(gameObject, ps.duration);
        }
    }

    void OnGUI() {
        if (displayHP) {
            Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
            center.y = Screen.height - center.y;
            float width = 40 * healthPoints / (float)maxHealthPoints;
            GUI.Label(new Rect(center.x - 20, center.y - 40, width, 20), "", "box");
        }
        
    }
}