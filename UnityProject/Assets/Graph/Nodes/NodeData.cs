﻿using UnityEngine;
using System.Collections;
using Assets.Player;

public class NodeData : Highlightable {

	public PlayerData startingOwner;

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

	public int getAttack(DominationType type) {
		AttackSkill[] skills = gameObject.GetComponents<AttackSkill>();
		foreach (AttackSkill a in skills) {
			if (a.type == type) {
				return a.value;
			}
		}
		return 0;
	}

	public int getDefense(DominationType type) {
		DefenseSkill[] skills = gameObject.GetComponents<DefenseSkill>();
		foreach (DefenseSkill d in skills) {
			if (d.type == type) {
				return d.value;
			}
		}
		return 0;
	}

	override public bool viewAsOwned(VisibilityController.Visibility vis) {
		bool isPrivate = vis == VisibilityController.Visibility.Private;
		return isPrivate && owner == TurnController.instance.CurrentPlayer;
	}
}
