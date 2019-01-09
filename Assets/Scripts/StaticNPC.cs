using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Non-opponent robo-avatar
/// </summary>
public class StaticNPC : MonoBehaviour {

	private Animator ani;

	private string[] clipNames = {
		"RoboAvatar_Idle2",
		"RoboAvatar_Idle3",
		"RoboAvatar_Confuse",
		"RoboAvatar_Shock"
	};

	void Start () {
		ani = GetComponent<Animator> ();
	}

	void Update () {
		// Play random animation
		if (ani.GetCurrentAnimatorStateInfo (0).IsName ("RoboAvatar_Idle") && Random.Range(0, 1000) >= 999) {
			ani.Play (clipNames[Random.Range(0, clipNames.Length)]);
		}
	}
}
