using UnityEngine;
using System.Collections;

public class HealthPoints : MonoBehaviour {

    public Texture2D texture;
    public float healthBarHeight = 3;
    public float healthBarWidth = 30;
    public float healthBarYOffset = -5;
    public float healthBarXOffset = 0;

    public int buildPointWorth = 1;

    public int healthPoints = 10;
    public int maxHealthPoints = 10;

    BuildPointsManager buildPointsManager;

    bool displayHP = true;
    ParticleSystem ps;

	GUIStyle style_;
	GUIStyle style {
		get {
			if (style_ == null) {
				style_ = new GUIStyle(GUI.skin.GetStyle("label"));
				style_.normal.background = texture;
			}
			return style_;
		}
	}

    void Start() {
        // on destruction we will increment the opposing sides build points
        if (GetComponent<Alignment>().IsAllyTo(Alignment.PLAYER)) {
            buildPointsManager = FindObjectOfType<EnemyMacroAI>().buildPoints;
        } else {
            buildPointsManager = FindObjectOfType<PlayerResources>().buildPoints;
        }
    }

    void Update() {
        if (enabled && healthPoints <= 0) {
            enabled = false;
            if (buildPointsManager != null) {
                buildPointsManager.Increment(buildPointWorth);
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
            float width = healthBarWidth * healthPoints / (float)maxHealthPoints;
            Rect rect = new Rect(
                center.x - healthBarWidth / 2 + healthBarXOffset,
                center.y + healthBarYOffset,
                width, healthBarHeight);
            GUI.Label(rect, "", style);
        }
        
    }
}