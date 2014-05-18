using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using System.Linq;
using Assets.Graph.Nodes;

public class SideEffectMenu : RadialMenu {

    private bool isMouseOver = false;
    private Dictionary<SideEffect, GameObject> buttons;

    private AttackAction currentAction;

    public override List<GameObject> getButtons() {
        SideEffect[] sideEffects = gameObject.GetComponents<SideEffect>();
        buttons = new Dictionary<SideEffect, GameObject>();

        foreach (SideEffect e in sideEffects) {
            GameObject realButton = (GameObject) Instantiate(e.button, transform.position, Quaternion.identity);
            realButton.transform.parent = this.gameObject.transform;
            buttons[e] = realButton;
        }

        return buttons.Values.ToList();
    }

    public override void Start() {
        base.Start();

        NodeData node = transform.parent.GetComponent<NodeData>();
        NodeGUI nodeGUI = transform.parent.GetComponent<NodeGUI>();
        nodeGUI.OnHover += () => {
            if (ActionController.instance.selected is AttackAction && ActionController.instance.getTargetsForScheduledAction().Contains(node)) {
                show();
                currentAction = (AttackAction) ActionController.instance.selected;
            }

            isMouseOver = true;
        };

        nodeGUI.OnEndHover += () => {
            OnMouseExit();
            hide();
            isMouseOver = false;
        };

        nodeGUI.OnClicked += () => {
            OnMouseUpAsButton();
            hide();
        };

        foreach (SideEffect e in buttons.Keys) {
            SideEffect copy = e;
            buttons[e].GetComponent<NodeButton>().OnClick += () => {
                currentAction.setSideEffect(copy);
            };
        }
    }

    void Update() {
        if (isMouseOver) {
            OnMouseOver();
        }
    }
}
