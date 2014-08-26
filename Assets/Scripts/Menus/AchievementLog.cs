using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementLog : MonoBehaviour {

	public Font FontFamily;
	public Color AchievementColor = Color.yellow;
	public float FadeDelay = 4f;
	private readonly List<GameObject> _logEntries = new List<GameObject> ();
	private readonly Vector2 _logStartPos = new Vector2 (-0.48f, 0.25f);

	// Use this for initialization
	void Awake () {
		Messenger<string>.AddListener ("achievementEarned", OnAchievementEarned);
	}
	
	// Update is called once per frame
	void OnDestroy() {
		Messenger<string>.RemoveListener ("achievementEarned", OnAchievementEarned);
	}

	protected virtual void OnAchievementEarned(string message) {
		CreateLogEntry (message);
	}

	private void CreateLogEntry(string message) {
		var log = new GameObject ("Log: " + message, new [] { typeof(GUIText) });
		log.transform.parent = transform;
		log.guiText.fontSize = 22;
		log.guiText.font = FontFamily;
		log.guiText.alignment = TextAlignment.Left;
		log.transform.localPosition = new Vector2 (_logStartPos.x, _logStartPos.y);

		log.guiText.text = message;
		StartCoroutine(Fade.use.Alpha (log.guiText.material, 1.0f, 0.0f, FadeDelay));
		StartCoroutine (DestroyLog(log));

		foreach (var logEntry in _logEntries) {
			logEntry.transform.localPosition = new Vector3(_logStartPos.x, logEntry.transform.localPosition.y + -0.035f, 0);
		}

		_logEntries.Add (log);
	}

	private IEnumerator DestroyLog(GameObject log) {
		yield return new WaitForSeconds (FadeDelay + 1f);
		_logEntries.Remove (log);
		Destroy (log);
	}
}
