using UnityEngine;
using System.Collections;

public class Alignment : MonoBehaviour {

    Color PLAYER_COLOR = new Color(0, 0.25f, 1);
    Color ENEMY_COLOR = new Color(1, 0, 0);

    [SerializeField]
    bool playerOwned = false;

    void Start() {
        RefreshColor();
    }

    void RefreshColor() {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer) {
            renderer.material.color = GetColor();
        }
    }

    public Color GetColor() {
        return playerOwned ? PLAYER_COLOR : ENEMY_COLOR;
    }

    // absolute methods
    public void SetAsPlayerOwned() {
        playerOwned = true;
    }
    public bool IsPlayerOwned() {
        return playerOwned;
    }

    // comparative methods

    public void SetAsAllyTo(Alignment other) {
        if (other != null) {
            playerOwned = other.playerOwned;
            RefreshColor();
        }
    }

    public void SetAsEnemyTo(Alignment other) {
        if (other != null) {
            playerOwned = !other.playerOwned;
            RefreshColor();
        }
    }

    public bool IsAllyTo(Alignment other) {
        return other != null ? playerOwned == other.playerOwned : false;
    }

    public bool IsEnemyTo(Alignment other) {
        return other != null ? playerOwned != other.playerOwned : false;
    }

    // GameObject versions
    public void SetAsAllyTo(GameObject other) {
        SetAsAllyTo(other.GetComponent<Alignment>());
    }
    public void SetAsEnemyTo(GameObject other) {
        SetAsEnemyTo(other.GetComponent<Alignment>());
    }
    public bool IsAllyTo(GameObject other) {
        return IsAllyTo(other.GetComponent<Alignment>());
    }
    public bool IsEnemyTo(GameObject other) {
        return IsEnemyTo(other.GetComponent<Alignment>());
    }

}