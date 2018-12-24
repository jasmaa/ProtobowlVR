using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class Test : MonoBehaviour {

	public QuestionDisplay qDisp;
	public Text question;
	Protobowl pb;

	// Use this for initialization
	void Start () {

		pb = new Protobowl ();
		StartCoroutine(pb.Init ());

		qDisp.pb = pb;

	}

	void Update(){
		question.text = qDisp.disp;
	}

	public void Buzz(){
		pb.Buzz ();
	}

	public void Guess(){
		pb.Guess ("bananas", true);
	}
}
