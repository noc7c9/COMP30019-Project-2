﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour {

    public BuildPointsManager buildPoints;
    public UnitPointsManager unitPoints;

    public int initialBuildPoints = 0;
    public int initialUnusedUnitPoints = 2;
    public int initialMaxUnitPoints = 5;

    public Text buildPointsDisplay;
    public Text unitPointsDisplay;

    void Awake() {
        buildPoints = new BuildPointsManager(initialBuildPoints);
        unitPoints = new UnitPointsManager(initialUnusedUnitPoints, initialMaxUnitPoints);
    }

    void Update() {
        buildPointsDisplay.text = buildPoints.GetPoints().ToString();
        unitPointsDisplay.text = unitPoints.GetUnusedPoints() + "/" + unitPoints.GetMaxPoints();
    }

}