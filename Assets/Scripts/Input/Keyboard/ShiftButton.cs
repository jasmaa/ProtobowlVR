using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftButton : MonoBehaviour {
	/// <summary>
	/// VR keyboard shift button
	/// </summary>
	
	public Keyboard keyboard;

	private AudioSource audioSrc;

	void Start () {
		keyboard = transform.parent.GetComponent<Keyboard> ();
		audioSrc = GetComponent<AudioSource> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			audioSrc.Play();
			keyboard.ToggleShift();
		});
	}

}
