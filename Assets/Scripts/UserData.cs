﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent user data
/// </summary>
public class UserData : MonoBehaviour {

	public static UserData instance;

	public string room = "bot-testing-vr";
	public string cookie = "derp";

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}

		cookie = PlayerPrefs.HasKey ("cookie") ? PlayerPrefs.GetString ("cookie") : Utils.GenerateCookie ();
		PlayerPrefs.SetString ("cookie", cookie);
	}
}
