using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject coreSelectedMenu;

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
        if (core != null && UnitPointsManager.CanDecrement()) {
            UnitPointsManager.Decrement();
            core.QueueUnit();
        }
    }

    public void DecrementUnitQueue() {
        CoreController core = SelectionManager.GetSelected().GetComponent<CoreController>();
        if (core != null && core.GetQueueSize() > 0) {
            if (UnitPointsManager.CanIncrement()) {
                UnitPointsManager.Increment();
                core.UnqueueUnit();
            }
        }
    }

}