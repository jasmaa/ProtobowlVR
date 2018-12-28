﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	// Selection wheel for list of strings

	public GameObject selectionTextTemplate;
	private List<GameObject> selectionTexts;

	public List<string> options;
	private string selected;

	public int separationFactor = 50;

	private GameObject selectionArea;
	private Vector2 oldPos = Vector2.zero;

	// Use this for initialization
	void Start () {

		selectionArea = transform.Find ("SelectionArea").gameObject;

		selectionTexts = new List<GameObject> ();
		int counter = 0;
		foreach (string option in options) {
			GameObject selectionText = Instantiate (selectionTextTemplate, selectionArea.transform);
			selectionText.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -separationFactor * counter);
			selectionText.GetComponent<Text> ().text = option;

			selectionTexts.Add (selectionText);
			counter++;
		}
	}

	public string GetSelected(){
		return selected;
	}

	public void SetSelected(string query){

		query = ParseQuery (query);

		if (query.Equals (selected)) {
			return;
		}

		int index = -1;
		for (int i=0; i < options.Count; i++) {
			if(query.Equals(options[i])){
				index = i;
				break;
			}
		}
		if (index != -1) {
			selectionArea.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, index * separationFactor);
			selected = query;
		}
	}

	public void OnBeginDrag(PointerEventData data){
		oldPos = data.position;
	}

	public void OnDrag(PointerEventData data){

		Vector2 diff = data.position - oldPos;
		selectionArea.GetComponent<RectTransform>().anchoredPosition += new Vector2 (0, diff.y);

		selectionArea.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0, Mathf.Clamp (
			selectionArea.GetComponent<RectTransform>().anchoredPosition.y,
			0,
			separationFactor * (options.Count - 1)
		));

		// set old pos
		oldPos = data.position;
	}

	public void OnEndDrag(PointerEventData data){
		selectionArea.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (
			0, Mathf.Round(selectionArea.GetComponent<RectTransform>().anchoredPosition.y / separationFactor) * separationFactor
		);

		// set option
		selected = ParseQuery(options[(int)(selectionArea.GetComponent<RectTransform>().anchoredPosition.y / separationFactor)]);
	}

	private string ParseQuery(string s){
		if (!options.Contains(s)) {
			return "";
		}
		return s;
	}
}
