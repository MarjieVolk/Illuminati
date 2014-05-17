using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Assets.Graph.Nodes;

public class ActionButton : NodeButton {

    public Action Action { get; private set; }

    public override void Start() {
        base.Start();
    }

    public override void OnMouseUpAsButton() {
        if (!ActionEnabled) return;

        bool wasSelected = ActionController.instance.inSelectionState && ActionController.instance.selected == Action;
        if (ActionController.instance.inSelectionState) {
            ActionController.instance.clearSelectionState();
        }

        if (!wasSelected) {
            base.OnMouseUpAsButton();
        }
    }

    public void SetAction(Action a)
    {
        if (this.Action != null) throw new UnityException("Can't reassign the action corresponding to a button!");
        this.Action = a;

        this.OnClick += () => ActionController.instance.selectAction(Action);
        Action.OnStateUpdate += OnActionUpdated;
    }

    private void OnActionUpdated(Action a)
    {
        ActionEnabled = !a.IsOnCooldown;
    }
}
