using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SingletonMonobehaviour : MonoBehaviour {

	public bool DestroyOnGameOver = true;
	private static List<SingletonMonobehaviour> _singletons = new List<SingletonMonobehaviour> ();
	public readonly System.Guid _uniqueId = System.Guid.NewGuid();
	public string UID;

	public void Awake() {
		UID = _uniqueId.ToString ();
		if (gameObject == null) {
			_singletons.Remove (this);
			return;
		}
		if (_singletons
		    .Where (singleton => singleton != null)
		    .Where (singleton => singleton.name == this.name)
		    .Select (singleton => singleton.GetComponent<SingletonMonobehaviour> ())
		    .Where (singleton => singleton.GetUniqueId () != _uniqueId)
		    .Any ()) {
				//(var component in gameObject.GetComponents<MonoBehaviour>()) {
				//	Destroy(component);
				////}
				DestroyImmediate (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);	
			Messenger<int>.AddListener ("levelChanging", OnLevelChanging);
			Messenger<int>.AddListener ("levelChanged", OnLevelChanged);
			Messenger<GameObject>.AddListener ("gameOver", OnGameOver);
			Load ();
			_singletons.Add (this);
		}
	}

	public void OnDestroy() {
		_singletons.Remove (this);
		Messenger<int>.RemoveListener ("levelChanging", OnLevelChanging);
		Messenger<int>.RemoveListener ("levelChanged", OnLevelChanged);
		Messenger<GameObject>.RemoveListener ("gameOver", OnGameOver);
		Unload ();
	}

	public virtual void Load() {

	}

	public virtual void Unload() {

	}

	public virtual System.Guid GetUniqueId() {
		return _uniqueId;
	}

	protected virtual void OnGameOver(GameObject sender) {
		if(DestroyOnGameOver)
			Destroy (gameObject);
	}

	protected virtual void OnLevelChanging(int level) {

	}

	protected virtual void OnLevelChanged(int level) {

	}
}
