using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls player buzzer
/// </summary>
public class Buzzer : MonoBehaviour {

	public Material onMat;
	public Material offMat;

	private AudioSource audioSrc;

	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		SetLight (false);
	}

	/// <summary>
	/// Toggle light
	/// </summary>
	public void SetLight(bool value){
		
		if (value) {
			transform.Find("Light").transform.GetComponent<Renderer>().material = onMat;
			audioSrc.Play ();
		} else {
			transform.Find("Light").transform.GetComponent<Renderer>().material = offMat;
		}
	}
}
