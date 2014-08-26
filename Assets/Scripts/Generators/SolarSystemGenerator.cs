using UnityEngine;
using System.Collections;
using System.Linq;

public class SolarSystemGenerator : MonoBehaviour {
	public float MinSunSize = 0.05f;
	public float MaxSunSize = 0.05f;
	public int MaxPlanetsPerSystem = 12;
	public int MinPlanetsPerSystem = 5;
	public float PlanetOrbitMargin = .005f; 
	public GameObject SolarSystemPrefab;
	public GameObject[] StarPrefabs;
	public Vector2 MaxSolarSystemSize = new Vector2(20,20);
	private GameObject _solarSystemFolder;

	void Awake() {
		_solarSystemFolder = new GameObject ("Solar Systems");
		_solarSystemFolder.transform.localScale = Vector3.one;
		_solarSystemFolder.transform.position = Vector3.zero;
		DontDestroyOnLoad (_solarSystemFolder);
	}

	void OnDestroy() {
		Destroy (_solarSystemFolder);
	}

	public GameObject GenerateSolarSystem(string name) {
		var planetGenerator = GetComponent<PlanetGenerator> ();
		var solarSystemRoot = (GameObject)Instantiate (SolarSystemPrefab);
		solarSystemRoot.transform.localScale = MaxSolarSystemSize;
		solarSystemRoot.transform.position = Vector3.zero;
		solarSystemRoot.transform.parent = _solarSystemFolder.transform;
		solarSystemRoot.name = name;

		// Create a sun
		var sun = (GameObject)Instantiate (StarPrefabs.OrderBy (_ => System.Guid.NewGuid ()).First ());
		var sunSize = Random.Range (MinSunSize, MaxSunSize);

		// Set parameters
		sun.transform.parent = solarSystemRoot.transform;
		sun.transform.localScale = new Vector3 (sunSize, sunSize, 1f);
		sun.transform.localPosition = Vector3.zero;

		// Create some planets
		var planetCount = Random.Range (MinPlanetsPerSystem, MaxPlanetsPerSystem);
		var minOrbitDistance = (sun.transform.localScale.y * 3) + PlanetOrbitMargin;
		for (var i=0; i < planetCount; i++) {
			if(minOrbitDistance * solarSystemRoot.transform.localScale.y > MaxSolarSystemSize.y / 2)
				break;
			var planet = planetGenerator.GeneratePlanet(solarSystemRoot, sun, minOrbitDistance);
			Messenger<Planet>.Broadcast ("planetCreated", planet.GetComponent<Planet>());

			minOrbitDistance = PlanetOrbitMargin + Vector3.Distance(planet.transform.localPosition, sun.transform.localPosition);
		}

		return solarSystemRoot;
	}
}
