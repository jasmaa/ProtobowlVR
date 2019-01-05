using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates text with time
/// </summary>
public class TimeText : MonoBehaviour {

	private Text timeText;

	void Start () {
		timeText = GetComponent<Text> ();
	}

	void Update () {
		timeText.text = System.DateTime.Now.ToLongTimeString();	
	}
}
