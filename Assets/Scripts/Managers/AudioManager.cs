using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip ChooChooNormal;
	public AudioClip ChooChooLower;
	public AudioClip ChooChooLowest;
	public AudioClip TimeRunningOut;
	public AudioClip ReadyGo;

	// Use this for initialization
	void Awake() {
		Messenger<int>.AddListener ("passengerCartAdded", OnPassengerCartAdded);
		Messenger<GameObject>.AddListener ("timeRunningOut", OnTimeRunningOut);
		Messenger<AudioClip>.AddListener ("playSound", OnPlaySound);
		Messenger<GameObject>.AddListener ("gameStarted", OnGameStarted);
	}

	void OnDestroy() {
		Messenger<int>.RemoveListener ("passengerCartAdded", OnPassengerCartAdded);
		Messenger<AudioClip>.RemoveListener ("playSound", OnPlaySound);
		Messenger<GameObject>.RemoveListener ("timeRunningOut", OnTimeRunningOut);
		Messenger<GameObject>.RemoveListener ("gameStarted", OnGameStarted);
	}

	protected virtual void OnPassengerCartAdded(int carts) {
		if (carts > 4) {
			audio.PlayOneShot(ChooChooLowest);
		} else if(carts > 3) {
			audio.PlayOneShot(ChooChooLower);
		} else if(carts > 2) {
			audio.PlayOneShot(ChooChooNormal);
		}
	}

	protected virtual void OnPlaySound(AudioClip clip) {
		audio.PlayOneShot (clip);
	}

	protected virtual void OnTimeRunningOut(GameObject sender) {
		audio.PlayOneShot (TimeRunningOut);
	}

	protected virtual void OnGameStarted(GameObject sender) {
		audio.PlayOneShot (ReadyGo);
	}
}
