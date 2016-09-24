using UnityEngine;
using System.Collections.Generic;

public class CoreSpawnManager : MonoBehaviour {

    static CoreSpawnManager instance;

    GameObject corePrefab;

    int territoryMask;
    int noBuildMask;
    float placementCheckRadius = 0.5f;

    void Awake() {
        instance = this;

        corePrefab = (GameObject) Resources.Load("Core");
    }

    void Start() {
        territoryMask = LayerMask.GetMask("Territory");
        noBuildMask = LayerMask.GetMask("No Build");
    }

	void Update() {
        foreach (RaycastHit hit in InputManager.GetTapsOnMap()) {
            if (BuildPointsManager.Decrement()) {
                SpawnCore(hit.point);
            }
        }
	}

    bool SpawnCore(Vector3 position) {
        // position must be in territory and outside no build zones
        if (!InTerritory(position) || InNoBuildZone(position)) {
            return false;
        }

        GameObject newCore = Instantiate<GameObject>(corePrefab);
        newCore.transform.position = position;

        newCore.GetComponent<Alignment>().IsPlayerOwned(true);

        return true;
    }

    bool InTerritory(Vector3 position, bool IsPlayerOwned=true) {
        foreach (Collider c in Physics.OverlapSphere(position, placementCheckRadius, territoryMask)) {
            if (c.transform.parent.GetComponent<Alignment>().IsPlayerOwned()) {
                return true;
            }
        }
        return false;
    }

    bool InNoBuildZone(Vector3 position) {
        return Physics.CheckSphere(position, placementCheckRadius, noBuildMask);
    }

    public static CoreController[] GetAllCores() {
        return GameObject.FindObjectsOfType<CoreController>();
    }

}