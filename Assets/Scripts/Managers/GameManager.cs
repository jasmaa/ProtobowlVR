using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Manages front end game

	public static GameManager instance;
	public PBClient client;

	// replace me with list later!!!!
	public GameObject buzzer;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//print (client.pb.state);

		// oculus input detection
		if(buzzer.GetComponent<OVRGrabbable>().isGrabbed && OVRInput.Get(OVRInput.Button.One)){
			client.pb.Buzz ();
			buzzer.GetComponent<Buzzer>().SetLight (true);
		}
		if(OVRInput.Get(OVRInput.Button.Two)){
			client.pb.Next ();
		}

	}
}
