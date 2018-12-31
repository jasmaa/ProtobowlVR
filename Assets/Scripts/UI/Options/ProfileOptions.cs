using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileOptions : MonoBehaviour {
	/// <summary>
	/// Commands for profile option
	/// </summary>

	public void ResetScore(){
		/// <summary>
		/// Reset user score
		/// </summary>
		
		GameManager.instance.client.pb.ResetScore ();
	}

	public void ResetRoom(){
		/// <summary>
		/// Reset room layout to original state
		/// </summary>
		
		print ("reset room layout here");
	}
}
