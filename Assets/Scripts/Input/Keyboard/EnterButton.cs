using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR keyboard enter button
/// </summary>
public class EnterButton : MonoBehaviour {

	public Keyboard keyboard;

	private AudioSource audioSrc;

	void Start () {
		keyboard = transform.parent.GetComponent<Keyboard> ();
		audioSrc = GetComponent<AudioSource> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			audioSrc.Play();
			keyboard.Submit();
		});
	}

}
