using System;
using UnityEngine;
using System.Collections;

public class TimeRemainingDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "{0}:{1} Remaining";
	private TimeSpan _timeRemaining = new TimeSpan(0, 0, 0);

	// Use this for initialization
	void Start () {
		Messenger<bool>.Broadcast ("requestTimeRemaining", true);
	}

	void Awake() {
		Messenger<TimeSpan>.AddListener ("timeRemainingUpdate", OnTimeRemainingUpdate);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat,
			_timeRemaining.Minutes.ToString ("0"),
		    _timeRemaining.Seconds.ToString ("00"));
	}

	void OnDestroy() {
		Messenger<TimeSpan>.RemoveListener ("timeRemainingUpdate", OnTimeRemainingUpdate);
	}

	protected virtual void OnTimeRemainingUpdate(TimeSpan timeRemaining) {
		_timeRemaining = timeRemaining;
	}
}
