using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour {

	public bool isMenuOpen = false;

	private GameObject player;
	private Animator ani;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator> ();
		player = GameObject.Find ("CenterEyeAnchor");
	}
	
	// Update is called once per frame
	void Update () {
		// Face player
		transform.LookAt(player.transform);
		transform.rotation = Quaternion.Euler (new Vector3 (0, transform.rotation.eulerAngles.y, 0));

		transform.position = player.transform.position + offset;
	}

	public void Open(Transform hand){
		if (!isMenuOpen) {
			transform.position = hand.position;
			offset = transform.position - player.transform.position;

			isMenuOpen = true;
			ani.Play ("OptionMenuOpen");
		}
	}
	public void Close(){
		if(isMenuOpen && Vector3.Dot(-transform.forward, player.transform.forward) > 0.9) {
			isMenuOpen = false;
			ani.Play ("OptionMenuClose");
		}
	}
}
