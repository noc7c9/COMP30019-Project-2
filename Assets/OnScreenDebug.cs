using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OnScreenDebug : MonoBehaviour {

    public int maxMessages = 20;

    static OnScreenDebug instance;
    static Text text;
    static Queue<string> log = new Queue<string>();
	
    void Start() {
        instance = this;
        text = GetComponent<Text>();
    }

	void Update() {
        text.text = string.Join("\n", log.ToArray());
	}

    public static void Log(params string[] msg) {
        log.Enqueue(string.Join(" ", msg));
        while (log.Count > instance.maxMessages) {
            log.Dequeue();
        }
    }

}