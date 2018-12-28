using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionList : MonoBehaviour {
	// Handles selectable options

	private GameObject categoryWheel;
	private GameObject difficultyWheel;

	private enum SelectedState {
		CATEGORY,
		DIFFICULTY,
		NONE
	}
	private SelectedState selectedState = SelectedState.NONE;

	void Start(){
		categoryWheel = transform.Find ("CategoryWheel").gameObject;
		difficultyWheel = transform.Find ("DifficultyWheel").gameObject;
	}

	public void ResetState(){
		selectedState = SelectedState.NONE;
	}

	public void ChooseCategory(){
		if (selectedState == SelectedState.NONE) {
			categoryWheel.SetActive (true);
			//categoryWheel.GetComponent<SelectionWheel> ().SetSelected (GameManager.instance.client.pb.GetRoomCategory());
			selectedState = SelectedState.CATEGORY;
		}
		else if (selectedState == SelectedState.CATEGORY) {
			if (!categoryWheel.GetComponent<SelectionWheel> ().GetSelected ().Equals (GameManager.instance.client.pb.GetRoomCategory())) {
				GameManager.instance.client.pb.SetCategory (categoryWheel.GetComponent<SelectionWheel>().GetSelected());
			}
				
			categoryWheel.SetActive (false);
			selectedState = SelectedState.NONE;
		}
	}

	public void ChooseDifficulty(){
		if (selectedState == SelectedState.NONE) {
			difficultyWheel.SetActive (true);
			difficultyWheel.GetComponent<SelectionWheel> ().SetSelected (GameManager.instance.client.pb.GetDifficulty());
			selectedState = SelectedState.DIFFICULTY;
		}
		else if (selectedState == SelectedState.DIFFICULTY) {
			if (!difficultyWheel.GetComponent<SelectionWheel> ().GetSelected ().Equals (GameManager.instance.client.pb.GetDifficulty())) {
				GameManager.instance.client.pb.SetDifficulty (difficultyWheel.GetComponent<SelectionWheel>().GetSelected());
			}
				
			difficultyWheel.SetActive (false);
			selectedState = SelectedState.NONE;
		}
	}
}
