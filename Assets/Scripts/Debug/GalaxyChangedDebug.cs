using UnityEngine;
using System.Collections;

public class GalaxyChangedDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "{0}";
	private bool _fading = false;

	// Use this for initialization
	void Awake() {
		Messenger<string>.AddListener ("galaxyVisited", OnGalaxyVisited);
	}

	void Start () {
		FadeInOut ();
	}

	void OnDestroy() {
		Messenger<string>.RemoveListener ("galaxyVisited", OnGalaxyVisited);
	}

	protected virtual void OnGalaxyVisited(string galaxyName) {
		if (_fading)
			return;
		guiText.text = string.Format (StringFormat, galaxyName);
		FadeInOut ();
	}

	void FadeInOut() {
		_fading = true;
		StartCoroutine(Fade.use.Alpha (guiText.material, 0.0f, 1.0f, 0.3f));
		StartCoroutine(Fade.use.Alpha (guiText.material, 1.0f, 0.0f, 2f));
		_fading = false;
	}
}
