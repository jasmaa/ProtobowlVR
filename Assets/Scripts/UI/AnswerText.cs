using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerText : MonoBehaviour {
	/// <summary>
	/// Displays answer text
	/// <summary>
	
	void Update(){
		if (GameManager.instance.client.pb.state == Protobowl.GameState.IDLE) {
			GetComponent<Text>().text = GameManager.instance.client.pb.GetAnswer ();
		} else {
			GetComponent<Text>().text = "";
		}
	}
}
