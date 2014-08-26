using UnityEngine;
using System.Collections;

public class PassengersTakenDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "Passengers Taken: {0}";
	private int _totalPassengers = 0;

	// Use this for initialization
	void Awake () {
		Messenger<int>.AddListener ("passengersAdded", OnPassengersAdded);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, _totalPassengers);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("passengersAdded", OnPassengersAdded);
	}

	protected virtual void OnPassengersAdded(int passengersAdded) {
		_totalPassengers += passengersAdded;
	}
}
