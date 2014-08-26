using UnityEngine;
using System.Collections;

public class PassengersDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "Passengers: {0}";
	private int _passengers;

	// Use this for initialization
	void Awake () {
		Messenger<int>.AddListener ("passengersAdded", OnPassengersAdded);
		Messenger<int>.AddListener ("passengersRemoved", OnPassengersRemoved);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, _passengers);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("passengersAdded", OnPassengersAdded);
		Messenger<int>.RemoveListener ("passengersRemoved", OnPassengersRemoved);
	}

	protected virtual void OnPassengersAdded(int passengersAdded) {
		_passengers += passengersAdded;
	}

	protected virtual void OnPassengersRemoved(int passengersRemoved) {
		_passengers -= passengersRemoved;
	}
}
