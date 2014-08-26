using UnityEngine;
using System.Collections;

public class NewGameButton : SpriteButtonMonoBehaviour {

	public int SceneToLoad;

	new void OnMouseDown() {
		base.OnMouseDown ();

		Messenger<int>.Broadcast ("loadLevel", SceneToLoad);
	}
}
