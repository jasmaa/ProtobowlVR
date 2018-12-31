using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftButton : MonoBehaviour {
	/// <summary>
	/// VR keyboard shift button
	/// </summary>
	
	public Keyboard keyboard;

	void Start () {
		keyboard = transform.parent.GetComponent<Keyboard> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			keyboard.ToggleShift();
		});
	}

}
