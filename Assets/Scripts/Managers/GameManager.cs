using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages front end game
/// </summary>
public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public PBClient client;

	public GameObject countdownBar;

	public AbstractInputManager inputManager;

	// COOLDOWNS and LOCKOUTS
	private float stateUpdateCooldown = 0;
	private bool buzzLockout = false;

	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}
	
	void Update () {

		// === Update based on state ===
		if (stateUpdateCooldown == 0 && client.pb.state != Protobowl.GameState.BUZZED && client.pb.state != Protobowl.GameState.PROMPTED) {
			
			inputManager.TurnOff ();
			buzzLockout = false;
			stateUpdateCooldown = 1;
		}

		// Update cooldowns
		stateUpdateCooldown = Mathf.Clamp(stateUpdateCooldown - Time.deltaTime, 0, 9999);
	}

	/// <summary>
	/// Handle player buzz
	/// </summary>
	public void PlayerBuzz(){
		if (buzzLockout) {
			return;
		}

		client.pb.Buzz ();
		buzzLockout = true;

		Invoke ("DetectBuzz", 0.05f);
	}

	/// <summary>
	/// Detect and update client if player claimed the buzz
	/// </summary>
	void DetectBuzz(){
		if (client.pb.hasBuzz) {
			inputManager.TurnOn ();
			countdownBar.GetComponent<CountdownBar> ().Reset ();
		}
	}
}
