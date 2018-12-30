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
	public IDictionary<string, User> users = new Dictionary<string, User> ();
	public string uid;

	public JSONNode data = JSON.Parse("{}");
	public JSONNode args;
	private JSONNode oldData;

	public bool hasBuzz = false;
	private bool awaitConfirmBuzz = false;

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

				ws.OnMessage += (sender, e) => {

					// Update data on websocket sync
					UpdateData(e.Data);
					UpdateState();
					UpdateUsers();
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
			args = parsedData ["args"][0];
			oldData = data;
			data = Utils.MergeDict (data, args);

			if (awaitConfirmBuzz) {
				if (uid.Equals (args ["attempt"] ["user"])) {
					hasBuzz = true;
				} else {
					hasBuzz = false;
				}
				awaitConfirmBuzz = false;
			}
		}
		else if("joined".Equals(parsedData ["name"])){
			uid = parsedData["args"][0]["id"];
		}
	}

	private void UpdateState(){
		if (data ["real_time"].AsLong - data ["time_offset"].AsLong - data ["end_time"].AsLong < 0) {
			state = Protobowl.GameState.RUNNING;
		} else {
			state = Protobowl.GameState.IDLE;
		}

		// time freeze check
		if (data ["time_freeze"] != 0) {
			state = Protobowl.GameState.BUZZED;
		}
		else if (Utils.containsKey("question", oldData) &&
			Utils.containsKey("question", args) &&
			!oldData ["question"].Equals(args ["question"])) {

			state = Protobowl.GameState.NEW_Q;
			oldData = args;
		}
	}

	public void UpdateUsers(){
		// Updates user names and scores

		foreach (JSONNode userData in args["users"]){

			User user;
			if (!users.ContainsKey (userData ["id"])) {
				user = new User (userData ["id"], userData ["name"], 0);
				users.Add (userData ["id"], user);
			} else {
				user = users [userData ["id"]];
			}

			// update name
			user.name = userData["name"];

			// update score
			int score = 0;

			score += data ["scoring"] ["normal"] [0] * userData ["corrects"] ["normal"];
			score += data ["scoring"] ["early"] [0] * userData ["corrects"] ["early"];
			score += data ["scoring"] ["interrupt"] [0] * userData ["corrects"] ["interrupt"];
			score += data ["scoring"] ["normal"] [1] * userData ["wrongs"] ["normal"];
			score += data ["scoring"] ["early"] [1] * userData ["wrongs"] ["early"];
			score += data ["scoring"] ["interrupt"] [1] * userData ["wrongs"] ["interrupt"];

			user.score = score;

		}
	}

	// === Receive ===

	public string GetQuestionCategory(){
		return data ["info"] ["category"];
	}

	public string GetRoomCategory(){
		return data ["category"];
	}

	public string GetDifficulty(){
		return data ["difficulty"];
	}

	public string GetAnswer(){
		return data ["answer"];
	}

	// === Send ===

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
		awaitConfirmBuzz = true;
	}
	
	public void Guess(string guess, bool done = false){
		// guess answer
		ws.Send ("5:::{\"name\":\"guess\",\"args\":[{\"text\":\"" + guess + "\",\"done\":" + done.ToString().ToLower() + "},null]}");
		Debug.Log (guess);
	}
	
	public void Next(){
		// next question
		ws.Send ("5:::{\"name\":\"next\",\"args\":[null,null]}");
	}
	
	public void Skip(){
		// skip question
		ws.Send ("5:::{\"name\":\"skip\",\"args\":[null,null]}");
	}
	
	public void Pause(){
		// pause question
		ws.Send ("5:::{\"name\":\"pause\",\"args\":[null,null]}");
	}
	
	public void Unpause(){
		// unpause question
		ws.Send ("5:::{\"name\":\"unpause\",\"args\":[null,null]}");
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
		Debug.Log ("5:::{\"name\":\"set_category\",\"args\":[\""+ category +"\",null]}");
	}
}
