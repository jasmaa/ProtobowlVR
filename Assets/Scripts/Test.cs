using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	Protobowl p;

	// Use this for initialization
	void Start () {

		p = new Protobowl ();
		StartCoroutine(p.Init ());
	}

	public void Buzz(){
		p.Buzz ();
	}

	public void Guess(){
		p.Guess ("bananas", false);
	}
}
