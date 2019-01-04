using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Keeps a list of users and scores in the room
/// </summary>
public class UserList : MonoBehaviour {

	public GameObject userWidgetTemplate;

	private List<GameObject> userWidgets;

	void Start(){
		userWidgets = new List<GameObject> ();
	}

	void Update(){
		UpdateUsers ();
	}

	/// <summary>
	/// Rebuild updated user list
	/// </summary>
	void UpdateUsers(){
	
		// empty old
		foreach (GameObject userWidget in userWidgets) {
			Destroy (userWidget);
		}
		userWidgets.Clear ();

		// re-add new users
		int count = 0;
		List<User> sortedUsers = GameManager.instance.client.pb.users.Values.ToList();
		sortedUsers.Sort ((User a, User b) => b.score - a.score);
		sortedUsers = sortedUsers.Take (3).ToList();
		foreach(User user in sortedUsers){
			GameObject userWidget = Instantiate(userWidgetTemplate, transform);
			userWidget.transform.Find ("UserName").GetComponent<Text>().text = user.name;
			userWidget.transform.Find ("UserScore").GetComponent<Text>().text = user.score+"";
			userWidget.transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -110 * count);

			userWidgets.Add (userWidget);

			count++;
		}
		
	}
}
