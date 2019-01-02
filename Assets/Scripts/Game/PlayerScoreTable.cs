using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opponent player score table
/// </summary>
public class PlayerScoreTable : MonoBehaviour {

	private GameObject player;
	private string uid;

	void Update(){
		if (uid != null && GameManager.instance.client.pb.users.ContainsKey(uid)) {
			transform.Find("NameBoard").GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.instance.client.pb.users [uid].name;
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
