﻿using UnityEngine;
using System.Collections.Generic;

public class CoreBuildManager : MonoBehaviour {

    BuildPointsManager buildPointsManager;
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

        buildPointsManager = FindObjectOfType<PlayerResources>().buildPoints;
    }

    void Update() {
        foreach (RaycastHit hit in InputManager.GetTapsOnMap()) {
            if (buildPointsManager.CanDecrement() && ValidBuildLocation(hit.point, Alignment.PLAYER)) {
                buildPointsManager.Decrement();
                SpawnCore(hit.point, Alignment.PLAYER);
            }
        }
    }

    public bool SpawnCore(Vector3 position, Alignment.Value alignment) {
        if (!ValidBuildLocation(position, alignment)) {
            return false;
        }

        GameObject newCore = Instantiate<GameObject>(corePrefab);
        newCore.transform.position = new Vector3(
            position.x,
            position.y + newCore.transform.position.y, // floor y + y offset
            position.z
        );

        newCore.GetComponent<Alignment>().SetAsAllyTo(alignment);
        newCore.GetComponent<Selectable>().enabled = true;

        return true;
    }

    public bool ValidBuildLocation(Vector3 position, Alignment.Value alignment) {
        return InTerritory(position, alignment)
            && !Physics.CheckSphere(position, coreRadius, unoccupiedCheckLayerMask);
    }

    bool InTerritory(Vector3 position, Alignment.Value alignment) {
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
            && closestCore.GetComponent<Alignment>().IsAllyTo(alignment);
    }

}