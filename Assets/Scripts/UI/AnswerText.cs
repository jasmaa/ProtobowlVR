using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays answer text
/// <summary>
public class AnswerText : MonoBehaviour {
	
	void Update(){
		if (GameManager.instance.client.pb.state == Protobowl.GameState.IDLE) {
			GetComponent<Text>().text = GameManager.instance.client.pb.GetAnswer ();
		} else {
			GetComponent<Text>().text = "";
		}
	}
}
