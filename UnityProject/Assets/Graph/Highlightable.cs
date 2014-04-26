using UnityEngine;
using System.Collections;
using System;

public class Highlightable : MonoBehaviour {
	
	public Sprite highlightSprite;
    public delegate void OnClickHandler();
    public event OnClickHandler OnClicked;

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

	void OnMouseEnter() {
		setHighlighted();
	}

	void OnMouseExit() {
		setUnhighlighted();
	}

    void OnMouseUpAsButton()
    {
        if (OnClicked != null) OnClicked();
    }
}
