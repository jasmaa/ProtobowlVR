using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Commands for profile option
/// </summary>
public class ProfileOptions : MonoBehaviour {

	/// <summary>
	/// Reset user score
	/// </summary>
	public void ResetScore(){
		GameManager.instance.client.pb.ResetScore ();
	}

	/// <summary>
	/// Reset room layout to original state
	/// </summary>
	public void ResetRoom(){
		//TODO: Reset room layout here
	}
}
