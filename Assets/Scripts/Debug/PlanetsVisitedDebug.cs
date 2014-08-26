using UnityEngine;
using System.Collections;

public class PlanetsVisitedDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "Planets Visited: {0}";
	private int _planetsVisited = 0;

	// Use this for initialization
	void Awake () {
		Messenger<Planet>.AddListener ("planetVisited", OnPlanetVisited);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, _planetsVisited);
	}

	protected virtual void OnPlanetVisited(Planet planet) {
		_planetsVisited++;
	}
}
