using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Speech recognizer for Windows
/// <summary>
public class VoiceInput : MonoBehaviour {
	
	DictationRecognizer recognizer;

	void Start () {
		recognizer = new DictationRecognizer ();
		recognizer.DictationResult += (text, confidence) => {
			if(!"".Equals(text)){
				GameManager.instance.client.pb.Guess(text, true);
			}
			TurnOff();
		};
	}

	/// <summary>
	/// Open for input
	/// </summary>
	public void TurnOn(){
		if (recognizer.Status == SpeechSystemStatus.Stopped) {
			recognizer.Start ();
		}
	}

	/// <summary>
	/// Close out any further input
	/// </summary>
	public void TurnOff(){
		if (recognizer.Status == SpeechSystemStatus.Running) {
			recognizer.Stop ();
		}
	}

	void OnDestroy(){
		recognizer.Dispose ();
	}
}
