using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownBar : MonoBehaviour {

	public GameObject movingBar;
	public GameObject backingBar;

	private float attemptTime = 8;
	private float currTime;

	private float initScale;

	// Use this for initialization
	void Start () {
		initScale = movingBar.transform.GetComponent<RectTransform> ().localScale.x;
		backingBar.SetActive (false);
		movingBar.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		currTime = Mathf.Clamp(currTime - Time.deltaTime, 0, attemptTime);
		movingBar.transform.GetComponent<RectTransform> ().localScale = new Vector3 (initScale * currTime / attemptTime, initScale, initScale);

		if (currTime == 0 || GameManager.instance.client.pb.state != Protobowl.GameState.BUZZED) {
			backingBar.SetActive (false);
			movingBar.SetActive (false);
		}
	}

	public void Reset(){
		currTime = attemptTime;
		backingBar.SetActive (true);
		movingBar.SetActive (true);
	}
}
