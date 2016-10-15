using UnityEngine;
using System.Collections.Generic;

public class CoreSpawnManager : MonoBehaviour {

    GameObject corePrefab;

    int inTerritoryCheckLayerMask;
    int unoccupiedCheckLayerMask;
    float placementCheckRadius = 0.1f;
    float coreRadius;

    void Awake() {
        corePrefab = (GameObject) Resources.Load("Core");
        coreRadius = corePrefab.GetComponent<CapsuleCollider>().radius;
    }

    void Start() {
        inTerritoryCheckLayerMask = LayerMask.GetMask("Territory");
        unoccupiedCheckLayerMask = ~LayerMask.GetMask("Territory", "Floor");
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
        newCore.transform.position = new Vector3(
            position.x,
            position.y + newCore.transform.position.y, // floor y + y offset
            position.z
        );

        newCore.GetComponent<Alignment>().IsPlayerOwned(true);

        return true;
    }

    bool ValidBuildLocation(Vector3 position) {
        return InTerritory(position)
            && !Physics.CheckSphere(position, coreRadius, unoccupiedCheckLayerMask);
    }

    bool InTerritory(Vector3 position, bool IsPlayerOwned=true) {
        foreach (Collider c in Physics.OverlapSphere(position, placementCheckRadius, inTerritoryCheckLayerMask)) {
            if (c.transform.parent.GetComponent<Alignment>().IsPlayerOwned()) {
                return true;
            }
        }
        return false;
    }

    public static CoreController[] GetAllCores() {
        return GameObject.FindObjectsOfType<CoreController>();
    }

}