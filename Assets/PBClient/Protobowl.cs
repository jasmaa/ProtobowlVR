using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using WebSocketSharp;
using SimpleJSON;

public class Protobowl {
	// Bare bones PB API

	public enum GameState {
		RUNNING,
		PAUSED,
		BUZZED,
		PROMPTED,
		IDLE,
		NEW_Q
	}
	public GameState state = GameState.NEW_Q;

	public JSONNode data = JSON.Parse("{}");
	private JSONNode oldData;

	private const string server = "ocean.protobowl.com:443/socket.io/1/websocket/";

	private string socketString;
	private string cookie = "derp";
	private WebSocket ws;
	
	public IEnumerator Init(){
		// Initialize socket
		
		// get socket id
		using (UnityWebRequest www = UnityWebRequest.Get("http://"+server)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else {
				Debug.Log (www.downloadHandler.text);

				// set up socket
				socketString = www.downloadHandler.text.Split (':')[0];
				ws = new WebSocket ("ws://" + server + socketString);

				//Debug.Log ("ws://" + server + socketString);

				ws.OnMessage += (sender, e) => {
					// Update data on websocket sync
					//Debug.Log(e.Data);
					UpdateData(e.Data);
					UpdateState();
					Ping();
				};

				ws.Connect ();

				// TEMP: join room
				JoinRoom ("bot-testing-vr");
				SetName ("Unity Bot");
			}
		}
	}

	private void UpdateData(string rawData){
		// Updates data
		JSONNode parsedData = JSON.Parse (rawData.Substring(4));

		if ("sync".Equals(parsedData ["name"])) {
			JSONNode args = parsedData ["args"][0];
			//data = args;
			oldData = data;
			data = Utils.MergeDict (data, args);

			Debug.Log ("UPDATE");
		}
	}

	private void UpdateState(){
		if (data ["real_time"] - data ["time_offset"] - data ["end_time"] < 0) {
			state = Protobowl.GameState.RUNNING;
		} else {
			state = Protobowl.GameState.IDLE;
		}

		// time freeze check
		if (data ["time_freeze"] != 0) {
			state = Protobowl.GameState.BUZZED;
		}
		else if (Utils.containsKey("question", oldData) &&
			Utils.containsKey("question", data)&&
			oldData ["question"] != data ["question"]) {
			state = Protobowl.GameState.NEW_Q;
		}
	}


	public void JoinRoom(string roomName){
		// joins room
		ws.Send ("5:::{\"name\":\"join\",\"args\":[{\"cookie\":\"" +
			cookie +
			"\",\"auth\":null,\"question_type\":\"qb\",\"room_name\":\"" +
			roomName +
			"\",\"muwave\":false,\"agent\":\"M4/Web\",\"agent_version\":\"Sat Sep 02 2017 11:33:43 GMT-0700 (PDT)\",\"version\":8}]}");
	}

	public void SetName(string name){
		// sets player handle
		ws.Send ("5:::{\"name\":\"set_name\",\"args\":[\"" + name + "\",null]}");
	}
	
	public void Buzz(){
		// buzz
		ws.Send ("5:23+::{\"name\":\"buzz\",\"args\":[\"" + data["qid"] + "\"]}");
	}
	
	public void Guess(string guess, bool done = false){
		// guess answer
		ws.Send ("5:::{\"name\":\"guess\",\"args\":[{\"text\":\"" + guess + "\",\"done\":" + done.ToString().ToLower() + "},null]}");
	}
	
	public void Next(){
		// next question
		ws.Send ("5:::{\"name\":\"next\",\"args\":[null,null]}");
	}
	
	public void Skip(){
		// skip question
		ws.Send ("5:::{\"skip\":\"next\",\"args\":[null,null]}");
	}
	
	public void Pause(){
		// pause question
		ws.Send ("5:::{\"pause\":\"next\",\"args\":[null,null]}");
	}
	
	public void Unpause(){
		// unpause question
		ws.Send ("5:::{\"unpause\":\"next\",\"args\":[null,null]}");
	}
	
	public void Ping(){
		// unpause question
		ws.Send ("2::");
	}
	
	public void Chat(string message, bool done = false){
		// chat
		ws.Send ("5:::{\"name\":\"chat\",\"args\":[{\"text\":\""+ message +"\",\"session\":null,\"first\":false,\"" + done.ToString().ToLower() + "\":false},null]}");
	}
	
	public void SetDifficulty(string difficulty){
		// set difficulty
		ws.Send ("5:::{\"name\":\"set_difficulty\",\"args\":[\""+ difficulty +"\",null]}");
	}
	
	public void SetCategory(string category){
		// set category
		ws.Send ("5:::{\"name\":\"set_category\",\"args\":[\""+ category +"\",null]}");
	}
}
