using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildPointsManager : MonoBehaviour {

    public int buildPoints = 0;

    public Text display;

    static BuildPointsManager instance;

    void Awake() {
        instance = this;
    }

    float timer = 0;
    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 10;
            //buildPoints += 10;
        }

        display.text = buildPoints.ToString();
    }

    public static bool Increment(int value=1) {
        instance.buildPoints += value;
        return true;
    }

    public static bool Decrement(int value=1) {
        if (instance.buildPoints >= value) {
            instance.buildPoints -= value;
            return true;
        }
        return false;
    }

}