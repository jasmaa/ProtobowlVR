using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR keyboard key
/// </summary>
public class KeyboardButton : MonoBehaviour {

	public string mainChar;
	public string shiftChar;
	public string currChar;
	public Keyboard keyboard;

	private AudioSource audioSrc;

	void Start(){
		keyboard = transform.parent.GetComponent<Keyboard> ();
		audioSrc = GetComponent<AudioSource> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			keyboard.text += currChar;
			keyboard.SetShift(false);

			audioSrc.Play();
		});
	}

	/// <summary>
	/// Initialize key
	/// </summary>
	public void Init(){
		currChar = mainChar;
		transform.GetChild(0).GetComponent<Text> ().text = currChar;
	}

	/// <summary>
	/// Set shift mode
	/// </summary>
	public void SetShift(bool shift){
		currChar = shift ? shiftChar : mainChar;
		transform.GetChild(0).GetComponent<Text> ().text = currChar;
	}
}
