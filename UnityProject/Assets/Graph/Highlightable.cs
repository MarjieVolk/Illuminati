using UnityEngine;
using System.Collections;
using System;

public class Highlightable : MonoBehaviour {
	
	public Sprite highlightSprite;

	private Sprite normalSprite;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	protected virtual void Start () {
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		normalSprite = spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setHighlighted() {
		spriteRenderer.sprite = highlightSprite;
	}
	
	public void setUnhighlighted() {
		spriteRenderer.sprite = normalSprite;
	}
}
