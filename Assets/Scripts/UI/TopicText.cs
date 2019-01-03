using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates text with question topic
/// <summary>
public class TopicText : MonoBehaviour {

	private Text topicText;

	// Use this for initialization
	void Start () {
		topicText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		topicText.text = GameManager.instance.client.pb.GetQuestionCategory();
	}
}
