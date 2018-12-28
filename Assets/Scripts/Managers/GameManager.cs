﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Manages front end game

	public static GameManager instance;
	public PBClient client;

	public AudioSource testSound;

	// replace me with list later!!!!
	public GameObject buzzer;
	public GameObject optionMenu;

	public Transform rHand;
	private Vector3 rHandOldPos;
	private Vector3 rHandDiff;

	// COOLDOWNS
	private float optionMenuCooldown = 0;

	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}

		testSound = GetComponent<AudioSource> ();
		rHandOldPos = rHand.position;
		optionMenu = GameObject.Find ("OptionMenu");
	}
	
	// Update is called once per frame
	void Update () {
		//print (client.pb.state);

		// === Input Detection ===

		// Buzz
		var grabbedBy = buzzer.GetComponent<OVRGrabbable> ().grabbedBy;
		if (grabbedBy != null) {
			if("RightHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.One)){

				client.pb.Buzz ();
				buzzer.GetComponent<Buzzer>().SetLight (true);
			}
			else if("LeftHandAnchor".Equals(grabbedBy.name) && OVRInput.Get(OVRInput.Button.Three)){

				client.pb.Buzz ();
				buzzer.GetComponent<Buzzer>().SetLight (true);
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
	}
}