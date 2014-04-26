using UnityEngine;
using System.Collections;
using System;

public class Highlightable : MonoBehaviour {
	
	public Sprite highlightSprite;
    public delegate void OnClickHandler();
    public event OnClickHandler OnClicked;

	protected Sprite normalSprite;
	private SpriteRenderer spriteRenderer;
	private bool isHighlighted = false;

	// Use this for initialization
	protected virtual void Start () {
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		normalSprite = spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setNormalSprite(Sprite newNormal) {
		normalSprite = newNormal;
		if (!isHighlighted) {
			setUnhighlighted();
		}
	}

	public void setHighlightedSprite(Sprite newHighlight) {
		highlightSprite = newHighlight;
		if (isHighlighted) {
			setHighlighted();
		}
	}
	
	public void setHighlighted() {
		isHighlighted = true;
		spriteRenderer.sprite = highlightSprite;
	}
	
	public void setUnhighlighted() {
		isHighlighted = false;
		spriteRenderer.sprite = normalSprite;
	}

    void OnMouseUpAsButton()
    {
        if (OnClicked != null) OnClicked();
    }
}
