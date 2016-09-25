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

    void Update() {
        display.text = buildPoints.ToString();
    }

    public static bool CanIncrement(int value=1) {
        return true;
    }

    public static bool CanDecrement(int value=1) {
        return instance.buildPoints >= value;
    }

    public static void Increment(int value=1) {
        if (CanIncrement(value)) {
            instance.buildPoints += value;
        }
    }

    public static void Decrement(int value=1) {
        if (CanDecrement(value)) {
            instance.buildPoints -= value;
        }
    }

}