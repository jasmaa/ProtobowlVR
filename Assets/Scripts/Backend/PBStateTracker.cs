﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using SimpleJSON;

/// <summary>
/// Handles question display and game state
/// </summary>
public class PBStateTracker : MonoBehaviour {
	
	public Protobowl pb;
	private long localTime = 0;
	private int localIndex = 0;

	private Protobowl.GameState oldState;
	private bool hasAddAnswer = false;

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

	/// <summary>
	/// Gets time passed since question began in seconds
	/// </summary>
	/// <returns>The time passed.</returns>
	public float GetTimePassed(){
		return (pb.data ["real_time"].AsLong - pb.data ["time_offset"].AsLong - pb.data ["begin_time"].AsLong) / 1000f;
	}

	/// <summary>
	/// Gets total question time in seconds
	/// </summary>
	/// <returns>The total time.</returns>
	public float GetTotalTime(){
		return (pb.data ["end_time"].AsLong - pb.data ["begin_time"].AsLong) / 1000f;
	}

	/// <summary>
	/// Gets the percentage time passed.
	/// </summary>
	/// <returns>The percentage time passed.</returns>
	public float GetPercentageTimePassed(){
		return (float) GetTimePassed () / GetTotalTime ();
	}

	/// <summary>
	/// Sets up tracker
	/// </summary>
	void PlayQuestion(){
		InitDisp ();
		StartCoroutine (UpdateDisp());
	}

	/// <summary>
	/// Set tracker to initial display and state
	/// </summary>
	void InitDisp(){
		
		var timePassed = pb.data ["real_time"].AsLong - pb.data ["time_offset"].AsLong - pb.data ["begin_time"].AsLong;

		if (pb.state == Protobowl.GameState.BUZZED) {
			timePassed = pb.data ["time_freeze"].AsLong - pb.data ["begin_time"].AsLong;
		}

		var accum = 0;
		var qList = pb.data ["question"].ToString ().Split (' ');
		List<int> timing = Utils.ConvertJSONToList (pb.data ["timing"].AsArray);
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

	/// <summary>
	/// Update display and state
	/// </summary>
	public IEnumerator UpdateDisp(){
		
		while (true) {
			if (!pb.isConnected ()) {
				continue;
			}

			// detect change in state
			if (oldState != pb.state) {
				oldState = pb.state;

				if (oldState == Protobowl.GameState.RUNNING) {
					localTime = pb.data ["real_time"].AsLong;
				}
			}

			// detect new question
			if (pb.state == Protobowl.GameState.NEW_Q) {
				hasAddAnswer = false;
				pb.state = Protobowl.GameState.RUNNING;
				InitDisp ();
			}

			// run client-side display
			if (pb.state == Protobowl.GameState.RUNNING) {

				pb.hasBuzz = false;

				List<int> timing = Utils.ConvertJSONToList (pb.data ["timing"].AsArray);

				if (localIndex < timing.Count) {
					var qList = pb.data ["question"].ToString ().Split (' ');
					disp = string.Join (" ", qList.Take (localIndex + 1).ToArray ());

					var currentInterval = Mathf.Round (timing [localIndex] * pb.data ["rate"]);
					yield return new WaitForSeconds (currentInterval / 1000);
					localTime += (long) currentInterval;
					localIndex++;
				}
				else {
					var timePassed = (localTime - pb.data ["time_offset"].AsLong - pb.data ["begin_time"].AsLong) / 1000f;
					var currentInterval = 100;
					if (timePassed < GetTotalTime ()) {
						localTime += currentInterval;
						yield return new WaitForSeconds (currentInterval / 1000f);
					} else {
						pb.state = Protobowl.GameState.IDLE;
					}
				}
			} else {
				yield return new WaitForSeconds (0.1f);
			}

			// auto-fill when question ends
			if (pb.state == Protobowl.GameState.IDLE) {
				disp = pb.data ["question"];

				// add answer to log
				if (!hasAddAnswer) {
					pb.answerStack.Push (pb.GetAnswer());
					hasAddAnswer = true;
				}
			}

			// ping server
			pb.Ping ();
		}
	}
}
