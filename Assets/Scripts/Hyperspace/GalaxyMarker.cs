using UnityEngine;
using System.Collections;


public class GalaxyMarker : MonoBehaviour {

	public float HoverSizeIncrease = .2f;
	public GameObject PriorityIcon;
	public GameObject GalaxyNameObject;
	private Vector3 _originalScale;
	private bool _priorityGalaxy;

	// Use this for initialization
	void Start () {
		_originalScale = transform.localScale;
		PriorityIcon.SetActive (_priorityGalaxy);
	}

	void OnMouseEnter() {
		transform.localScale += transform.localScale * HoverSizeIncrease;
	}

	void OnMouseExit() {
		transform.localScale = _originalScale;
	}

	void OnMouseDown() {
		Messenger<int>.Broadcast ("loadLevel", (int)Levels.SolarSystem);
		if (Messenger<GalaxyMarker>.HasListener ("loadSolarSystem"))
			Messenger<GalaxyMarker>.Broadcast ("loadSolarSystem", this);
	}

	public void SetGalaxyName(string name) {
		GalaxyNameObject.GetComponent<TextMesh> ().text = name;
		this.name = name;
	}

	public void SetPriority(bool priority) {
		_priorityGalaxy = priority;
		PriorityIcon.SetActive (_priorityGalaxy);
	}
}
