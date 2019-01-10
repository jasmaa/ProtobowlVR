using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Commands for profile option
/// </summary>
public class ProfileOptions : MonoBehaviour {

	private AudioSource audioSrc;

	void Start(){
		audioSrc = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Reset user score
	/// </summary>
	public void ResetScore(){
		audioSrc.Play ();
		GameManager.instance.client.pb.ResetScore ();
	}

	/// <summary>
	/// Reset room layout to original state
	/// </summary>
	public void ResetRoom(){
		audioSrc.Play ();
		//TODO: Reset room layout here
	}
}
