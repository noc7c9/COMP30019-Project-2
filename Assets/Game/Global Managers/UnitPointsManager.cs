using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitPointsManager : MonoBehaviour {

    public int unusedUnitPoints = 0;
    public int maxUnitPoints = 3;

    public Text display;

    static UnitPointsManager instance;

    void Awake() {
        instance = this;
    }

    void Update() {
        display.text = unusedUnitPoints + "/" + maxUnitPoints;
    }

    public static bool CanIncrement(int value=1) {
        return instance.unusedUnitPoints <= instance.maxUnitPoints - value;
    }

    public static bool CanDecrement(int value=1) {
        return instance.unusedUnitPoints >= value;
    }

    public static void Increment(int value=1) {
        if (CanIncrement(value)) {
            instance.unusedUnitPoints = instance.unusedUnitPoints + value;
        }
    }

    public static void Decrement(int value=1) {
        if (CanDecrement(value)) {
            instance.unusedUnitPoints -= value;
        }
    }

}