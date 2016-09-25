using UnityEngine;
using System.Collections;

public class HealthPoints : MonoBehaviour {

    public int healthPoints = 10;
    public int maxHealthPoints = 10;

    void Update() {
        if (healthPoints <= 0) {
            if (!GetComponent<Alignment>().IsPlayerOwned()) {
                BuildPointsManager.Increment();
            }
            Destroy(gameObject);
        }
    }

    void OnGUI() {
        Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
        center.y = Screen.height - center.y;
        float width = 40 * healthPoints / (float) maxHealthPoints;
        GUI.Label(new Rect(center.x - 20, center.y - 40, width, 20), "", "box");
    }

}