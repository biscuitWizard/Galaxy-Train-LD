using UnityEngine;
using System.Collections;

public class QuitGameButton : SpriteButtonMonoBehaviour {
	new void OnMouseDown() {
		base.OnMouseDown ();
		Application.Quit ();
	}
}
