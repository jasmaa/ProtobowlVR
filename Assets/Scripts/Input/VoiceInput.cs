using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceInput : MonoBehaviour {
	// Speech recognizer for Windows

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
		if (recognizer.Status == SpeechSystemStatus.Stopped) {
			recognizer.Start ();
		}
	}
	public void TurnOff(){
		if (recognizer.Status == SpeechSystemStatus.Running) {
			recognizer.Stop ();
		}
	}

	void OnDestroy(){
		recognizer.Dispose ();
	}
}
