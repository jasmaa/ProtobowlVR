using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Protobowl user object
/// </summary>
public class User {

	public string id;
	public string name;
	public int score;
	public bool isActive;

	public User(string id, string name, int score, bool isActive){
		this.id = id;
		this.name = name;
		this.score = score;
		this.isActive = isActive;
	}

}
