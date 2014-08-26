using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour {

	public int SceneToLoad = 0;
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			// Don't respond to game control keys.
			if(Input.GetAxis ("Vertical") != 0
			   || Input.GetAxis ("Horizontal") != 0
			   || Input.GetKeyDown(KeyCode.Space))
				return;

			foreach (var g in GameObject.FindObjectsOfType<GameObject>())
				Destroy (g);
			
			Messenger<int>.Broadcast ("loadLevel", SceneToLoad);
		}
	}
}
