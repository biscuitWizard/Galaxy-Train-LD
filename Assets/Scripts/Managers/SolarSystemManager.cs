using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystemManager : SingletonMonobehaviour {

	public bool GenerateOnStart = false;
	public bool GenerateInactiveSolarSystem = true;

	private Dictionary<string, GameObject> _solarSystems = new Dictionary<string, GameObject>();
	private GameObject _activeSolarSystem;

	public override void Load() {
		Messenger<GalaxyMarker>.AddListener ("loadSolarSystem", OnLoadSolarSystem);
		Messenger<GalaxyMarker>.AddListener ("generateSolarSystem", OnGenerateSolarSystem);
	}

	public override void Unload() {
		Messenger<GalaxyMarker>.RemoveListener ("loadSolarSystem", OnLoadSolarSystem);
		Messenger<GalaxyMarker>.RemoveListener ("generateSolarSystem", OnGenerateSolarSystem);
	}

	void Start() {
		if(GenerateOnStart)
			OnGenerateSolarSystem ();
	}

	protected override void OnLevelChanging(int level) {
		base.OnLevelChanging (level);
		
		switch ((Levels)level) {
		case Levels.GameOver:
			ClearSolarSystems();
			break;
		case Levels.HyperspaceMap:
			Debug.Log ("Unloading Solar System...");
			if (_activeSolarSystem != null) {
				_activeSolarSystem.SetActive (false);
				_activeSolarSystem = null;
			}
			break;
		}
	}

	public void ClearSolarSystems() {
		Debug.Log ("Clearing all solar systems...");
		foreach (var solarSystem in _solarSystems.Values) {
			Destroy(solarSystem);
		}

		_solarSystems.Clear ();
		_activeSolarSystem = null;
	}

	protected virtual void OnLoadSolarSystem(GalaxyMarker galaxyMarker) {
		Debug.Log ("Loading Single Solar System...");
		if (!_solarSystems.ContainsKey (galaxyMarker.name)) {
			var s = new System.Text.StringBuilder(string.Format ("Looking for {0} and couldn't find it in library of:\n", galaxyMarker.name));
			foreach(var galaxy in _solarSystems.Keys) {
				s.Append(", " + galaxy);
			}
			Debug.LogError (s.ToString());
		}

	 	_activeSolarSystem = _solarSystems [galaxyMarker.name];
		_activeSolarSystem.SetActive (true);
	}

	protected virtual void OnGenerateSolarSystem(GalaxyMarker marker = null) {
		Debug.Log ("Generating Solar System...");
		var solarSystemName = marker != null 
			? marker.name 
			: "Anomalous Galaxy";
		if (_solarSystems.ContainsKey (solarSystemName)) {
			Debug.Log ("lul nvm");
			return;
		}
		var generator = GetComponent<SolarSystemGenerator> ();
		var solarSystem = generator.GenerateSolarSystem (solarSystemName);
		solarSystem.SetActive (!GenerateInactiveSolarSystem);
		_solarSystems.Add (solarSystemName, solarSystem);
		Debug.Log ("Graph is now this long: " + _solarSystems.Count);
	}
}
