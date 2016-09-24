using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveManager : MonoBehaviour {

    public Text countdown;

    public float maxWaveTimer = 30;
    public float initialWaveTimer = 2.5f;
    float waveTimer;

    void Start() {
        waveTimer = initialWaveTimer;
	}

    void Update() {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0) {
            waveTimer = maxWaveTimer;
            GenerateWave();
        }

        countdown.text = string.Format("{0}", waveTimer);
	}

    void GenerateWave() {
        foreach (CoreController core in CoreSpawnManager.GetAllCores()) {
            core.GenerateUnits();
        }
    }

}