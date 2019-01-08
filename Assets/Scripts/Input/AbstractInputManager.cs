using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract input manager
/// </summary>
public abstract class AbstractInputManager : MonoBehaviour {
	public abstract void TurnOn();
	public abstract void TurnOff();
}
