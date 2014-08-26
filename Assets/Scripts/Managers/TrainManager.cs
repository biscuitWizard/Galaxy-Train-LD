using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainManager : SingletonMonobehaviour {
	public int CartCount = 2;
	public float CartMargin = 0.375f;
	public GameObject PassengerCart;
	public GameObject CoalCart;
	public GameObject Caboose;
	private readonly List<GameObject> _carts = new List<GameObject>();

	// Use this for initialization
	public override void Load() {
		Messenger<int>.AddListener ("addPassengerCart", OnAddPassengerCart);
		Messenger<GameObject>.AddListener ("bonusBrakes", OnBonusBrakes);
		Messenger<GameObject>.AddListener ("bonusTurning", OnBonusTurningJets);
	}

	void Start () {
		// Initialize carts
		for(var x = 0; x < CartCount;x++) {
			AddCart ();
		}
	}

	public override void Unload() {
		Messenger<int>.RemoveListener ("addPassengerCart", OnAddPassengerCart);
		Messenger<GameObject>.RemoveListener ("bonusBrakes", OnBonusBrakes);
		Messenger<GameObject>.RemoveListener ("bonusTurning", OnBonusTurningJets);
		foreach (var cart in _carts) {
			Destroy (cart);
		}
	}

	public void AddCart(int position = 0) {
		position = position == 0 ? _carts.Count : position;
		var previousCart = _carts.Count == 0 
			? gameObject
			: _carts [position - 1];
		var cart = (GameObject)Instantiate(PassengerCart);
		cart.transform.name = "Passenger Cart";
		cart.transform.position = new Vector3(transform.position.x, transform.position.y - (CartMargin * position) - 1,  0);
		
		var joint = cart.GetComponent<HingeJoint2D>();
		joint.connectedAnchor = previousCart == gameObject	
			? new Vector2(0, -0.70f) // was -0.64f 
				: new Vector2(0, -0.55f);
		joint.anchor = new Vector2(0, 0.55f);
		joint.connectedBody = previousCart.rigidbody2D;

		_carts.Insert (position, cart);
		if (Messenger<int>.HasListener ("passengerCartAdded"))
			Messenger<int>.Broadcast ("passengerCartAdded", _carts.Count);
	}

	protected virtual void OnAddPassengerCart(int planetsVisited) {
		AddCart ();
	}

	protected virtual void OnBonusBrakes(GameObject sender) {
		var controller = GetComponent<TrainController> ();
		controller.BrakeThrust += 1;
	}

	protected virtual void OnBonusTurningJets(GameObject sender) {
		var controller = GetComponent<TrainController> ();
		controller.VectorThrust += 25;
	}

	protected override void OnLevelChanged(int level) {
		base.OnLevelChanging (level);

		gameObject.SetActive (false);
		foreach (var cart in _carts)
			cart.SetActive (false);
		transform.position = new Vector3 (0, -15, 0);
		transform.eulerAngles = Vector3.zero;
		for (var i=0; i < _carts.Count;i++) {
			_carts[i].transform.position = new Vector3(transform.position.x, transform.position.y - (CartMargin * i) - 1,  0);
		}

		if ((Levels)level == Levels.SolarSystem) {
			gameObject.SetActive (true);
			foreach (var cart in _carts)
				cart.SetActive (true);
		}
	}
}
