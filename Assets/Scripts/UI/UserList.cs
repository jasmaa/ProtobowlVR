using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserList : MonoBehaviour {

	public GameObject userWidgetTemplate;
	public GameObject widgetRoot;

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
			GameObject userWidget = Instantiate(userWidgetTemplate, widgetRoot.transform);
			userWidget.transform.Find ("UserName").GetComponent<Text>().text = user.name;
			userWidget.transform.Find ("UserScore").GetComponent<Text>().text = user.score+"";
			userWidget.transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0.3f - 0.11f * count);

			userWidgets.Add (userWidget);

			count++;
		}
		
	}
}
