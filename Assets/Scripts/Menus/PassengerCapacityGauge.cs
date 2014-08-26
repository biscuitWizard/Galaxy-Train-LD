using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PassengerCapacityGauge : MonoBehaviour {

	public GameObject[] PassengerIcons;
	public GameObject CapacityText;
	public string StringFormat = "{0} / {1} Passengers";
	private int _passengers = 0;
	private int _passengerCapacity = 0;

	// Use this for initialization
	void Awake() {
		Messenger<int>.AddListener ("passengersAdded", OnPassengersAdded);
		Messenger<int>.AddListener ("passengersRemoved", OnPassengersRemoved);
		Messenger<int>.AddListener ("printPassengerTotal", OnPrintPassengerTotal);
		Messenger<int>.AddListener ("printPassengerCapacity", OnPrintPassengerCapacity);
		Messenger<int>.AddListener ("passengerCapacityAdded", OnPassengerCapacityAdded);
	}

	void Start () {
		Messenger<GameObject>.Broadcast ("requestPassengerTotal", gameObject);
		Messenger<GameObject>.Broadcast ("requestPassengerCapacity", gameObject);
		RedrawGauge ();
	}
	
	void OnDestroy() {
		Messenger<int>.RemoveListener ("passengersAdded", OnPassengersAdded);
		Messenger<int>.RemoveListener ("passengersRemoved", OnPassengersRemoved);
		Messenger<int>.RemoveListener ("printPassengerTotal", OnPrintPassengerTotal);
		Messenger<int>.RemoveListener ("printPassengerCapacity", OnPrintPassengerCapacity);
		Messenger<int>.RemoveListener ("passengerCapacityAdded", OnPassengerCapacityAdded);
	}

	public void RedrawGauge() {
		var percentage = _passengerCapacity != 0
			? Math.Round(Math.Round ((float)_passengers / (float)_passengerCapacity, 1) / 2f * 10)
			: 0;

		// Recolor icons.
		for (var i=0; i < PassengerIcons.Length; i++) {
			PassengerIcons[i].guiTexture.color = i < percentage ? Color.green : Color.grey;
		}

		// Update capacity text
		CapacityText.guiText.text = string.Format (StringFormat, _passengers, _passengerCapacity);

	}

	protected virtual void OnPassengerCapacityAdded(int amount) {
		_passengerCapacity += amount;
		RedrawGauge ();
	}

	protected virtual void OnPassengersAdded(int amount) {
		_passengers += amount;
		RedrawGauge ();
	}

	protected virtual void OnPassengersRemoved(int amount) {
		_passengers -= amount;
		RedrawGauge ();
	}

	protected virtual void OnPrintPassengerTotal(int total) {
		_passengers = total;
		RedrawGauge ();
	}

	protected virtual void OnPrintPassengerCapacity(int capacity) {
		_passengerCapacity = capacity;
		RedrawGauge ();
	}
}
