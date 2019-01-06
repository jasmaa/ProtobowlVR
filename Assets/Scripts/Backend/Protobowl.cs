using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using WebSocketSharp;
using SimpleJSON;

/// <summary>
/// Bare bones Protobowl API
/// </summary>
public class Protobowl {

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
	public string buzzUid;
	public bool correct;

	public Stack<JSONNode> logStack = new Stack<JSONNode>();

	private const string server = "ocean.protobowl.com:443/socket.io/1/websocket/";
	private string socketString;
	private string cookie;
	private WebSocket ws;
	private bool connected = false;

	/// <summary>
	/// Initialize socket connection
	/// </summary>
	/// <param name="roomName">Room name</param>
	/// <param name="cookie">User cookie</param>
	public IEnumerator Connect(string roomName, string cookie){
		
		this.cookie = cookie;

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

				ws.OnOpen += (sender, e) => {
					connected = true;

					// join room
					JoinRoom (roomName);
				};

				ws.OnMessage += (sender, e) => {
					// Update data on websocket sync
					UpdateData(e.Data);
					UpdateState();
					UpdateUsers();
				};

				ws.ConnectAsync ();
			}
		}
	}

	/// <summary>
	/// Disconnect socket
	/// </summary>
	public void Disconnect(){
		
		if (connected) {
			connected = false;
			ws.CloseAsync ();
		}
	}

	/// <summary>
	/// Checks if connected.
	/// </summary>
	/// <returns><c>true</c>, if connected was ised, <c>false</c> otherwise.</returns>
	public bool isConnected(){
		return connected;
	}

	/// <summary>
	/// Updates data
	/// <summary>
	/// <param name="rawData">Raw socket response</param>
	private void UpdateData(string rawData){
		
		JSONNode parsedData = JSON.Parse (rawData.Substring(4));

		if ("sync".Equals (parsedData ["name"])) {
			args = parsedData ["args"] [0];
			oldData = data;
			data = Utils.MergeDict (data, args);

			// update on attempt
			if (args ["attempt"] != null) {
				buzzUid = args ["attempt"] ["user"];
				correct = args ["attempt"] ["correct"];

				// log on done
				if (args ["attempt"] ["done"]) {
					JSONNode entry = new JSONObject ();
					entry.Add ("user", args ["attempt"] ["user"]);
					entry.Add ("verb", "guessed " + args ["attempt"] ["text"]  + " (" + (args["attempt"]["correct"] ? "CORRECT" : "WRONG") + ")");
					logStack.Push (entry);
				}
			}

			// Determine if user can claim buzz
			if (awaitConfirmBuzz) {
				hasBuzz = uid.Equals (buzzUid);
				awaitConfirmBuzz = false;
			}
		}
		else if ("joined".Equals (parsedData ["name"])) {
			uid = parsedData ["args"] [0] ["id"];
		}
		else if ("log".Equals (parsedData ["name"])) {
			JSONNode logData = parsedData ["args"] [0];
			logStack.Push (logData);
		}
		else if ("chat".Equals (parsedData ["name"])) {
			args = parsedData ["args"] [0];

			// log on done
			if (args ["done"]) {
				JSONNode entry = new JSONObject ();
				entry.Add ("user", args ["user"]);
				entry.Add ("verb", ": " + args ["text"]);
				logStack.Push (entry);
			}
		}
	}

	/// <summary>
	/// Update client state based on socket response
	/// </summary>
	private void UpdateState(){
		
		if (data ["real_time"].AsLong - data ["time_offset"].AsLong - data ["end_time"].AsLong < 0) {
			state = Protobowl.GameState.RUNNING;
		} else {
			state = Protobowl.GameState.IDLE;
		}

		// time freeze check
		if (data ["time_freeze"] != 0) {
			if (args ["attempt"] == null) {
				state = Protobowl.GameState.PAUSED;
			}
			else if (args ["attempt"] ["prompt"] == true) {
				state = Protobowl.GameState.PROMPTED;
			}
			else {
				state = Protobowl.GameState.BUZZED;
			}
		}
		else if (Utils.containsKey("question", oldData) &&
			Utils.containsKey("question", args) &&
			!oldData ["question"].Equals(args ["question"])) {

			state = Protobowl.GameState.NEW_Q;
			oldData = args;
		}
	}

	/// <summary>
	/// Updates user names and scores
	/// </summary>
	public void UpdateUsers(){

		foreach (JSONNode userData in args["users"]){

			var isActive = args ["real_time"].AsLong - userData ["last_action"].AsLong < 120000;

			User user;
			if (!users.ContainsKey (userData ["id"])) {
				user = new User (userData ["id"], userData ["name"], 0, isActive);
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

			// update active
			user.isActive = isActive;
		}
			
	}

	// === Receive ===

	/// <summary>
	/// Get question category
	/// </summary>
	public string GetQuestionCategory(){
		return data ["info"] ["category"];
	}

	/// <summary>
	/// Get room category
	/// </summary>
	public string GetRoomCategory(){
		return data ["category"];
	}

	/// <summary>
	/// Get room difficulty
	/// </summary>
	public string GetDifficulty(){
		return data ["difficulty"];
	}

	/// <summary>
	/// Get question answer
	/// </summary>
	public string GetAnswer(){
		return data ["answer"];
	}

	/// <summary>
	/// Get prompt time in seconds
	/// </summary>
	/// <returns>The prompt time.</returns>
	public float GetPromptTime(){
		return data ["prompt_duration"] / 1000;
	}

	// === Send ===

	/// <summary>
	/// Join room
	/// </summary>
	/// <param name="roomName">Room to join</param>
	public void JoinRoom(string roomName){
		ws.Send ("5:::{\"name\":\"join\",\"args\":[{\"cookie\":\"" +
			cookie +
			"\",\"auth\":null,\"question_type\":\"qb\",\"room_name\":\"" +
			roomName +
			"\",\"muwave\":false,\"agent\":\"M4/Web\",\"agent_version\":\"Sat Sep 02 2017 11:33:43 GMT-0700 (PDT)\",\"version\":8}]}");
	}

	/// <summary>
	/// Set player handle
	/// </summary>
	/// <param name="name">Handle name</param>
	public void SetName(string name){
		ws.Send ("5:::{\"name\":\"set_name\",\"args\":[\"" + name + "\",null]}");
	}

	/// <summary>
	/// Buzz on question
	/// </summary>
	public void Buzz(){
		ws.Send ("5:23+::{\"name\":\"buzz\",\"args\":[\"" + data["qid"] + "\"]}");
		awaitConfirmBuzz = true;
	}

	/// <summary>
	/// Guess answer
	/// </summary>
	/// <param name="guess">Answer to guess</param>
	/// <param name="done">Whether guess is done</param>
	public void Guess(string guess, bool done = false){
		ws.Send ("5:::{\"name\":\"guess\",\"args\":[{\"text\":\"" + guess + "\",\"done\":" + done.ToString().ToLower() + "},null]}");
	}

	/// <summary>
	/// Go to next question
	/// </summary>
	public void Next(){
		ws.Send ("5:::{\"name\":\"next\",\"args\":[null,null]}");
	}

	/// <summary>
	/// Skip question
	/// </summary>
	public void Skip(){
		ws.Send ("5:::{\"name\":\"skip\",\"args\":[null,null]}");
	}

	/// <summary>
	/// Pause question
	/// </summary>
	public void Pause(){
		ws.Send ("5:::{\"name\":\"pause\",\"args\":[null,null]}");
	}

	/// <summary>
	/// Unpause question
	/// </summary>
	public void Unpause(){
		ws.Send ("5:::{\"name\":\"unpause\",\"args\":[null,null]}");
	}

	/// <summary>
	/// Reset user score
	/// </summary>
	public void ResetScore(){
		ws.Send ("5:::{\"name\":\"reset_score\",\"args\":[null,null]}");
	}

	/// <summary>
	/// Ping server to keep connection alive
	/// </summary>
	public void Ping(){		
		ws.Send ("2::");
	}

	/// <summary>
	/// Send message through chat
	/// </summary>
	/// <param name="message">Message to send</param>
	/// <param name="done">Whether message sending is done</param>
	public void Chat(string message, bool done = false){
		ws.Send ("5:::{\"name\":\"chat\",\"args\":[{\"text\":\""+ message +"\",\"session\":null,\"first\":false,\"" + done.ToString().ToLower() + "\":false},null]}");
	}

	/// <summary>
	/// Set difficulty
	/// </summary>
	/// <param name="difficulty">Difficulty string</param>
	public void SetDifficulty(string difficulty){
		ws.Send ("5:::{\"name\":\"set_difficulty\",\"args\":[\""+ difficulty +"\",null]}");
	}

	/// <summary>
	/// Set category
	/// </summary>
	/// <param name="category">Category string</param>
	public void SetCategory(string category){
		ws.Send ("5:::{\"name\":\"set_category\",\"args\":[\""+ category +"\",null]}");
	}
}
