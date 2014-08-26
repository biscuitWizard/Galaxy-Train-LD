using UnityEngine;
using System.Collections;

public class AchievementEarned : GUIDebugTextMonoBehaviour {

	public string StringFormat = "{0}";
	private bool _fading;

	// Use this for initialization
	void Awake () {
		Messenger<string>.AddListener ("achievementEarned", OnAchievementEarned);
	}

	void Start() {
		StartCoroutine (Fade.use.Alpha (guiText.material, 1.0f, 0.0f, 0f));
	}

	void OnDestroy() {
		Messenger<string>.RemoveListener ("achievementEarned", OnAchievementEarned);
	}
	
	protected virtual void OnAchievementEarned(string achievement) {
		if (_fading)
			return;
		guiText.text = string.Format (StringFormat, achievement);
		FadeInOut ();
	}

	void FadeInOut() {
		_fading = true;
		StartCoroutine(Fade.use.Alpha (guiText.material, 0.0f, 1.0f, 0.3f));
		StartCoroutine(Fade.use.Alpha (guiText.material, 1.0f, 0.0f, 2f));
		_fading = false;
	}
}
