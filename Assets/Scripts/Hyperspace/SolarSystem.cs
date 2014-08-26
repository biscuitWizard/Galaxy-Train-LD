using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystem : MonoBehaviour {

	private bool _triggered;

	void OnLevelWasLoaded(int level) {
		if((Levels)level == Levels.SolarSystem) {
			_triggered = false;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if(_triggered)
			return;
		if (collider.tag != "Player")
			return;
		if(Messenger<int>.HasListener("loadLevel"))
			Messenger<int>.Broadcast("loadLevel", (int)Levels.HyperspaceMap);
		_triggered = true;
	}
}
