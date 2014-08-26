using UnityEngine;
using System.Collections;

public class PassengersOffloadedDebug : MonoBehaviour {

	public string StringFormat = "Passengers Offloaded: {0}";
	private int _passengersRemoved;

	// Use this for initialization
	void Awake () {
		Messenger<int>.AddListener ("passengersRemoved", OnPassengersRemoved);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, _passengersRemoved);
	}

	protected virtual void OnPassengersRemoved(int passengersRemoved) {
		_passengersRemoved += passengersRemoved;
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("passengersRemoved", OnPassengersRemoved);
	}
}
