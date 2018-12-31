using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {
	/// <summary>
	/// Spins me right round, baby, right round on the y axis
	/// </summary>
	
	void Update () {
		gameObject.transform.Rotate (new Vector3(0, 1, 0));
	}
}
