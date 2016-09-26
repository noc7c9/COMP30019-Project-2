using UnityEngine;
using System.Collections.Generic;

public class CoreSpawnManager : MonoBehaviour {

    static CoreSpawnManager instance;
    public readonly float Y_POS = 1;

    GameObject corePrefab;

    int territoryMask;
    int noBuildMask;
    float placementCheckRadius = 0.5f;

    void Awake() {
        instance = this;

        corePrefab = (GameObject) Resources.Load("Core");
        corePrefab.tag = "Core";
    }

    void Start() {
        territoryMask = LayerMask.GetMask("Territory");
        noBuildMask = LayerMask.GetMask("No Build");
    }

	void Update() {
        foreach (RaycastHit hit in InputManager.GetTapsOnMap()) {
            if (ValidBuildLocation(hit.point) && BuildPointsManager.CanDecrement()) {
                BuildPointsManager.Decrement();
                SpawnCore(hit.point);
            }
        }
	}

    bool SpawnCore(Vector3 position) {
        if (!ValidBuildLocation(position)) {
            return false;
        }

        GameObject newCore = Instantiate<GameObject>(corePrefab);
        newCore.transform.position = new Vector3(position.x,Y_POS,position.z);

        newCore.GetComponent<Alignment>().IsPlayerOwned(true);

        return true;
    }

    bool ValidBuildLocation(Vector3 position) {
        return InTerritory(position) && !InNoBuildZone(position);
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