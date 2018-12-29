using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum InputMode {
		KEYBOARD,
		SPEECH
	}
	public InputMode mode;

	public GameObject keyboardVR;
	public VoiceInput voiceInput;

	public void TurnOn(){
		if (mode == InputMode.KEYBOARD) {
			keyboardVR.SetActive (true);
		}
		else if (mode == InputMode.SPEECH){
			voiceInput.TurnOn ();
		}

	}

	public void TurnOff(){
		keyboardVR.GetComponent<Keyboard> ().ClearText (); 
		keyboardVR.SetActive (false);
		voiceInput.TurnOff ();
	}
}
