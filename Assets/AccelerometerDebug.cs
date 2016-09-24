using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccelerometerDebug : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "" + Input.acceleration.x;
        //text.text = string.Format("x: {0}\ny: {1}\nz: {2}", Input.acceleration.x, Input.acceleration.y, Input.acceleration.z);
        //text.text = string.Format("pinch: {0}", TouchHandler.GetPinch());
	}
}
