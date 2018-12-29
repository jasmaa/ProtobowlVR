using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Manages front end game

	public static GameManager instance;
	public PBClient client;

	// replace me with list later!!!!
	public GameObject buzzer;
	public GameObject optionMenu;
	public GameObject countdownBar;

	public InputManager inputManager;

	public Transform rHand;
	private Vector3 rHandOldPos;
	private Vector3 rHandDiff;

	// COOLDOWNS and LOCKOUTS
	private float optionMenuCooldown = 0;
	private float stateUpdateCooldown = 0;
	private bool buzzLockout = false;

	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
			
		rHandOldPos = rHand.position;
	}
	
	// Update is called once per frame
	void Update () {
		//print (client.pb.state);

		// === Update based on state ===
		if (stateUpdateCooldown == 0 && client.pb.state == Protobowl.GameState.RUNNING) {
			buzzer.GetComponent<Buzzer>().SetLight (false);
			inputManager.TurnOff ();
			buzzLockout = false;

			stateUpdateCooldown = 1;
		}

		// === Input Detection ===

		// Buzz
		var grabbedBy = buzzer.GetComponent<OVRGrabbable> ().grabbedBy;
		if (grabbedBy != null) {
			if("RightHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.One)){
				PlayerBuzz ();
			}
			else if("LeftHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.Three)){
				PlayerBuzz ();
			}
		}
		// Next
		if(OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four)){
			client.pb.Next ();
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
		stateUpdateCooldown = Mathf.Clamp(stateUpdateCooldown - Time.deltaTime, 0, 9999);
	}

	void PlayerBuzz(){
		// Handles player buzz
		if (buzzLockout) {
			return;
		}

		client.pb.Buzz ();
		buzzLockout = true;

		Invoke ("DetectBuzz", 0.1f);
	}
	void DetectBuzz(){
		if (client.pb.hasBuzz) {
			buzzer.GetComponent<Buzzer>().SetLight (true);
			inputManager.TurnOn ();
			countdownBar.GetComponent<CountdownBar> ().Reset ();
		}
	}
}
