using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionList : MonoBehaviour {
	/// <summary>
	/// Handles selectable options
	/// </summary>

	private GameObject categoryWheel;
	private GameObject difficultyWheel;
	private GameObject profileOptions;

	private enum SelectedState {
		PROFILE,
		CATEGORY,
		DIFFICULTY,
		NONE
	}
	private SelectedState selectedState = SelectedState.NONE;

	void Start(){
		categoryWheel = transform.Find ("CategoryWheel").gameObject;
		difficultyWheel = transform.Find ("DifficultyWheel").gameObject;
		profileOptions = transform.Find ("ProfileOptions").gameObject;
	}

	public void ResetState(){
		/// <summary>
		/// Reset option menu state
		/// </summary>

		selectedState = SelectedState.NONE;
	}

	public void ChooseProfile(){
		/// <summary>
		/// Toggle profile option menu
		/// </summary>

		if (selectedState == SelectedState.NONE) {
			profileOptions.SetActive (true);
			selectedState = SelectedState.PROFILE;
		}
		else if (selectedState == SelectedState.PROFILE) {
			profileOptions.SetActive (false);
			selectedState = SelectedState.NONE;
		}
	}

	public void ChooseCategory(){
		/// <summary>
		/// Toggle category options
		/// </summary>

		if (selectedState == SelectedState.NONE) {
			categoryWheel.SetActive (true);
			categoryWheel.GetComponent<SelectionWheel> ().SetSelected (GameManager.instance.client.pb.GetRoomCategory());
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
		/// <summary>
		/// Toggle difficulty options
		/// </summary>

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

	public void ChooseLeave(){
		/// <summary>
		/// Leave room
		/// </summary>
		
		GameManager.instance.client.pb.Disconnect ();

		// send player back to hub here!!!!!!!!!!!!
	}
}
