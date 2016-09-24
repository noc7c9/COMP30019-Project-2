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

    public static bool Increment(int value=1) {
        if (instance.unusedUnitPoints <= instance.maxUnitPoints - value) {
            instance.unusedUnitPoints = instance.unusedUnitPoints + value;
            return true;
        }
        return false;
    }

    public static bool Decrement(int value=1) {
        if (instance.unusedUnitPoints >= value) {
            instance.unusedUnitPoints -= value;
            return true;
        }
        return false;
    }

}