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
	private List<GameObject> scoreTables;

	private HashSet<string> idSet;

	public GameObject center;
	public float radius;

	// Use this for initialization
	void Start () {
		scoreTables = new List<GameObject> ();
		idSet = new HashSet<string> ();
	}

	void Update(){

		foreach (User user in GameManager.instance.client.pb.users.Values) {
			if (user.isActive && !idSet.Contains (user.id)) {
				UpdateArena ();
				return;
			}
		}
	}

	/// <summary>
	/// Updates arena.
	/// </summary>
	void UpdateArena(){

		// Destroy old
		foreach (GameObject scoreTable in scoreTables) {
			Destroy (scoreTable);
		}
		scoreTables.Clear ();

		int total = GameManager.instance.client.pb.users.Values.Where (user => user.isActive && !GameManager.instance.client.pb.uid.Equals (user.id)).Count() + 1;
		int count = 1;
		foreach (User user in GameManager.instance.client.pb.users.Values.Where (user => user.isActive)) {

			idSet.Add (user.id);

			if (!user.isActive || GameManager.instance.client.pb.uid.Equals (user.id)) {
				continue;
			}

			// Place players in a circle
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

			// Initiate opponent params
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetUID (user.id);
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetPlayer (player);
			playerScoreTable.GetComponent<PlayerScoreTable> ().SetTarget(center);

			scoreTables.Add (playerScoreTable);

			count++;
		}

	}
}
