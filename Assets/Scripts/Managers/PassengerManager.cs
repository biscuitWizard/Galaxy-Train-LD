using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PassengerManager : MonoBehaviour {

	public int MinPlanetPassengers = 5;
	public int MaxPlanetPassengers = 40;
	public int MaxPassengersPerCart = 20;
	public int AddCartThreshold = 5;
	public float PlanetPassengerRefreshRate = 0.25f;
	public float PlanetPriorityAlertCheckRate = 0.10f;
	private int _planetsVisited = 0;
	private int _passengers;
	public int CurrentPassengerCapacity = 20;
	private bool _gamePaused;
	private readonly List<Planet> _planets = new List<Planet> ();
	private readonly List<Planet> _priorityPlanets = new List<Planet>();
	public string UniqueId = System.Guid.NewGuid ().ToString ();

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
		Messenger<Planet>.AddListener ("planetVisited", OnPlanetVisited);
		Messenger<Planet>.AddListener ("planetCreated", OnPlanetCreated);
		Messenger<Planet>.AddListener ("planetDestroyed", OnPlanetDestroyed);
		Messenger<GameObject>.AddListener ("gamePaused", OnGamePaused);
		Messenger<GameObject>.AddListener ("gameResumed", OnGameResumed);
		Messenger<GameObject>.AddListener ("requestPassengerTotal", OnRequestPassengerTotal);
		Messenger<GameObject>.AddListener ("requestPassengerCapacity", OnRequestPassengerCapacity);
	}

	void Start() {
		InvokeRepeating ("PassengerCheck", 0, 5);
		InvokeRepeating ("PriorityAlertCheck", 0, 5);
		Debug.Log ("PassengerManager loaded");
	}

	void OnDestroy() {
		Debug.Log ("Destroying (PassengerManager): " + UniqueId);
		Messenger<Planet>.RemoveListener ("planetVisited", OnPlanetVisited);
		Messenger<Planet>.RemoveListener ("planetCreated", OnPlanetCreated);
		Messenger<Planet>.RemoveListener ("planetDestroyed", OnPlanetDestroyed);
		Messenger<GameObject>.RemoveListener ("requestPassengerTotal", OnRequestPassengerTotal);
		Messenger<GameObject>.RemoveListener ("requestPassengerCapacity", OnRequestPassengerCapacity);
	}

	void PassengerCheck() {
		if(!_gamePaused) {
			foreach (var planet in _planets) {
				if(planet.GetPassengers() == 0
				   	&& !planet.GetPriority()
				   	&& Random.Range (0, 100) / 100f < PlanetPassengerRefreshRate) {
					planet.SetVisited(false);
					planet.SetPassengers(Random.Range (MinPlanetPassengers, MaxPlanetPassengers));
				}
			}
		}
	}

	void PriorityAlertCheck() {
		if(!_gamePaused) {
			foreach (var planet in _planets) {
				if(Random.Range (0, 100) / 100f < PlanetPriorityAlertCheckRate) {
					if(_priorityPlanets.Any(p => p.transform.parent.name == planet.transform.parent.name))
						continue;
					planet.SetPriority(true);
					planet.SetPassengers(0);
					planet.SetVisited(false);
					_priorityPlanets.Add (planet);
					Messenger<Planet>.Broadcast("priorityAlert", planet);
				}
			}
		}
	}

	protected virtual void OnGamePaused(GameObject sender) {
		_gamePaused = true;
	}

	protected virtual void OnGameResumed(GameObject sender) {
		_gamePaused = false;
	}

	protected virtual void OnRequestPassengerTotal(GameObject sender) {
		Messenger<int>.Broadcast ("printPassengerTotal", _passengers);
	}

	protected virtual void OnRequestPassengerCapacity(GameObject sender) {
		Messenger<int>.Broadcast ("printPassengerCapacity", CurrentPassengerCapacity);
		Debug.Log ("Broadcasted Cap: " + CurrentPassengerCapacity);
	}

	protected virtual void OnAssignPassengers(Planet planet) {
		planet.SetVisited(false);
		planet.SetPassengers(Random.Range (MinPlanetPassengers, MaxPlanetPassengers));
	}

	protected virtual void OnPlanetVisited(Planet planet) {
		_planetsVisited++;

		if(planet.GetPriority()) {
			Messenger<Planet>.Broadcast("priorityCompleted", planet);
			planet.SetPriority(false);
			_priorityPlanets.Remove (planet);
		}

		if(_planetsVisited % AddCartThreshold == 0) {
			// Add a cart.
			Messenger<int>.Broadcast ("addPassengerCart", _planetsVisited);
			Messenger<string>.Broadcast ("achievementEarned", "New cart unlocked!");
			Messenger<int>.Broadcast ("passengerCapacityAdded", MaxPassengersPerCart);
			CurrentPassengerCapacity += MaxPassengersPerCart;
			Debug.Log ("Current Cap: " + CurrentPassengerCapacity);
		}

		if (_passengers > 0) {
			var percentageDroppedOff = Random.Range (35, 65) / 100f;
			var passengersRemoved = Mathf.FloorToInt (_passengers * percentageDroppedOff);

			Messenger<int>.Broadcast("passengersRemoved", passengersRemoved);
			Messenger<string>.Broadcast("achievementEarned", string.Format("{0} Passengers Dropped Off", passengersRemoved));
			_passengers -= passengersRemoved;
		}

		if(_passengers <= CurrentPassengerCapacity) {
			var passengersAdded = Mathf.RoundToInt(Random.Range (MinPlanetPassengers, MaxPlanetPassengers) 
				* (planet.transform.localScale.x * planet.transform.parent.localScale.x));
			passengersAdded = Mathf.Clamp(passengersAdded, 0, CurrentPassengerCapacity - _passengers);
			_passengers += passengersAdded;
			if(_planetsVisited % AddCartThreshold != 0) 
				Messenger<string>.Broadcast ("achievementEarned",
				                             string.Format ("{0} Passengers Picked Up", passengersAdded));
			Messenger<int>.Broadcast ("passengersAdded", passengersAdded);
		}
	}

	protected virtual void OnPlanetCreated(Planet planet) {
		_planets.Add (planet);
		planet.SetVisited(false);
		planet.SetPassengers(Random.Range (MinPlanetPassengers, MaxPlanetPassengers));
	}

	protected virtual void OnPlanetDestroyed(Planet planet) {
		_planets.Remove (planet);
	}
}
