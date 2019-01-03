using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent user data
/// </summary>
public class UserData : MonoBehaviour {

	public static UserData instance;

	public string room = "bot-testing-vr";
	public string cookie = "derp";

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}

		//TODO: retrieve cookie here
	}
}
