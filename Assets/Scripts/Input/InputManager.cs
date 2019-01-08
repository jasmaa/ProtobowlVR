using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Normal input manager
/// </summary>
public class InputManager : AbstractInputManager {

	void Start () {
		TurnOff ();
	}

	void Update () {
		
	}

	public override void TurnOn () {
		print ("turn on");
	}
	public override void TurnOff(){
		print ("turn off");
	}
}
