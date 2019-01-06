using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using SimpleJSON;

/// <summary>
/// Updates game text fields
/// </summary>
public class GameText : MonoBehaviour {

	public enum TextType {
		ANSWER,
		LOG,
		QUESTION,
		TIME,
		TOPIC
	}
	public TextType textType;

	private Text gameText;

	void Start(){
		gameText = GetComponent<Text> ();
	}

	void Update(){
		if (textType == TextType.ANSWER) {
			if (GameManager.instance.client.pb.state == Protobowl.GameState.IDLE) {
				gameText.text = GameManager.instance.client.pb.GetAnswer ();
			} else {
				gameText.text = "";
			}
		} else if (textType == TextType.QUESTION) {
			gameText.text = GameManager.instance.client.tracker.Disp;
		} else if (textType == TextType.TIME) {
			gameText.text = System.DateTime.Now.ToLongTimeString ();	
		} else if (textType == TextType.TOPIC) {
			gameText.text = GameManager.instance.client.pb.GetQuestionCategory ();
		} else if (textType == TextType.LOG) {
			// construct log text
			List<string> logList = new List<string>();
			foreach (JSONNode entry in GameManager.instance.client.pb.logStack) {
				logList.Add (GameManager.instance.client.pb.users [entry ["user"]].name + " " + entry ["verb"]);
			}
			gameText.text = System.String.Join ("\n", logList.Take(20).ToArray());
		}
	}
}
