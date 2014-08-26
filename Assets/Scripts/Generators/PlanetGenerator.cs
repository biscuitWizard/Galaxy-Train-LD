using UnityEngine;
using System.Linq;
using System.Collections;

public class PlanetGenerator : MonoBehaviour {
	public float MinPlanetSize = 0.025f;
	public float MaxPlanetSize = 0.1f;
	public float MinPlanetOrbitDeviation = .005f;
	public float MaxPlanetOrbitDeviation = .025f;
	public GameObject[] PlanetPrefabs;

	public GameObject GeneratePlanet(GameObject parent, GameObject sun, float minOrbitDistance) {
		var planet = (GameObject)Instantiate(PlanetPrefabs.OrderBy(_ => System.Guid.NewGuid()).First());

		var planetSize = Random.Range (MinPlanetSize, MaxPlanetSize);
		var deviation = Random.Range(MinPlanetOrbitDeviation, MaxPlanetOrbitDeviation);
		// Set parameters
		planet.transform.parent = parent.transform;
		planet.transform.localScale = new Vector3 (planetSize, planetSize, 1f);
		planet.transform.localPosition = new Vector3(0, minOrbitDistance + deviation);
		planet.transform.RotateAround(sun.transform.position, Vector3.forward, Random.Range (0, 360));
		planet.transform.eulerAngles = Vector3.zero;

		return planet;	
	}
}
