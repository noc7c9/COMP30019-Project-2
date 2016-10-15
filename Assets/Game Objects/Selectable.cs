using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

    bool selected;

    public bool IsSelected(bool? selected = null) {
        if (selected.HasValue) {
            this.selected = selected.Value;
        }
        return enabled && this.selected;
    }

    void OnGUI() {
        if (IsSelected()) {
            Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
            center.y = Screen.height - center.y;
            GUI.Label(new Rect(center.x - 10, center.y + 10, 20, 20), "S", "box");
        }
    }

}