using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerText : MonoBehaviour {
	// Displays answer text

	void Update(){
		if (GameManager.instance.client.pb.state == Protobowl.GameState.IDLE) {
			GetComponent<Text>().text = GameManager.instance.client.pb.GetAnswer ();
		} else {
			GetComponent<Text>().text = "";
		}
	}
}
