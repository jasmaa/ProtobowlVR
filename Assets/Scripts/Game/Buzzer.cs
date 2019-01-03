using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls buzzer GameObject
/// </summary>
public class Buzzer : MonoBehaviour {

	public Material onMat;
	public Material offMat;

	// Use this for initialization
	void Start () {
		SetLight (false);
	}

	/// <summary>
	/// Toggle light
	/// </summary>
	public void SetLight(bool value){
		
		if (value) {
			transform.Find("Light").transform.GetComponent<Renderer>().material = onMat;
		} else {
			transform.Find("Light").transform.GetComponent<Renderer>().material = offMat;
		}
	}
}
