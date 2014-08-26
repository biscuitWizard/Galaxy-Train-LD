using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
	public Transform Target;
	public float distance = 3.0f;
	public float height = 3.0f;
	public float damping = 5.0f;
	public bool smoothRotation = true;
	public float rotationDamping = 10.0f;
	public bool Following = false;

	void Update() {
		if (Following) {
			var newX = Mathf.Lerp (transform.position.x, Target.position.x, Time.deltaTime * damping);
			var newY = Mathf.Lerp (transform.position.y, Target.position.y, Time.deltaTime * damping);
			transform.position = new Vector3 (newX, newY, transform.position.z);


			if (smoothRotation) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Target.rotation, Time.deltaTime * rotationDamping);
			}
		}
	}

	public GameObject TryFindTarget() {
		var player = GameObject.FindGameObjectWithTag ("Player");
		if (player == null)
			return null;
		
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		return player;
	}

}