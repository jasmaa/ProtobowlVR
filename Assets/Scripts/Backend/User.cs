﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User {
	// Protobowl user object

	public string id;
	public string name;
	public int score;

	public User(string id, string name, int score){
		this.id = id;
		this.name = name;
		this.score = score;
	}

}