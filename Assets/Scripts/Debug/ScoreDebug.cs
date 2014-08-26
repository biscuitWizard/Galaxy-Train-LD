using UnityEngine;
using System.Collections;

public class ScoreDebug : GUIDebugTextMonoBehaviour {

	public string StringFormat = "Score: {0}";
	private int _score = 0;

	// Use this for initialization
	void Awake () {
		Messenger<int>.AddListener ("scoreAdded", OnScoreAdded);
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = string.Format (StringFormat, _score);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("scoreAdded", OnScoreAdded);
	}

	protected virtual void OnScoreAdded(int scoreAdded) {
		_score += scoreAdded;
	}
}
