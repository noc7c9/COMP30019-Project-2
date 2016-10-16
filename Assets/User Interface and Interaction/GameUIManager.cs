using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    UnitPointsManager unitPointsManager;

    public GameObject coreSelectedMenu;

    void Start() {
        unitPointsManager = FindObjectOfType<PlayerResources>().unitPoints;
    }

    void Update() {
        GameObject selection = SelectionManager.GetSelected();
        if (selection != null && selection.GetComponent<CoreController>() != null) {
            coreSelectedMenu.SetActive(true);
        } else {
            coreSelectedMenu.SetActive(false);
        }
    }

    public void IncrementUnitQueue() {
        CoreController core = SelectionManager.GetSelected().GetComponent<CoreController>();
        if (core != null && unitPointsManager.CanDecrement()) {
            unitPointsManager.Decrement();
            core.QueueUnit();
        }
    }

    public void DecrementUnitQueue() {
        CoreController core = SelectionManager.GetSelected().GetComponent<CoreController>();
        if (core != null && core.GetQueueSize() > 0) {
            if (unitPointsManager.CanIncrement()) {
                unitPointsManager.Increment();
                core.UnqueueUnit();
            }
        }
    }

}