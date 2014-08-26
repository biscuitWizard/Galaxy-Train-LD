using UnityEngine;
using System.Collections;

public class SceneManager : SingletonMonobehaviour {

	// Use this for initialization
	public override void Load() {
		Messenger<int>.AddListener ("loadLevel", OnLoadScene);
	}

	public override void Unload() {
		Messenger<int>.RemoveListener ("loadLevel", OnLoadScene);
	}

	public void OnLoadScene(int level) {
		if (Messenger<int>.HasListener ("levelChanging")) {
			Messenger<int>.Broadcast ("levelChanging", level);
		}

		 Application.LoadLevel(level);

		if (Messenger<int>.HasListener ("levelChanged")) {
			Messenger<int>.Broadcast ("levelChanged", level);
		}
	}
}
