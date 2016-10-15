using UnityEngine;
using System.Collections;

public class Alignment : MonoBehaviour {

    public bool playerOwned = false;

    void Awake() {
        RefreshColor();
    }

    void RefreshColor() {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer) {
            renderer.material.color = playerOwned ? Color.blue : Color.red;
        }
    }

    public bool IsPlayerOwned(bool? playerOwned=null) {
        if (playerOwned.HasValue) {
            this.playerOwned = playerOwned.Value;
            RefreshColor();
        }
        return this.playerOwned;
    }

}