using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {

	private GameManager GameManager;
	private int _totalObjectsLoading;
	private int _objectsLoading;

	// Use this for initialization
	void Awake () {
		Messenger<GameObject>.AddListener ("startedLoading", OnStartedLoading);
		Messenger<GameObject>.AddListener("finishedLoading", OnFinishedLoading);
	}

	void Update() {
		if (_objectsLoading == 0
		    && _totalObjectsLoading > 0) {
			Messenger<int>.Broadcast("loadLevel", (int)Levels.HyperspaceMap);
			GameManager.StartGame();
		}
	}

	void Start() {
		Messenger<bool>.Broadcast ("startLoading", true);
	}

	void OnDestroy() {
		Messenger<GameObject>.RemoveListener ("startedLoading", OnStartedLoading);
		Messenger<GameObject>.RemoveListener ("finishedLoading", OnFinishedLoading);
	}

	protected virtual void OnStartedLoading(GameObject sender) {
		_totalObjectsLoading++;
		_objectsLoading++;
	}

	protected virtual void OnFinishedLoading(GameObject sender) {
		_objectsLoading--;

		if (sender.name == "Game Manager")
			GameManager = sender.GetComponent<GameManager>();
	}
}
