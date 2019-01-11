using System;
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
		TOPIC,
		ANSWER_LOG,
		SCORE
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
			gameText.text = GameManager.instance.client.pb.GetLogStr (20);
		} else if (textType == TextType.ANSWER_LOG) {
			gameText.text = GameManager.instance.client.pb.GetAnswerLogStr (20);
		} else if (textType == TextType.SCORE) {
			if (GameManager.instance.client.pb.uid != null) {
				if (GameManager.instance.client.pb.users.ContainsKey (GameManager.instance.client.pb.uid)) {
					gameText.text = GameManager.instance.client.pb.users [GameManager.instance.client.pb.uid].score + "";
				}
			}
		}
	}
}
