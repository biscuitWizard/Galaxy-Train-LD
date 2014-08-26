using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class CountdownTimer : SingletonMonobehaviour {

	public string StringFormat = "{0}:{1} Remaining";
	private TimeSpan _timeRemaining = new TimeSpan(0, 0, 0);
	private bool _gamePaused;

	// Use this for initialization
	public override void Load() {
		Messenger<TimeSpan>.AddListener ("timeRemainingUpdate", OnTimeRemainingUpdate);
		Messenger<GameObject>.AddListener ("gameStarted", OnGameStarted);
		Messenger<GameObject>.AddListener ("gamePaused", OnGamePaused);
		Messenger<GameObject>.AddListener ("gameResumed", OnGameUnpaused);
	}

	public override void Unload() {
		Messenger<TimeSpan>.RemoveListener ("timeRemainingUpdate", OnTimeRemainingUpdate);
		Messenger<GameObject>.RemoveListener ("gameStarted", OnGameStarted);
		Messenger<GameObject>.RemoveListener ("gamePaused", OnGamePaused);
		Messenger<GameObject>.RemoveListener ("gameResumed", OnGameUnpaused);
	}

	void Start() {
		InvokeRepeating ("Flash", 0f, 0.5f);
	}

	void Flash() {
		if(_gamePaused)
			gameObject.SetActive (!gameObject.activeSelf);
		else
			gameObject.SetActive(true);
	}

	protected virtual void OnTimeRemainingUpdate(TimeSpan timeRemaining) {
		_timeRemaining = timeRemaining;
		guiText.text = string.Format (StringFormat,
		                              _timeRemaining.Minutes.ToString ("0"),
		                              _timeRemaining.Seconds.ToString ("00"));

		if (_timeRemaining.TotalSeconds == 10)
			Messenger<GameObject>.Broadcast ("timeRunningOut", gameObject);
	}

	protected virtual void OnGameStarted(GameObject sender) {
		gameObject.SetActive (true);
		_gamePaused = true;
	}

	protected virtual void OnGamePaused(GameObject sender) {
		_gamePaused = true;
	}

	protected virtual void OnGameUnpaused(GameObject sender) {
		_gamePaused = false;
		gameObject.SetActive (true);
	}
}
