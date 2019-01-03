using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates text with current question text
/// </summary>
public class QuestionText : MonoBehaviour {

	private Text questionText;

	// Use this for initialization
	void Start () {
		questionText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		questionText.text = GameManager.instance.client.tracker.Disp;
	}
}
