using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserList : MonoBehaviour {
	// Keeps a list of users in the room

	public GameObject userWidgetTemplate;

	private List<GameObject> userWidgets;


	void Start(){
		userWidgets = new List<GameObject> ();
	}

	void Update(){
		UpdateUsers ();
	}

	void UpdateUsers(){

		// empty old
		foreach (GameObject userWidget in userWidgets) {
			Destroy (userWidget);
		}
		userWidgets.Clear ();

		// re-add new users
		int count = 0;
		foreach(User user in GameManager.instance.client.pb.users.Values){
			GameObject userWidget = Instantiate(userWidgetTemplate, transform);
			userWidget.transform.Find ("UserName").GetComponent<Text>().text = user.name;
			userWidget.transform.Find ("UserScore").GetComponent<Text>().text = user.score+"";
			userWidget.transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -110 * count);

			userWidgets.Add (userWidget);

			count++;
		}
		
	}
}
