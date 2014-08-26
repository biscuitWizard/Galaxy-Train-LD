using UnityEngine;
using System.Collections;
using System.Linq;

public class HighscoreCounter : MonoBehaviour {

	public string StringFormat = "Highscore {0}";

	// Use this for initialization
	void Awake() {
		Messenger<int[]>.AddListener ("printHighscores", OnPrintHighscores);
	}

	void Start() {
		Messenger<GameObject>.Broadcast ("requestHighscores", gameObject);
	}

	void OnDestroy() {
		Messenger<int[]>.RemoveListener ("printHighscores", OnPrintHighscores);
	}
	
	protected virtual void OnPrintHighscores(int[] highscores) {
		guiText.text = string.Format (StringFormat, highscores.OrderBy (score => score).FirstOrDefault ());
	}
}
