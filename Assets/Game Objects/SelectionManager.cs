using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour {

    static SelectionManager instance;

    Selectable selected;
	
    void Awake() {
        instance = this;
    }

	void Update() {
        Selectable newSelected = selected;

        // deselect on tap
        if (InputManager.GetTapsOnMap().Count > 0 || InputManager.GetTapsOnObjects().Count > 0) {
            newSelected = null;
        }

        // select selectables on tap
        foreach (RaycastHit hit in InputManager.GetTapsOnObjects()) {
            Selectable selectable = hit.collider.GetComponent<Selectable>();
            if (selectable != null && selectable.enabled) {
                newSelected = selectable;
            }
        }

        // change selection if necessary
        if (newSelected != selected) {
            if (selected) {
                selected.IsSelected(false);
            }
            if (newSelected) {
                newSelected.IsSelected(true);
            }
            selected = newSelected;
        }
    }

    public static GameObject GetSelected() {
        if (instance.selected) {
            return instance.selected.gameObject;
        }
        return null;
    }

}