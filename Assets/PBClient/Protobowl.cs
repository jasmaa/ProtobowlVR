using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

public class Protobowl {

	private string server = "ocean.protobowl.com:443/socket.io/1/websocket/";
	private string socketString;
	private string cookie = "derp";
	private WebSocket ws;

	public IEnumerator GetSocket(){
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

				Debug.Log ("ws://" + server + socketString);

				ws.OnMessage += (sender, e) => {
					Debug.Log(e.Data);
				};

				ws.Connect ();

				ws.Send ("5:::{\"name\":\"join\",\"args\":[{\"cookie\":\"" +
					cookie +
					"\",\"auth\":null,\"question_type\":\"qb\",\"room_name\":\"" +
					"bot-testing-vr" +
					"\",\"muwave\":false,\"agent\":\"M4/Web\",\"agent_version\":\"Sat Sep 02 2017 11:33:43 GMT-0700 (PDT)\",\"version\":8}]}");
			}
		}
	}

	public void JoinRoom(string roomName){
		ws.Send ("5:::{\"name\":\"join\",\"args\":[{\"cookie\":" +
		cookie +
		",\"auth\":null,\"question_type\":\"qb\",\"room_name\":" +
		roomName +
		"\",\"muwave\":false,\"agent\":\"M4/Web\",\"agent_version\":\"Sat Sep 02 2017 11:33:43 GMT-0700 (PDT)\",\"version\":8}]}");
	}
}
