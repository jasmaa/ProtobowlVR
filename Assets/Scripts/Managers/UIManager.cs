using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	// Manages UI updates

	public static UIManager instance;
	public PBClient client;

	public Text personalQuestionText;
	public Text bulletinQuestionText;
	public Text topicText;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}
		
	void Update () {
		personalQuestionText.text = client.questionDisplay.Disp;
		bulletinQuestionText.text = client.questionDisplay.Disp;
		topicText.text = client.pb.GetCategory();
	}
}
