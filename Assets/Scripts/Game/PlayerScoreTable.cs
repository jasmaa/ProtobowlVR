using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opponent player score table
/// </summary>
public class PlayerScoreTable : MonoBehaviour {

	private Animator ani;
	private GameObject player;
	private string uid;
	private bool hasBuzz;

	private string[] clipNames = {
		"PlayerScoreTable_Embarrassed",
		"PlayerScoreTable_Thinking",
		"PlayerScoreTable_Taunt"
	};

	void Start(){
		ani = GetComponent<Animator> ();
	}

	void Update(){

		if (uid == null) {
			return;
		}

		// Update name board and score
		if (GameManager.instance.client.pb.users.ContainsKey(uid)) {
			transform.Find("NameBoard").GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.instance.client.pb.users [uid].name;
			transform.Find("ScoreBoard").GetChild(0).GetComponent<Text>().text = GameManager.instance.client.pb.users [uid].score + "";
		}

		// Check if buzzed
		if(GameManager.instance.client.pb.state == Protobowl.GameState.BUZZED && uid.Equals(GameManager.instance.client.pb.buzzUid)){
			hasBuzz = true;
			ani.Play ("PlayerScoreTable_Buzz");
		}
		else if((GameManager.instance.client.pb.state == Protobowl.GameState.RUNNING || GameManager.instance.client.pb.state == Protobowl.GameState.IDLE) && hasBuzz){
			if (GameManager.instance.client.pb.correct) {
				ani.Play ("PlayerScoreTable_Victory");
			} else {
				ani.Play ("PlayerScoreTable_Shock");
			}
			hasBuzz = false;
		}

		// Play random animation
		if (ani.GetCurrentAnimatorStateInfo (0).IsName ("PlayerScoreTable_Idle") && Random.Range(0, 1000) >= 999) {
			ani.Play (clipNames[Random.Range(0, clipNames.Length)]);
		}
	}

	/// <summary>
	/// Sets the target.
	/// </summary>
	public void SetTarget(GameObject target){
		transform.Find ("Cord").GetComponent<Rope> ().target = target;
		transform.GetComponent<FaceTarget> ().target = target;
	}

	/// <summary>
	/// Sets the player.
	/// </summary>
	/// <param name="player">VR camera rig</param>
	public void SetPlayer(GameObject player){
		transform.Find("NameBoard").GetComponent<FaceTarget> ().target = player;
	}

	/// <summary>
	/// Set user id
	/// </summary>
	/// <param name="uid">User id</param>
	public void SetUID(string uid){
		this.uid = uid;
	}
}
