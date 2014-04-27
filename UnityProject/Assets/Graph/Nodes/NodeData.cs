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

		private set {
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
		bool isPrivate = VisibilityController.instance.visibility == VisibilityController.Visibility.Public;
		bool viewAsOwned = isPrivate && owner == TurnController.instance.CurrentPlayer;

		setNormalSprite(viewAsOwned ? playerOwnedNormal : unownedNormal);
		setHighlightedSprite(viewAsOwned ? playerOwnedHighlight : unownedHighlight);
	}
}
