using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileOptions : MonoBehaviour {
	// Commands for profile option

	public void ResetScore(){
		GameManager.instance.client.pb.ResetScore ();
	}

	public void ResetRoom(){
		print ("reset room layout here");
	}
}
