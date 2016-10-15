using UnityEngine;
using System.Collections.Generic;

public class CoreBuildManager : MonoBehaviour {

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
            if (BuildPointsManager.CanDecrement()) {
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

        newCore.GetComponent<Alignment>().SetAsPlayerOwned();
        newCore.GetComponent<Selectable>().enabled = true;

        return true;
    }

    bool ValidBuildLocation(Vector3 position) {
        return InTerritory(position)
            && !Physics.CheckSphere(position, coreRadius, unoccupiedCheckLayerMask);
    }

    bool InTerritory(Vector3 position) {
        // find the closest core
        Transform closestCore = null;
        float minDist = Mathf.Infinity;
        foreach (Collider c in Physics.OverlapSphere(position, placementCheckRadius, inTerritoryCheckLayerMask)) {
            Transform core = c.transform.parent;
            float dist = (core.position - position).sqrMagnitude;
            if (dist < minDist || closestCore == null) {
                minDist = dist;
                closestCore = core;
            }
        }

        // position is valid if in territory and the closest core is an ally
        return closestCore != null
            && closestCore.GetComponent<Alignment>().IsPlayerOwned();
    }

    public static CoreController[] GetAllCores() {
        return GameObject.FindObjectsOfType<CoreController>();
    }

}