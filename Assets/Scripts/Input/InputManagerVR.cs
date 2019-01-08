using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages input method in VR
/// </summary>
public class InputManagerVR : AbstractInputManager {
		
	public enum InputMode {
		KEYBOARD,
		SPEECH
	}
	public InputMode mode;

	public Keyboard keyboardVR;
	public VoiceInput voiceInput;

	public GameObject buzzer;
	public GameObject optionMenu;

	public Transform rHand;
	private Vector3 rHandOldPos;
	private Vector3 rHandDiff;

	// COOLDOWNS and LOCKOUTS
	private float optionMenuCooldown;

	void Start(){
		TurnOff ();
		rHandOldPos = rHand.position;
	}

	void Update(){
		// === Input Detection ===

		// Buzz
		var grabbedBy = buzzer.GetComponent<OVRGrabbable> ().grabbedBy;
		if (grabbedBy != null) {
			if("RightHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.One)){
				GameManager.instance.PlayerBuzz ();
			}
			else if("LeftHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.Three)){
				GameManager.instance.PlayerBuzz ();
			}
		}
		// Next
		if(OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four)){
			GameManager.instance.client.pb.Next ();
		}
		// Menu
		if(OVRInput.Get (OVRInput.RawAxis1D.RIndexTrigger) == 0 && OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9f && rHand.GetComponent<OVRGrabber>().grabbedObject == null) {
			// detect gesture
			if(rHandDiff.y * 100 < -2 && optionMenuCooldown == 0){
				optionMenuCooldown = 0.5f;
				optionMenu.GetComponent<OptionMenu> ().Open(rHand.transform);
			}
			else if(rHandDiff.y * 100 > 2 && optionMenuCooldown == 0){
				optionMenuCooldown = 0.5f;
				optionMenu.GetComponent<OptionMenu> ().Close();
			}
		}

		// Update diff
		rHandDiff = rHand.position - rHandOldPos;
		rHandOldPos = rHand.position;

		// Update cooldowns
		optionMenuCooldown = Mathf.Clamp(optionMenuCooldown - Time.deltaTime, 0, 9999);
	}

	/// <summary>
	/// Open for input
	/// </summary>
	public override void TurnOn(){
		if (mode == InputMode.KEYBOARD) {
			keyboardVR.gameObject.SetActive (true);
		}
		else if (mode == InputMode.SPEECH){
			voiceInput.TurnOn ();
		}

		buzzer.GetComponent<Buzzer>().SetLight (true);
	}

	/// <summary>
	/// Close out any further input
	/// </summary>
	public override void TurnOff(){
		keyboardVR.ClearText ();
		keyboardVR.gameObject.SetActive (false);
		voiceInput.TurnOff ();

		buzzer.GetComponent<Buzzer>().SetLight (false);
	}
}
