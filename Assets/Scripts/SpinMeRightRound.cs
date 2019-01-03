using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spins me right round, baby, right round on axis
/// </summary>
public class SpinMeRightRound : MonoBehaviour {

	public Vector3 axis;

	void Update () {
		gameObject.transform.Rotate (axis);
	}
}
