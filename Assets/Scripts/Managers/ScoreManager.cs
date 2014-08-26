using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class ScoreManager : SingletonMonobehaviour {
	public int PointsPerPassenger = 5;
	public List<int> Highscores = new List<int>();
	public int CurrentScore = 0;
	private int _oldScore = 0;
	private bool _loaded = false;

	// Use this for initialization
	public override void Load() {
		Messenger<GameObject>.AddListener ("requestHighscores", OnRequestHighscores);
		Messenger<GameObject>.AddListener ("requestScore", OnRequestScore);
		Messenger<GameObject>.AddListener ("requestEndScore", OnRequestEndScore);
		Messenger<GameObject>.AddListener ("gameOver", OnSaveScores);
		Messenger<int>.AddListener ("passengersRemoved", OnPassengersRemoved);
		Messenger<int>.AddListener ("bonusScore", OnBonusScore);
	}

	public override void Unload() {
		Messenger<GameObject>.RemoveListener ("requestHighscores", OnRequestHighscores);
		Messenger<GameObject>.RemoveListener ("requestScore", OnRequestScore);
		Messenger<GameObject>.RemoveListener ("requestEndScore", OnRequestEndScore);
		Messenger<GameObject>.RemoveListener ("gameOver", OnSaveScores);
		Messenger<int>.RemoveListener ("passengersRemoved", OnPassengersRemoved);
		Messenger<int>.RemoveListener ("bonusScore", OnBonusScore);
	}
	
	protected virtual void OnPassengersRemoved(int passengersRemoved) {
		CurrentScore += passengersRemoved * PointsPerPassenger;
		Messenger<int>.Broadcast ("scoreAdded", passengersRemoved * PointsPerPassenger);
	}

	protected virtual void OnSaveScores(GameObject sender) {
		if (CurrentScore > 0) {
			Highscores.Add (CurrentScore);
			_oldScore = CurrentScore;
			CurrentScore = 0;
		}

		PlayerPrefs.SetString ("Highscores", JsonSerializer.Serialize (Highscores));
		Debug.Log ("Highscores JSON: " + JsonSerializer.Serialize (Highscores));
		PlayerPrefs.Save ();
	}

	private void LoadScores() {
		CurrentScore = 0;

		var output = JsonSerializer.Deserialize(PlayerPrefs.GetString("Highscores"));
		Debug.Log ("Highscore JSON: " + PlayerPrefs.GetString ("Highscores"));
		Highscores = output ?? new List<int>();

		_loaded = true;
		
	}

	protected virtual void OnRequestHighscores(GameObject sender) {
		if(!_loaded)
			LoadScores ();
		Messenger<int[]>.Broadcast ("printHighscores", Highscores.ToArray ());
	}

	protected virtual void OnRequestScore(GameObject sender) {
		if(!_loaded)
			LoadScores ();
		Messenger<int>.Broadcast ("printScore", CurrentScore);
	}

	protected virtual void OnRequestEndScore(GameObject sender) {
		if(!_loaded)
			LoadScores ();
		Messenger<int>.Broadcast ("printScore", _oldScore);
	}

	protected virtual void OnBonusScore(int amount) {
		CurrentScore += amount;
		Messenger<int>.Broadcast ("scoreAdded", amount);
	}
}
