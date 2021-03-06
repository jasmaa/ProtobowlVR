﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Selection wheel widget for selecting from a list of options
/// </summary>
public class SelectionWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public GameObject selectionTextTemplate;
	private List<GameObject> selectionTexts;

	public List<string> options;
	private string selected = "";

	public int separationFactor = 50;

	public GameObject selectionArea;
	private Vector2 oldPos = Vector2.zero;

	void Start () {

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

	/// <summary>
	/// Get selected string
	/// </summary>
	public string GetSelected(){
		return selected;
	}

	/// <summary>
	/// Set wheel's state based on query
	/// </summary>
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
		
	/// <summary>
	/// Parse special queries from category and difficulty lists
	/// </summary>
	private string ParseQuery(string s){
		
		if ("Everything".Equals(s) || "Any".Equals(s)) {
			return "";
		}
		return s;
	}
}
