using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays buzz countdown time
/// </summary>
public class CountdownBar : MonoBehaviour {

	public enum Mode {
		UNINIT,
		RUNNING,
		BUZZED,
		PAUSED,
		PROMPTED,
		IDLE
	}
	public Mode mode = Mode.UNINIT;

	private Protobowl.GameState oldState;

	public GameObject movingBar;
	public GameObject backingBar;

	private const float ATTEMPT_TIME = 8;

	private float targetTime = 1;
	private float currTime;
	private bool isMoving = true;

	private float initScale;

	void Start () {
		initScale = movingBar.transform.GetComponent<RectTransform> ().localScale.x;
	}
		
	void Update () {
		
		// Check for mode update
		if (GameManager.instance.client.pb.state != oldState) {
			oldState = GameManager.instance.client.pb.state;
			if (oldState == Protobowl.GameState.RUNNING) {
				SetMode(Mode.RUNNING);
			}
			else if(oldState == Protobowl.GameState.BUZZED){
				SetMode(Mode.BUZZED);
			}
			else if(oldState == Protobowl.GameState.PAUSED){
				SetMode(Mode.PAUSED);
			}
			else if(oldState == Protobowl.GameState.PROMPTED){
				SetMode(Mode.PROMPTED);
			}
			else if(oldState == Protobowl.GameState.IDLE){
				SetMode(Mode.IDLE);
			}

		}

		// Update bar
		if(isMoving){
			currTime = Mathf.Clamp(currTime + Time.deltaTime, 0, targetTime);
		}

		if (targetTime != 0) {
			movingBar.transform.GetComponent<RectTransform> ().localScale = new Vector3 (initScale * currTime / targetTime, initScale, initScale);
		}
	}

	/// <summary>
	/// Reset time
	/// </summary>
	public void Reset(){
		backingBar.SetActive (true);
		movingBar.SetActive (true);
	}

	/// <summary>
	/// Set countdown mode and reset
	/// </summary>
	public void SetMode(Mode mode){
		this.mode = mode;

		// Change bar color
		Image barImg = movingBar.GetComponent<Image>();
		if (mode == Mode.RUNNING) {
			targetTime = GameManager.instance.client.tracker.GetTotalTime ();
			currTime = GameManager.instance.client.tracker.GetTimePassed ();
			barImg.color = new Color (51/255f, 122/255f, 183/255f, 200/255f);
			isMoving = true;
		}
		else if(mode == Mode.BUZZED){
			targetTime = ATTEMPT_TIME;
			currTime = 0;
			barImg.color = new Color (217/255f, 83/255f, 79/255f, 200/255f);
			isMoving = true;
		}
		else if(mode == Mode.PROMPTED){
			targetTime = GameManager.instance.client.pb.GetPromptTime ();
			currTime = 0;
			barImg.color = new Color (91/255f, 192/255f, 222/255f, 200/255f);
			isMoving = true;
		}
		else if(mode == Mode.PAUSED){
			targetTime = GameManager.instance.client.tracker.GetTotalTime ();
			currTime = GameManager.instance.client.tracker.GetTimePassed ();
			barImg.color = new Color (240/255f, 173/255f, 78/255f, 200/255f);
			isMoving = false;
		}
		else if(mode == Mode.IDLE){
			targetTime = 1;
			currTime = 1;
			currTime = targetTime;
			barImg.color = new Color (128/255f, 128/255f, 128/255f, 200/255f);
			isMoving = false;
		}
	}
}
