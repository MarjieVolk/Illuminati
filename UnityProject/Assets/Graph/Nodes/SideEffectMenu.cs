using UnityEngine;
using System.Collections;
using Assets.Player;

public class SideEffectMenu : RadialMenu {

    private bool isMouseOver = false;

    public override void Start() {
        base.Start();

        NodeData node = transform.parent.GetComponent<NodeData>();
        node.OnHover += () => {
            if (ActionController.instance.selected is AttackAction && ActionController.instance.getTargetsForScheduledAction().Contains(node)) {
                show();
            }

            isMouseOver = true;
        };

        node.OnEndHover += () => {
            OnMouseExit();
            hide();
            isMouseOver = false;
        };

        node.OnClicked += () => {
            OnMouseUpAsButton();
            hide();
        };

        //foreach (Action a in buttons.Keys) {
        //    Action actionCopy = a;
        //    buttons[a].GetComponent<ActionButton>().OnClick += () => ActionController.instance.selectAction(actionCopy);
        //}
    }

    void Update() {
        if (isMouseOver) {
            OnMouseOver();
        }
    }
}
