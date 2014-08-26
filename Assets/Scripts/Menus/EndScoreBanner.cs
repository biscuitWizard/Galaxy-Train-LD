using UnityEngine;
using System.Collections;

public class EndScoreBanner : MonoBehaviour {

	public string StringFormat = "End Score {0}";

	// Use this for initialization
	void Awake() {
		
		Messenger<int>.AddListener ("printScore", OnPrintScores);
	}

	void Start () {
		Messenger<GameObject>.Broadcast ("requestEndScore", gameObject);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("printScore", OnPrintScores);
	}
	
	protected virtual void OnPrintScores(int score) {
		guiText.text = string.Format (StringFormat, score);
	}
}
