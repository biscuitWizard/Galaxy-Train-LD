using UnityEngine;
using System.Collections;

// Put this on a rigidbody object and instantly
// have 2D spaceship controls like OverWhelmed Arena
// that you can tweak to your heart's content.

[RequireComponent (typeof (Rigidbody2D))]
public class TrainController : MonoBehaviour
{
	public bool CanPlayerControl = true;
	public bool IsActive = true;
	public float ForwardThrust = 100;
	public float BackwardThrust = 50;
	public float VectorThrust = 75;
	public float BrakeThrust = 5;
	public float Drag = 0.1f;
	public float AngularDrag = 0.1f;

	void Awake() {
		gameObject.SetActive (IsActive);
	}


	bool _braking;
	float _thrust;
	float _turn;
	void FixedUpdate() {
		if(_braking) {
			rigidbody2D.drag = BrakeThrust;
			rigidbody2D.angularDrag = BrakeThrust;
		} else {
			rigidbody2D.drag = Drag;
			rigidbody2D.angularDrag = AngularDrag;
			rigidbody2D.AddRelativeForce(Vector2.up * _thrust * Time.deltaTime);
		}

		rigidbody2D.AddTorque(_turn * Time.deltaTime);
	}

	void Update() {
		if(CanPlayerControl) {
			var thrust = Input.GetAxis ("Vertical");
			var turn = Input.GetAxis ("Horizontal") * VectorThrust * -1;

			if(thrust > 0) {
				thrust *= ForwardThrust;
			} else if (thrust < 0) {
				thrust *= BackwardThrust;
			}

			_braking = Input.GetKey(KeyCode.Space);

			_thrust = thrust;
			_turn = turn;
		}
	}
}
