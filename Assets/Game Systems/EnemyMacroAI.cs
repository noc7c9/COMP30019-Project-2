using UnityEngine;
using System.Collections.Generic;

public class EnemyMacroAI : MonoBehaviour {

    public BuildPointsManager buildPoints;
    public UnitPointsManager unitPoints;

    public int initialBuildPoints = 0;
    public int initialMaxUnitPoints = 10;

    List<CoreController> playerCores = new List<CoreController>();
    List<CoreController> enemyCores = new List<CoreController>();

    void Start() {
        buildPoints = new BuildPointsManager(initialBuildPoints);
        unitPoints = new UnitPointsManager(initialMaxUnitPoints, initialMaxUnitPoints);
    }

    void Update() {
        ProcessCores();

        // if the ai has build points, build a core
        if (buildPoints.CanDecrement()) {

        }

        // if there are unqueued unit points, queue them
        while (unitPoints.CanDecrement()) {
            unitPoints.Decrement();
            GetRandomEnemyCore().QueueUnit();
        }
    }

    CoreController GetRandomPlayerCore() {
        return playerCores[Random.Range(0, playerCores.Count)];
    }

    CoreController GetRandomEnemyCore() {
        return enemyCores[Random.Range(0, enemyCores.Count)];
    }

    void ProcessCores() {
        CoreController[] allCores = FindObjectsOfType<CoreController>();
        playerCores.Clear();
        enemyCores.Clear();
        foreach (CoreController core in allCores) {
            if (core.gameObject.GetComponent<Alignment>().IsAllyTo(Alignment.PLAYER)) {
                playerCores.Add(core);
            } else {
                enemyCores.Add(core);
            }
        }
    }

}