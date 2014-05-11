using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Assets.Graph.Nodes;

public class ActionButton : NodeButton {

    public override void Start() {
        base.Start();
    }

    public override void OnMouseUpAsButton() {
        if (!ActionEnabled) return;

        bool wasSelected = ActionController.instance.inSelectionState && belongsToAction(ActionController.instance.selected);
        if (ActionController.instance.inSelectionState) {
            ActionController.instance.clearSelectionState();
        }

        if (!wasSelected) {
            base.OnMouseUpAsButton();
        }
    }

    private bool belongsToAction(Action a) {
        Dictionary<Action, GameObject> buttons = transform.parent.GetComponent<NodeMenu>().buttons;
        return buttons.ContainsKey(a) && buttons[a] == this.gameObject;
    }
}
