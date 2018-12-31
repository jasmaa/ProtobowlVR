using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceInput : MonoBehaviour {
	/// <summary>
	/// Speech recognizer for Windows
	/// <summary>
	
	DictationRecognizer recognizer;

	void Start () {
		recognizer = new DictationRecognizer ();
		recognizer.DictationResult += (text, confidence) => {
			if(!"".Equals(text)){
				GameManager.instance.client.pb.Guess(text, true);
				TurnOff();
			}
		};
	}

	public void TurnOn(){
		/// <summary>
		/// Open for input
		/// </summary>
		
		if (recognizer.Status == SpeechSystemStatus.Stopped) {
			recognizer.Start ();
		}
	}
	public void TurnOff(){
		/// <summary>
		/// Close out any further input
		/// </summary>
		
		if (recognizer.Status == SpeechSystemStatus.Running) {
			recognizer.Stop ();
		}
	}

	void OnDestroy(){
		recognizer.Dispose ();
	}
}
