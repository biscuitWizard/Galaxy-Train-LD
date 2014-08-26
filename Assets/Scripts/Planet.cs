using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
	public Transform OrbitPoint;
	public float OrbitSpeed = 20;
	public float OrbitRadius;
	public GameObject PriorityIcon;
	public GameObject PassengerIcon;
	private bool _visited = false;
	private Vector3 _desiredPosition;
	private int _passengers = 0;
	private bool _priorityPlanet;

	// Use this for initialization
	void Start () {
		if (!gameObject.HasComponent<CircleCollider2D> ())
			gameObject.AddComponent<CircleCollider2D> ();

		gameObject.GetComponent<CircleCollider2D> ().isTrigger = true;

		PriorityIcon.SetActive(_priorityPlanet);
		PassengerIcon.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (_passengers > 0) {
			PassengerIcon.SetActive(true);
		} else if (_passengers == 0){
			PassengerIcon.SetActive (false);
		}

		if (OrbitPoint == null)
			return;
		//transform.RotateAround (OrbitPoint.position, Vector3.forward, OrbitSpeed * Time.deltaTime);
	}

	void OnDestroy() {
		if(Messenger<Planet>.HasListener("planetDestroyed"))
			Messenger<Planet>.Broadcast ("planetDestroyed", this);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (_visited || col.tag != "Player")
			return;
		SetVisited (true);
		Messenger<Planet>.Broadcast("planetVisited", this);
	}

	public void SetVisited(bool visited) {
		_visited = visited;
		if (_visited)
			SetPassengers (0);
	}

	public void SetPriority(bool priority) {
		_priorityPlanet = priority;
		PriorityIcon.SetActive(_priorityPlanet);
	}

	public bool GetPriority() {

		return _priorityPlanet;
	}

	public void SetPassengers(int passengers) {
		_passengers = passengers;
	}

	public int GetPassengers() {
		return _passengers;
	}
}
