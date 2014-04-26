using UnityEngine;
using System.Collections;

public class NodeData : Highlightable {

	public Sprite playerOwnedNormal, playerOwnedHighlight;

	private Sprite unownedNormal, unownedHighlight;

	private PlayerData owner;
	public PlayerData Owner {
		get {
			return owner;
		}

		private set {
			owner = value;
			setNormalSprite(owner == null ? unownedNormal : playerOwnedNormal);
			setHighlightedSprite(owner == null ? unownedHighlight : playerOwnedHighlight);
		}
	}

	// Used for freezing the node for a certain number of turns after performing an action
	private int nTurnsUntilAvailable;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		unownedNormal = normalSprite;
		unownedHighlight = highlightSprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		// Show node menu
		this.gameObject.GetComponent<NodeMenu>().show();
	}
}
