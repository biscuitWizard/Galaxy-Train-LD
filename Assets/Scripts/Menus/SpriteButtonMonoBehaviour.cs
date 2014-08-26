using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class SpriteButtonMonoBehaviour: MonoBehaviour {

	public Color NormalButtonColor = Color.white;
	public Color MouseOverButtonColor = Color.white;
	public Color ClickButtonColor = Color.white;
	private SpriteRenderer _sprite;

	// Use this for initialization
	public void Awake () {
		_sprite = GetComponent<SpriteRenderer> ();
		_sprite.color = NormalButtonColor;
	}
	
	public void OnMouseEnter() {
		_sprite.color = MouseOverButtonColor;
	}

	public void OnMouseExit() {
		_sprite.color = NormalButtonColor;
	}

	public void OnMouseDown() {
		_sprite.color = ClickButtonColor;
	}

	public void OnMouseUp() {
		_sprite.color = NormalButtonColor;
	}
}
