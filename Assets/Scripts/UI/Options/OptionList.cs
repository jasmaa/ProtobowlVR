using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles selectable options
/// </summary>
public class OptionList : MonoBehaviour {

	private GameObject categoryWheel;
	private GameObject difficultyWheel;
	private GameObject profileOptions;

	private AudioSource audioSrc;

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

		audioSrc = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Reset option menu state
	/// </summary>
	public void ResetState(){
		selectedState = SelectedState.NONE;
	}

	/// <summary>
	/// Toggle profile option menu
	/// </summary>
	public void ChooseProfile(){

		audioSrc.Play ();

		if (selectedState == SelectedState.NONE) {
			profileOptions.SetActive (true);
			selectedState = SelectedState.PROFILE;
		}
		else if (selectedState == SelectedState.PROFILE) {
			profileOptions.SetActive (false);
			selectedState = SelectedState.NONE;
		}
	}

	/// <summary>
	/// Toggle category options
	/// </summary>
	public void ChooseCategory(){

		audioSrc.Play ();

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

	/// <summary>
	/// Toggle difficulty options
	/// </summary>
	public void ChooseDifficulty(){

		audioSrc.Play ();

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

	/// <summary>
	/// Leave room and send player back to hub
	/// </summary>
	public void ChooseLeave(){

		audioSrc.Play ();

		GameManager.instance.client.pb.Disconnect ();
		SceneManager.LoadScene("HubVR");
	}
}
