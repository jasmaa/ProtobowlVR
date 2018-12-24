using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBClient : MonoBehaviour {
	// Interfaces game and Protobowl

	public QuestionDisplay questionDisplay;
	Protobowl pb;

	void Start () {

		pb = new Protobowl ();
		StartCoroutine(pb.Init ());

		questionDisplay.pb = pb;

	}
}
