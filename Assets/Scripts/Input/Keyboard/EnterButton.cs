using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VR keyboard enter button
/// </summary>
public class EnterButton : MonoBehaviour {

	public Keyboard keyboard;

	void Start () {
		keyboard = transform.parent.GetComponent<Keyboard> ();

		GetComponent<Button> ().onClick.AddListener(() => {
			keyboard.Submit();
		});
	}

}
