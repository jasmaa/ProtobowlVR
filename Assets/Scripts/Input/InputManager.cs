using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages input method in VR
/// </summary>
public class InputManager : MonoBehaviour {
		
	public enum InputMode {
		KEYBOARD,
		SPEECH
	}
	public InputMode mode;

	public GameObject keyboardVR;
	public VoiceInput voiceInput;

	void Start(){
		TurnOff ();
	}

	/// <summary>
	/// Open for input
	/// </summary>
	public void TurnOn(){
		if (mode == InputMode.KEYBOARD) {
			keyboardVR.SetActive (true);
		}
		else if (mode == InputMode.SPEECH){
			voiceInput.TurnOn ();
		}
	}

	/// <summary>
	/// Close out any further input
	/// </summary>
	public void TurnOff(){
		keyboardVR.GetComponent<Keyboard> ().ClearText ();
		keyboardVR.SetActive (false);
		voiceInput.TurnOff ();
	}
}
