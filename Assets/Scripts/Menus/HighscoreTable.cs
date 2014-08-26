using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

public class HighscoreTable : MonoBehaviour {
	public int HighscoreCount = 5;

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
		var sortedScores = highscores
			.OrderBy (score => score)
				.Take (HighscoreCount)
				.Reverse()
				.ToArray ();
		Debug.Log ("Scores: " + highscores.Length);

		var highscoreString = new StringBuilder ("Highscores\n");

		for(int i=0;i < sortedScores.Length;i++) {
			highscoreString.AppendLine(sortedScores[i].ToString("00000000"));
		}

		GetComponent<GUIText>().text = highscoreString.ToString ();
	}
}
