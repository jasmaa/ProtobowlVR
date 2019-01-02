using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {
	/// <summary>
	/// Spins me right round, baby, right round on axis
	/// </summary>

	public Vector3 axis;

	void Update () {
		gameObject.transform.Rotate (axis);
	}
}
