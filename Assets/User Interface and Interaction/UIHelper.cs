using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHelper : MonoBehaviour {
    public void GoTo(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ToggleVisibility(GameObject o) {
        o.SetActive(!o.activeSelf);
    }
}