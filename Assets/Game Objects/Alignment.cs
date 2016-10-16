using UnityEngine;
using System.Collections;

public class Alignment : MonoBehaviour {

    public enum Value {
        Player,
        Enemy
    };

    public static readonly Value PLAYER = Value.Player;
    public static readonly Value ENEMY = Value.Enemy;

    Color PLAYER_COLOR = new Color(0, 0.25f, 1);
    Color ENEMY_COLOR = new Color(1, 0, 0);

    [SerializeField]
    Value value = Value.Player;

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
        return value == Value.Player ? PLAYER_COLOR : ENEMY_COLOR;
    }

    public void SetAsAllyTo(Value other) {
        value = other;
        RefreshColor();
    }

    public void SetAsEnemyTo(Value other) {
        value = value == Value.Player ? Value.Enemy : Value.Player;
        RefreshColor();
    }

    public bool IsAllyTo(Value other) {
        return value == other;
    }

    public bool IsEnemyTo(Value other) {
        return value != other;
    }

    // Alignment component versions
    public void SetAsAllyTo(Alignment other) {
        if (other != null) {
            SetAsAllyTo(other.value);
        }
    }
    public void SetAsEnemyTo(Alignment other) {
        if (other != null) {
            SetAsEnemyTo(other.value);
        }
    }
    public bool IsAllyTo(Alignment other) {
        if (other != null) {
            return IsAllyTo(other.value);
        }
        return false;
    }
    public bool IsEnemyTo(Alignment other) {
        if (other != null) {
            return IsEnemyTo(other.value);
        }
        return false;
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