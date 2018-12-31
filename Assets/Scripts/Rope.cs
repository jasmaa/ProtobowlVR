using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {
	/// <summary>
	/// Physics rope with static src and movable target
	/// </summary>

	public LineRenderer lineRenderer;
	private List<GameObject> joints;

	public int numJoints = 10;
	public GameObject jointObj;
	public GameObject endJoint;


	public GameObject src;
	public GameObject target;

	void Start(){
		// init joints
		joints = new List<GameObject> ();
		for (int i = 0; i < numJoints; i++) {
			joints.Add (Instantiate(jointObj, Vector3.Lerp(src.transform.position, target.transform.position, (float)i / numJoints), Quaternion.identity));
			joints [i].transform.parent = transform;
		}
		for(int i = 0; i < numJoints - 1; i++){
			joints [i].GetComponent<CharacterJoint> ().connectedBody = joints [i + 1].GetComponent<Rigidbody>();
		}

		endJoint.GetComponent<CharacterJoint> ().connectedBody = joints [0].GetComponent<Rigidbody>();

		// init line renderer
		lineRenderer.positionCount = numJoints;
	}

	void Update(){
		lineRenderer.SetPosition (0, src.transform.position);
		for(int i = 0; i < numJoints - 1; i++){
			lineRenderer.SetPosition (i + 1, joints [i].transform.position);
		}
	}
}
