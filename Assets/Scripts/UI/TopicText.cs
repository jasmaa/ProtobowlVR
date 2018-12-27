using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicText : MonoBehaviour {
	// Updates text with topic

	private Text topicText;

	// Use this for initialization
	void Start () {
		topicText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		topicText.text = GameManager.instance.client.pb.GetCategory();
	}
}
