using UnityEngine;
using System.Collections;
using Assets.Player;

public class NodeData : Highlightable {

	public Sprite playerOwnedNormal, playerOwnedHighlight;
	public PlayerData startingOwner;

	private Sprite unownedNormal, unownedHighlight;

	private PlayerData owner;
	public PlayerData Owner {
		get {
			return owner;
		}

		set {
			owner = value;
			updateSprites();
		}
	}

	// Used for freezing the node for a certain number of turns after performing an action
	private int nTurnsUntilAvailable;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		unownedNormal = normalSprite;
		unownedHighlight = highlightSprite;
		Owner = startingOwner;
		VisibilityController.instance.VisibilityChanged += new VisibilityController.VisibilityChangeHandler(updateVisibility);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		// Show node menu
		if (owner == TurnController.instance.CurrentPlayer && !ActionController.instance.inSelectionState) {
			this.gameObject.GetComponent<NodeMenu>().show();
		}
	}

	public void updateSprites() {
		updateVisibility(VisibilityController.instance.visibility);
	}

	private void updateVisibility(VisibilityController.Visibility vis) {
		bool isPrivate = vis == VisibilityController.Visibility.Private;
		bool viewAsOwned = isPrivate && owner == TurnController.instance.CurrentPlayer;
		
		setNormalSprite(viewAsOwned ? playerOwnedNormal : unownedNormal);
		setHighlightedSprite(viewAsOwned ? playerOwnedHighlight : unownedHighlight);
	}
}
