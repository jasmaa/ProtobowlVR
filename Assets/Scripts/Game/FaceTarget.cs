using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Face target.
/// </summary>
public class FaceTarget : MonoBehaviour {

	public GameObject target;

	void Update(){
		if (target != null) {
			transform.LookAt (target.transform);
			transform.rotation = Quaternion.Euler (new Vector3(
				0,
				transform.eulerAngles.y,
				0
			));
		}
	}
}
