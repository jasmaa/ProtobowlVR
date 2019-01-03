using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfaces game and Protobowl
/// </summary>
public class PBClient : MonoBehaviour {
	
	public PBStateTracker tracker;
	public Protobowl pb;

	void Start () {

		pb = new Protobowl ();
		StartCoroutine(pb.Connect (UserData.instance.room, UserData.instance.cookie));

		tracker.pb = pb;
	}

	void OnDestroy(){
		pb.Disconnect ();
	}
}
