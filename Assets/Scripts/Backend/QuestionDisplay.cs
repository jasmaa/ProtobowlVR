using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using SimpleJSON;

public class QuestionDisplay : MonoBehaviour {
	// Handles question display and game state

	public Protobowl pb;
	private float localTime = 0;
	private int localIndex = 0;

	private string disp = "";
	public string Disp
	{
		get {
			return disp;
		}
	}

	void Start(){
		Invoke ("PlayQuestion", 0.5f);
	}

	void PlayQuestion(){
		InitDisp ();
		StartCoroutine (UpdateDisp());
	}

	void InitDisp(){
		// init display

		var timePassed = pb.data ["real_time"].AsLong - pb.data ["time_offset"].AsLong - pb.data ["begin_time"].AsLong;

		if (pb.state == Protobowl.GameState.BUZZED) {
			timePassed = pb.data ["time_freeze"].AsLong - pb.data ["begin_time"].AsLong;
		}

		var accum = 0;
		var qList = pb.data ["question"].ToString ().Split (' ');
		List<int> timing = Utils.convertJSONToList (pb.data ["timing"].AsArray);
		for (int i = 0; i < timing.Count; i++) {
			localIndex = i;
			accum += (int)Mathf.Round (timing [i] * pb.data ["rate"]);

			if (accum - timePassed >= 0) {
				break;
			}
		}

		disp = string.Join (" ", qList.Take (localIndex + 1).ToArray ());
		localTime = pb.data ["real_time"].AsLong;
	}

	IEnumerator UpdateDisp(){
		// update display
		while (true) {
			// detect new question
			if (pb.state == Protobowl.GameState.NEW_Q) {
				pb.state = Protobowl.GameState.RUNNING;
				InitDisp ();
			}

			// run client-side display
			if (pb.state == Protobowl.GameState.RUNNING) {

				List<int> timing = Utils.convertJSONToList (pb.data ["timing"].AsArray);

				if (localIndex < timing.Count) {
					var qList = pb.data ["question"].ToString ().Split (' ');
					disp = string.Join (" ", qList.Take (localIndex + 1).ToArray ());

					var currentInterval = Mathf.Round (timing [localIndex] * pb.data ["rate"]);
					yield return new WaitForSeconds (currentInterval / 1000);
					localTime += currentInterval;
					localIndex++;
				} else {
					pb.state = Protobowl.GameState.IDLE;
				}
			} else {
				yield return new WaitForSeconds (0.1f);
			}

			// auto-fill when question ends
			if (pb.state == Protobowl.GameState.IDLE) {
				disp = pb.data ["question"];
			}

			//print (pb.state);
		}
	}
}
