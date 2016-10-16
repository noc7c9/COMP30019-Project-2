using UnityEngine;
using System.Collections;

public class HealthPoints : MonoBehaviour {

    BuildPointsManager buildPointsManager;

    public int healthPoints = 10;
    public int maxHealthPoints = 10;

    bool displayHP = true;
    ParticleSystem ps;

    void Awake() {
        // on destruction we will increment the opposing sides build points
        if (GetComponent<Alignment>().IsPlayerOwned()) {

        } else {
            buildPointsManager = FindObjectOfType<PlayerResources>().buildPoints;
        }
    }

    void Update() {
        if (enabled && healthPoints <= 0) {
            enabled = false;
            if (buildPointsManager != null) {
                buildPointsManager.Increment();
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