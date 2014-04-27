using UnityEngine;
using System.Collections;
using System;
using Assets.Player;

public abstract class Highlightable : MonoBehaviour {
	
	public Sprite normalSprite, highlightSprite, ownedNormalSprite, ownedHighlightSprite;
    public delegate void OnClickHandler();
    public event OnClickHandler OnClicked;

	private SpriteRenderer spriteRenderer;
	private bool isHighlighted = false;
	private Sprite curNormal, curHighlight;

	// Use this for initialization
	protected virtual void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		VisibilityController.instance.VisibilityChanged += new VisibilityController.VisibilityChangeHandler(updateVisibility);
		updateSprites();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setNormalSprite(Sprite newNormal) {
		curNormal = newNormal;
		setHighlighted(isHighlighted);
	}

	public void setHighlightedSprite(Sprite newHighlight) {
		curHighlight = newHighlight;
		setHighlighted(isHighlighted);
	}
	
	public void setHighlighted(bool isHighlighted) {
		this.isHighlighted = isHighlighted;
		spriteRenderer.sprite = isHighlighted ? curHighlight : curNormal;
	}

    void OnMouseUpAsButton()
    {
        if (OnClicked != null) OnClicked();
    }

	
	public void updateSprites() {
		updateVisibility(VisibilityController.instance.visibility);
	}
	
	private void updateVisibility(VisibilityController.Visibility vis) {
		bool owned = viewAsOwned(vis);
		setNormalSprite(owned ? ownedNormalSprite : normalSprite);
		setHighlightedSprite(owned ? ownedHighlightSprite : highlightSprite);
	}

	public abstract bool viewAsOwned(VisibilityController.Visibility visibility);
}
