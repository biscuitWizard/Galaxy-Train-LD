using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public class GameManager : SingletonMonobehaviour {

	public int PriorityPlanetBonusSeconds = 15;
	public int GameLengthSeconds = 120;
	private float _elapsedSeconds = 0;
	private float _timeSinceLastUpdate = 0;
	private bool _gamePaused = true;

	// Use this for initialization
	public override void Load() {
		Application.targetFrameRate = 30;
		Messenger<bool>.AddListener ("requestTimeRemaining", OnRequestTimeRemaining);
		Messenger<bool>.AddListener ("startLoading", OnStartLoading);
		Messenger<Planet>.AddListener ("priorityCompleted", OnPriorityCompleted);
		Messenger<int>.AddListener ("addPassengerCart", OnAddPassengerCart);
	}

	public override void Unload() {
		Messenger<bool>.RemoveListener ("requestTimeRemaining", OnRequestTimeRemaining);
		Messenger<bool>.RemoveListener ("startLoading", OnStartLoading);
		Messenger<Planet>.RemoveListener ("priorityCompleted", OnPriorityCompleted);
		Messenger<int>.RemoveListener ("addPassengerCart", OnAddPassengerCart);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_gamePaused) {
				_elapsedSeconds += Time.deltaTime;
				_timeSinceLastUpdate += Time.deltaTime;

				if (_timeSinceLastUpdate >= 1) {
					if(Messenger<TimeSpan>.HasListener("timeRemainingUpdate"))
						Messenger<TimeSpan>.Broadcast ("timeRemainingUpdate", new TimeSpan (0, 0, GetRemainingTime ()));
					_timeSinceLastUpdate = 0;
				}

				if (IsGameOver ()) {
					EndGame ();
			}
		}
	}

	public void StartGame() {
		if(Messenger<GameObject>.HasListener ("gameStarted"))
			Messenger<GameObject>.Broadcast ("gameStarted", gameObject);
		if(Messenger<TimeSpan>.HasListener("timeRemainingUpdate"))
			Messenger<TimeSpan>.Broadcast ("timeRemainingUpdate", new TimeSpan (0, 0, GetRemainingTime ()));
	}

	public void PauseGame() {
		_gamePaused = true;
		if(Messenger<GameObject>.HasListener("gamePaused"))
			Messenger<GameObject>.Broadcast ("gamePaused", gameObject);
	}

	public void EndGame() {
		PauseGame ();
		Messenger<int>.Broadcast ("loadLevel", (int)Levels.GameOver);
		Messenger<GameObject>.Broadcast ("gameOver", gameObject);
	}

	public void ResumeGame() {
		_gamePaused = false;
		if(Messenger<GameObject>.HasListener("gameResumed"))
			Messenger<GameObject>.Broadcast ("gameResumed", gameObject);
	}

	protected virtual void OnRequestTimeRemaining(bool srsly) {
		Messenger<TimeSpan>.Broadcast ("timeRemainingUpdate", new TimeSpan (0, 0, GetRemainingTime()));
	}


	protected virtual void OnStartLoading(bool newGame) {
		Messenger<GameObject>.Broadcast ("startedLoading", gameObject);
		Messenger<GameObject>.Broadcast ("finishedLoading", gameObject);
	}

	private int GetRemainingTime() {
		return Mathf.RoundToInt (GameLengthSeconds - _elapsedSeconds);
	}

	protected virtual bool IsGameOver() {
		return GetRemainingTime() <= 0;
	}

	private enum Bonuses {
		BonusTime,
		BonusBrake,
		BonusScore,
		BonusTurning
	}

	protected virtual void OnPriorityCompleted(Planet planet) {
		// Bonus time!

		var values = Enum.GetValues (typeof(Bonuses));
		var choice = (Bonuses)values.GetValue (UnityEngine.Random.Range (0, values.Length));
		switch (choice) {
		case Bonuses.BonusTime:
			GameLengthSeconds += PriorityPlanetBonusSeconds;
			Messenger<string>.Broadcast ("achievementEarned", "+15s Bonus Time!");
			break;
		case Bonuses.BonusBrake:
			Messenger<string>.Broadcast ("achievementEarned", "Improved Brakes Installed!");
			Messenger<GameObject>.Broadcast("bonusBrakes", gameObject);
			break;
		case Bonuses.BonusScore:
			var bonusScore = UnityEngine.Random.Range(50,150) * 5;
			Messenger<string>.Broadcast ("achievementEarned", string.Format ("{0} Added to Score!", bonusScore));
			Messenger<int>.Broadcast("bonusScore", bonusScore);
			break;
		case Bonuses.BonusTurning:
			Messenger<GameObject>.Broadcast("bonusTurning", gameObject);
			Messenger<string>.Broadcast("achievementEarned", "Improved Turning Jets Installed!");
			break;
		}
	}

	protected override void OnLevelChanged(int level) {
		switch ((Levels)level) {
			case Levels.HyperspaceMap:
				PauseGame ();
			break;
			case Levels.SolarSystem:
				ResumeGame();
			break;
		}
	}

	protected virtual void OnAddPassengerCart(int amount) {
		Messenger<string>.Broadcast ("achievementEarned", "+5s Bonus Time!");
		GameLengthSeconds += 5;
	}
}