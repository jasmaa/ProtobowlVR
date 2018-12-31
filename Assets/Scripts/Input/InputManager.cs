using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	/// <summary>
	/// Manages input method in VR
	/// </summary>
		
	public enum InputMode {
		KEYBOARD,
		SPEECH
	}
	public InputMode mode;

	public GameObject keyboardVR;
	public VoiceInput voiceInput;

	public void TurnOn(){
		/// <summary>
		/// Open for input
		/// </summary>
		
		if (mode == InputMode.KEYBOARD) {
			keyboardVR.SetActive (true);
		}
		else if (mode == InputMode.SPEECH){
			voiceInput.TurnOn ();
		}

	}

	public void TurnOff(){
		/// <summary>
		/// Close out any further input
		/// </summary>
		
		keyboardVR.GetComponent<Keyboard> ().ClearText (); 
		keyboardVR.SetActive (false);
		voiceInput.TurnOff ();
	}
}
