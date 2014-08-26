using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GalaxyManager : SingletonMonobehaviour {

	public int DensityX = 5;
	public int DensityY = 4;
	public bool StartInactive = false;
	public bool GenerateOnStart = false;
	public float Deviation = 1f;
	public float BlankPercentage = .20f;
	public Vector2 SceneWidthLimits = new Vector2 (-10, 10);
	public Vector2 SceneHeightLimits = new Vector2 (-6, 6);
	public GameObject GalaxyMarker;
	public Color[] GalaxyColors = new [] { Color.white };
	public TextAsset GalaxyNames;
	private readonly IList<GalaxyMarker> _galaxies = new List<GalaxyMarker>();
	private GameObject _hyperspaceRoot;

	// Use this for initialization
	public override void Load() {
		Messenger<bool>.AddListener ("startLoading", OnStartLoading);
		Messenger<Planet>.AddListener ("priorityAlert", OnPriorityAlert);
		Messenger<Planet>.AddListener ("priorityCompleted", OnPriorityCompleted);
	}

	public override void Unload() {
		Messenger<bool>.RemoveListener ("startLoading", OnStartLoading);
		Messenger<Planet>.RemoveListener ("priorityAlert", OnPriorityAlert);
		Messenger<Planet>.RemoveListener ("priorityCompleted", OnPriorityCompleted);
	}

	void Start () {
		if (GenerateOnStart)
			Generate ();
	}

	protected override void OnLevelChanging(int level) {
		base.OnLevelChanging (level);

		switch ((Levels)level) {
		case Levels.GameOver:
			Destroy ();
			break;
		case Levels.SolarSystem:
			_hyperspaceRoot.SetActive (false);
			break;
		case Levels.HyperspaceMap:
			_hyperspaceRoot.SetActive (true);
			break;
		}
	}

	public void Generate() {
		Destroy();

		_hyperspaceRoot = new GameObject ("Hyperspace");
		_hyperspaceRoot.transform.localScale = Vector3.one;
		_hyperspaceRoot.transform.position = Vector3.zero;
		_hyperspaceRoot.SetActive (!StartInactive);
		DontDestroyOnLoad (_hyperspaceRoot);

		var nameStack = new Stack<string> ();
		foreach (var name in  GalaxyNames.text.Split(new char[] { '\n' }).OrderBy(_ => Guid.NewGuid()))
						nameStack.Push (name);
		var incrementX = (Mathf.Abs (SceneWidthLimits.x) + SceneWidthLimits.y) / DensityX;
		var incrementY = (Mathf.Abs (SceneHeightLimits.x) + SceneHeightLimits.y) / DensityY;
		var offsetX = 1.8f;
		var offsetY = 1.8f;

		for(var x = 0; x < DensityX;x++) {
			for(var y = 0;y < DensityY;y++) {
				if(UnityEngine.Random.Range (0, 100) / 100f < BlankPercentage)
					continue;
				var galaxyDeviationX = UnityEngine.Random.Range (Deviation / 2f * -1f, Deviation / 2f);
				var galaxyDeviationY = UnityEngine.Random.Range (Deviation / 2f * -1f, Deviation / 2f);
				var galaxyX = (incrementX * x) + SceneWidthLimits.x + offsetX + galaxyDeviationX;
				var galaxyY = (incrementY * y) + SceneHeightLimits.x + offsetY + galaxyDeviationY;

				var gMarker = (GameObject)Instantiate(GalaxyMarker);
				gMarker.transform.localScale = new Vector3(2.5f, 2.5f, 1);
				gMarker.transform.parent = _hyperspaceRoot.transform;
				gMarker.transform.localPosition = new Vector3(galaxyX, galaxyY, 0);
				gMarker.GetComponent<SpriteRenderer>().color = GalaxyColors.OrderBy (_ => Guid.NewGuid()).First ();
				var marker = gMarker.GetComponent<GalaxyMarker>();
				marker.SetGalaxyName(nameStack.Pop ());
				_galaxies.Add (marker);
			    
				DontDestroyOnLoad(gMarker);

				// Send the generate message.
				Messenger<GalaxyMarker>.Broadcast ("generateSolarSystem", marker);
			}
		}

	}

	public void Destroy() {
		if(_hyperspaceRoot != null)
			Destroy (_hyperspaceRoot);
		_galaxies.Clear ();
	}

	protected virtual void OnStartLoading(bool newGame) {
		Messenger<GameObject>.Broadcast ("startedLoading", gameObject);
		Generate ();
		Messenger<GameObject>.Broadcast ("finishedLoading", gameObject);
	}

	protected virtual void OnPriorityAlert(Planet planet) {
		var galaxy = _galaxies
			.Where (g => g.name == planet.transform.parent.name)
				.FirstOrDefault ();
		if(galaxy != null)
			galaxy.GetComponent<GalaxyMarker> ()
				.SetPriority(true);
	}

	protected virtual void OnPriorityCompleted(Planet planet) {
		_galaxies
			.Where (galaxy => galaxy.name == planet.transform.parent.name)
				.First ()
				.GetComponent<GalaxyMarker> ()
				.SetPriority(false);
	}
}
