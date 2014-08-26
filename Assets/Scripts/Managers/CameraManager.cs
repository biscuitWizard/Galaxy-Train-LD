using UnityEngine;
using System.Collections;

public class CameraManager : SingletonMonobehaviour {

	public Color HyperspaceBackgroundColor = Color.black;
	public Color SolarSystemBackgroundColor = Color.black;

	protected override void OnLevelChanged (int level)
	{
		base.OnLevelChanged (level);
		switch ((Levels)level) {
			case Levels.HyperspaceMap:
			GetComponent<SmoothFollow>().Following = false;
			camera.backgroundColor = HyperspaceBackgroundColor;
				camera.transform.position = new Vector3(0, 0, -12.5f);
				break;
			case Levels.SolarSystem:
			camera.backgroundColor = SolarSystemBackgroundColor;
			camera.transform.position = new Vector3(0, -15, -10f);
			var follower = GetComponent<SmoothFollow>();
			follower.Target = follower.TryFindTarget().transform;
			follower.Following = true;
				break;
		}

		transform.eulerAngles = Vector3.zero;
	}
}
