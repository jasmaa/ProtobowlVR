using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Manages game arena
/// </summary>
public class ArenaManager : MonoBehaviour {

	public GameObject player;

	public GameObject scoreTableTemplate;
	List<GameObject> scoreTables;

	HashSet<string> idSet;

	public GameObject center;
	public float radius;

	// Use this for initialization
	void Start () {
		scoreTables = new List<GameObject> ();
		idSet = new HashSet<string> ();
	}

	void Update(){
		foreach (string id in GameManager.instance.client.pb.users.Keys) {
			if (!idSet.Contains (id)) {
				UpdateArena ();
				return;
			}
		}
	}

	/// <summary>
	/// Updates arena.
	/// </summary>
	void UpdateArena(){
		
		foreach (GameObject scoreTable in scoreTables) {
			Destroy (scoreTable);
		}
		scoreTables.Clear ();

		int total = GameManager.instance.client.pb.users.Values.Where(user => !GameManager.instance.client.pb.uid.Equals(user.id) && user.isActive).Count() + 1;
		int count = 1;
		foreach (User user in GameManager.instance.client.pb.users.Values) {

			idSet.Add (user.id);

			if (!user.isActive || GameManager.instance.client.pb.uid.Equals(user.id) || GameManager.instance.client.pb.uid.Equals (user.id)) {
				continue;
			}

			GameObject playerScoreTable = Instantiate (scoreTableTemplate, transform);
			playerScoreTable.transform.position = new Vector3 (
				radius * Mathf.Sin (2 * Mathf.PI * ((float)count / total)),
				playerScoreTable.transform.position.y,
				-radius * Mathf.Cos (2 * Mathf.PI * ((float)count / total))
			);
			playerScoreTable.transform.position += new Vector3(
				center.transform.position.x,
				0,
				center.transform.position.z
			);
				
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetUID (user.id);
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetPlayer (player);
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetTarget(center);

			scoreTables.Add (playerScoreTable);

			count++;
		}

	}
}
