using UnityEngine;
using System.Collections;

public class VelocityDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "Velocity: {0},{1}";
	private Vector2 _velocity = Vector2.zero;

	// Use this for initialization
	void Awake () {
		Messenger<Vector2>.AddListener ("velocityUpdate", OnVelocityUpdate);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, 
		                              _velocity.x.ToString("0.00"), 
		                              _velocity.y.ToString ("0.00"));
	}

	void OnDestroy() {
		Messenger<Vector2>.RemoveListener ("velocityUpdate", OnVelocityUpdate);
	}

	protected virtual void OnVelocityUpdate(Vector2 newVelocity) {
		_velocity = newVelocity;
	}
}
