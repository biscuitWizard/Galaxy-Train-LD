using UnityEngine;
using System.Collections;

public class Galaxy : MonoBehaviour {

	public string Name;
	
	void OnTriggerEnter2D(Collider2D col) {
		Messenger<string>.Broadcast("galaxyVisited", Name);
	}
}
