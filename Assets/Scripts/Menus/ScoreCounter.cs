using UnityEngine;
using System.Collections;

public class ScoreCounter : MonoBehaviour {
	public string StringFormat = "Score {0}";
	private int _score = 0;

	// Use this for initialization
	void Awake () {
		Messenger<int>.AddListener ("scoreAdded", OnScoreAdded);
		Messenger<int>.AddListener ("printScore", OnPrintScore);
	}

	void Start() {
		Messenger<GameObject>.Broadcast ("requestScore", gameObject);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("scoreAdded", OnScoreAdded);
		Messenger<int>.RemoveListener ("printScore", OnPrintScore);
	}

	void Update() {
		guiText.text = string.Format (StringFormat, _score);
	}

	protected virtual void OnScoreAdded(int amount) {
		_score += amount;
	}

	protected virtual void OnPrintScore(int score) {
		_score = score;
	}
}
