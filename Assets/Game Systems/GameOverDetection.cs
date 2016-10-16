using UnityEngine;
using System.Collections.Generic;

public class GameOverDetection : MonoBehaviour {

    List<CoreController> playerCores = new List<CoreController>();
    List<CoreController> enemyCores = new List<CoreController>();

    public Canvas victoryScreen;
    public Canvas lossScreen;
    public Canvas gameUI;

    void Start() {
        Time.timeScale = 1;
    }

    void Update() {
        CoreBuildManager.ProcessCores(ref playerCores, ref enemyCores);

        if (playerCores.Count <= 0) {
            PlayerWins();
        } else if (enemyCores.Count <= 0) {
            PlayerLoses();
        }
    }

    void PlayerWins() {
        Time.timeScale = 0;
        victoryScreen.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    void PlayerLoses() {
        Time.timeScale = 0;
        lossScreen.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

}