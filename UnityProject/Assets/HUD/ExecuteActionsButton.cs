using UnityEngine;
using System.Collections;
using Assets.Player;

public class ExecuteActionsButton : HUDButton {

	private bool isClick = false;
	
	protected override void Start() {
        base.Start();
		x = 0.01f;
        y = 0.157f;
        TurnController.instance.OnTurnEnd += toggleButton;
	}
	
	override public bool viewAsOwned(VisibilityController.Visibility visibility) {
		return isClick;
	}
	
	void OnMouseDown() {
        if (TurnController.instance.CurrentPlayer.IsLocalHumanPlayer)
        {
            isClick = true;
            updateSprites();
        }
	}
	
	void OnMouseUp() {
        if (TurnController.instance.CurrentPlayer.IsLocalHumanPlayer)
        {
            isClick = false;
            updateSprites();
        }
	}
	
	void OnMouseUpAsButton() {
        if (TurnController.instance.CurrentPlayer.IsLocalHumanPlayer)
        {
            OnMouseUp();
            TurnControllerGUI.instance.TryExecuteActions();
        }
	}

    private void toggleButton() {
        GameObject parent = transform.parent.gameObject;
        OnMouseUp();
        OnMouseExit();
        ButtonToggler toggler = parent.GetComponent<ButtonToggler>();
        toggler.toggle(gameObject);
    }
}
