using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour {
	// Updates game question

	public PBClient client;
	public Text questionText;

	void Update () {
		questionText.text = client.questionDisplay.Disp;
	}
}
