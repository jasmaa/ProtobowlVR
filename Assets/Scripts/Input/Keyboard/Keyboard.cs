﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// VR keyboard for Protobowl input fields
/// </summary>
public class Keyboard : MonoBehaviour {

	public enum KeyboardType {
		BUZZ,
		JOIN_ROOM,
		CHANGE_NAME,
		CHAT
	}
	public KeyboardType keyboardType;

	public string text = "brothers";
	public GameObject keyboardButtonTemplate;
	public GameObject spaceButtonTemplate;
	public GameObject backspaceButtonTemplate;
	public GameObject enterButtonTemplate;
	public GameObject shiftButtonTemplate;
	public Text keyboardDisplay;

	private List<KeyboardButton> keyboardButtons;

	private bool isShift = false;
	private int cursorFlick = 0;

	void Start () {
		keyboardButtons = new List<KeyboardButton> ();
		BuildQWERTY ();
		InitText ();
	}

	void Update(){

		keyboardDisplay.text = text;

		// Update flickering cursor
		if (cursorFlick < 50) {
			keyboardDisplay.text += "|";
		}
		cursorFlick = (cursorFlick + 1) % 100;
	}

	/// <summary>
	/// Clears keyboard input text
	/// </summary>
	public void ClearText(){
		text = "";
	}

	/// <summary>
	/// Toggle shift mode
	/// </summary>
	public void ToggleShift(){
		isShift = !isShift;
		SetShift (isShift);
	}

	/// <summary>
	/// Set shift mode on or off
	/// </summary>
	public void SetShift(bool shift){
		foreach (KeyboardButton keyboardButton in keyboardButtons) {
			keyboardButton.SetShift (shift);
		}
	}

	/// <summary>
	/// Backspace
	/// </summary>
	public void Backspace(){
		if (text.Length > 0) {
			text = text.Substring (0, text.Length - 1);
		}
	}

	/// <summary>
	/// Build a bare bones QWERTY keyboard
	/// </summary>
	void BuildQWERTY(){
		
		// build row 1
		string[] rowMain1 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };
		string[] rowShift1 = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" };

		for (int i = 0; i < rowMain1.Length; i++) {
			AddKey (
				keyboardButtonTemplate,
				new Vector2(45 * i + 20, 0),
				rowMain1[i],
				rowShift1[i]
			);
		}

		// build row 2
		string[] rowMain2 = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]" };
		string[] rowShift2 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}" };

		for (int i = 0; i < rowMain2.Length; i++) {
			AddKey (
				keyboardButtonTemplate,
				new Vector2(45 * i + 40, -45),
				rowMain2[i],
				rowShift2[i]
			);
		}

		// build row 3
		string[] rowMain3 = { "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "\\" };
		string[] rowShift3 = { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "|" };

		for (int i = 0; i < rowMain3.Length; i++) {
			AddKey (
				keyboardButtonTemplate,
				new Vector2(45 * i + 60, -90),
				rowMain3[i],
				rowShift3[i]
			);
		}

		// build row 4
		string[] rowMain4 = { "z", "x", "c", "v", "b", "n", "m", ",", ".", "/" };
		string[] rowShift4 = { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?" };

		for (int i = 0; i < rowMain4.Length; i++) {
			AddKey (
				keyboardButtonTemplate,
				new Vector2(45 * i + 80, -135),
				rowMain4[i],
				rowShift4[i]
			);
		}

		// add space
		AddKey(
			spaceButtonTemplate,
			new Vector2(100, -180),
			" ",
			" "
		);

		// add special func keys
		AddFuncKey(backspaceButtonTemplate, new Vector2(535, 0));
		AddFuncKey(enterButtonTemplate, new Vector2(555, -45));
		AddFuncKey(shiftButtonTemplate, new Vector2(15, -135));
		AddFuncKey(shiftButtonTemplate, new Vector2(550, -135));
	}
		
	/// <summary>
	/// Adds a normal key to keyboard
	/// </summary>
	/// <param name="template">Normal key GameObject template</param>
	/// <param name="pos">Position of the key</param>
	/// <param name="mainChar">Unshifted character</param>
	/// <param name="shiftChar">Shifted character</param>
	void AddKey(GameObject template, Vector2 pos, string mainChar, string shiftChar){
		
		GameObject key = Instantiate (template, transform);

		KeyboardButton keyboardButton = key.GetComponent<KeyboardButton> ();
		keyboardButton.mainChar = mainChar;
		keyboardButton.shiftChar = shiftChar;
		keyboardButton.Init ();

		key.GetComponent<RectTransform> ().anchoredPosition = pos;

		keyboardButtons.Add (keyboardButton);
	}

	/// <summary>
	/// Adds a function key to keyboard
	/// </summary>
	/// <param name="template">Normal key GameObject template</param>
	/// <param name="pos">Position of the key</param>
	void AddFuncKey(GameObject template, Vector2 pos){
		GameObject key = Instantiate (template, transform);
		key.GetComponent<RectTransform> ().anchoredPosition = pos;
	}

	/// <summary>
	/// Submit query
	/// </summary>
	public void Submit(){
		if (keyboardType == KeyboardType.BUZZ) {
			GameManager.instance.client.pb.Guess (text, true);
		}
		else if (keyboardType == KeyboardType.JOIN_ROOM) {
			UserData.instance.room = text;
			SceneManager.LoadScene ("GameRoomVR");
		}
		else if (keyboardType == KeyboardType.CHANGE_NAME) {
			PlayerPrefs.SetString ("name", text);
			print (PlayerPrefs.GetString("name"));
		}
		else if (keyboardType == KeyboardType.CHAT) {
			GameManager.instance.client.pb.Chat (text, true);
		}
	}

	/// <summary>
	/// Inits the text.
	/// </summary>
	public void InitText(){
		if (keyboardType == KeyboardType.CHANGE_NAME) {
			text = PlayerPrefs.GetString("name");
		}
	}
}
