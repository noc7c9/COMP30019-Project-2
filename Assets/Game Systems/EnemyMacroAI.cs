using UnityEngine;
using System.Collections.Generic;

public class EnemyMacroAI : MonoBehaviour {

    public int maxAttempts = 100;
    public float maxOffset = 5;
    public float coreRange = 15;

    public BuildPointsManager buildPoints;
    public UnitPointsManager unitPoints;

    public int initialBuildPoints = 0;
    public int initialMaxUnitPoints = 10;

    CoreBuildManager coreBuildManager;

    List<CoreController> playerCores = new List<CoreController>();
    List<CoreController> enemyCores = new List<CoreController>();

    void Start() {
        coreBuildManager = FindObjectOfType<CoreBuildManager>();
        buildPoints = new BuildPointsManager(initialBuildPoints);
        unitPoints = new UnitPointsManager(initialMaxUnitPoints, initialMaxUnitPoints);
    }

    void Update() {
        ProcessCores();

        // if the ai has build points, build a core
        int maxAttempts = this.maxAttempts;
        while (maxAttempts-- > 0 && buildPoints.CanDecrement()) { // loop until no more can be built
            // get a position between an enemy core and a player core
            Vector3 playerCore = GetRandomPlayerCore().transform.position;
            Vector3 enemyCore = GetRandomEnemyCore().transform.position;
            Vector3 position = enemyCore + (playerCore - enemyCore).normalized * coreRange;
            position.y = 0;

            // offset by a random position
            position += maxOffset * new Vector3(
                Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)
            );

            // check if the position is valid
            //ScreenLogger.Log(playerCore, enemyCore, position, coreBuildManager.ValidBuildLocation(position, Alignment.ENEMY));
            if (coreBuildManager.ValidBuildLocation(position, Alignment.ENEMY)) {
                coreBuildManager.SpawnCore(position, Alignment.ENEMY);
                buildPoints.Decrement();
            }
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