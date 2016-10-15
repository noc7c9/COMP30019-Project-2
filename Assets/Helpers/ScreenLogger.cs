using UnityEngine;
using System.Collections.Generic;
using System;

public class ScreenLogger : MonoBehaviour {

	public int maxMessages = 20;
	public int fontSize = 20;
	public bool dynamicFontSize = true;
	public Color color = Color.magenta;

	LinkedList<string> messages;

	GUIStyle style_;
	GUIStyle style {
		get {
			if (style_ == null) {
				style_ = new GUIStyle(GUI.skin.GetStyle("label"));
				style_.normal.textColor = Color.magenta;
				style_.wordWrap = true;
				style.padding.top = 0;
				style.padding.bottom = 0;
				style.margin.top = 0;
				style.margin.bottom = 0;

				// find font size that fits the requirements
				if (dynamicFontSize) {
					float targetHeight = Screen.height / (float)maxMessages;
					GUIContent c = new GUIContent("");
					for (style_.fontSize = 1;
							style_.CalcHeight(c, Screen.width) <= targetHeight;
							style_.fontSize++) {}
					fontSize = --style_.fontSize;
					style_.fixedHeight = targetHeight;
							
					// reset if font size gets too small
					if (style_.fontSize < 1) {
						fontSize = 1;
					}
				}
			}
			return style_;
		}
	}

	void Awake() {
		messages = new LinkedList<string>();
	}

	void OnGUI() {
		foreach (string msg in messages) {
			GUILayout.Label(msg, style);
		}
	}

	void AddMsg(string msg) {
		messages.AddFirst(msg);
		while (messages.Count > maxMessages) {
			messages.RemoveLast();
		}
	}

	static ScreenLogger instance;
	static ScreenLogger getInstance() {
		if (instance == null) {
			instance = (ScreenLogger)GameObject.FindObjectOfType(typeof(ScreenLogger));
			if (instance == null) {
				GameObject go = new GameObject("UILogger");
				instance = go.AddComponent<ScreenLogger>();
			}
		}
		return instance;
	}

	private static string[] ConvertToStringArray(object[] objs) {
        string[] result = new string[objs.Length];
        for (int i = 0; i < objs.Length; i++) {
            result[i] = objs[i] == null ? "null" : objs[i].ToString();
        }
        return result;
	}

	private static void LogMsg(string msg) {
		getInstance().AddMsg(msg);
	}

	public static void Log(params object[] objs) {
		LogMsg(string.Join(" ", ConvertToStringArray(objs)));
	}

	public static void Logf(string format, params object[] objs) {
		LogMsg(string.Format(format, ConvertToStringArray(objs)));
	}

}