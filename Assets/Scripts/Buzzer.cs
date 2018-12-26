using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour {
	// Controls buzzer

	public Material onMat;
	public Material offMat;

	// Use this for initialization
	void Start () {
		SetLight (false);
	}

	public void SetLight(bool value){
		if (value) {
			transform.Find("Light").transform.GetComponent<Renderer>().material = onMat;
		} else {
			transform.Find("Light").transform.GetComponent<Renderer>().material = offMat;
		}
	}
}
