using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR keyboard backspace
/// </summary>
public class BackspaceButton : MonoBehaviour {
	
	public Keyboard keyboard;

	private AudioSource audioSrc;

	void Start () {
		keyboard = transform.parent.GetComponent<Keyboard> ();
		audioSrc = GetComponent<AudioSource> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			audioSrc.Play();
			keyboard.Backspace();
		});
	}

}
