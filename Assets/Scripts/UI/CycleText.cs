using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cycles through text to display
/// </summary>
public class CycleText : MonoBehaviour {

	private string[] textList = {
		"Welcome to Protobowl VR!",
		"Enter a name and room and play quizbowl in VR",
		"=== CREDITS ===",
		"CODE\n\nOculus Unity Integration\nWebSocketSharp\nSimpleJSON",
		"SOUND\n\nTim Mortimer Sci-Fi UI\nIOS Keyboard Sounds\nNenadSimic Menu Click",
		"GRAPHICAL\n\nFantasy Skybox\nStylized Vegetation Pack\nVarious textures...",
		"FONT\n\nDSEG Font\nFontAwesome",
		"Thank You!\nHave a nice day!"
	};

	private Text cycleText;
	private int letIdx = 0;
	private int wordIdx = 0;

	private float updateCooldown = 0.1f;
	private float waitCooldown = 3f;

	void Start(){
		cycleText = GetComponent<Text> ();
	}

	void Update () {

		if (letIdx < textList [wordIdx].Length){
			if (updateCooldown == 0) {
				letIdx++;
				cycleText.text = textList [wordIdx].Substring (0, letIdx);
				updateCooldown = 0.1f;
			}
			updateCooldown = Mathf.Clamp(updateCooldown - Time.deltaTime, 0, 9999);
		}
		else{
			if (waitCooldown == 0) {
				wordIdx = (wordIdx + 1) % textList.Length;
				letIdx = 0;
				waitCooldown = 3f;
			}
			waitCooldown = Mathf.Clamp(waitCooldown - Time.deltaTime, 0, 9999);
		}
	}
}
