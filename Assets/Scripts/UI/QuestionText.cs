using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionText : MonoBehaviour {
	// Updates text with question

	private Text questionText;

	// Use this for initialization
	void Start () {
		questionText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		questionText.text = GameManager.instance.client.questionDisplay.Disp;
	}
}
