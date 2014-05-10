using UnityEngine;
using System.Collections;

public class BlackmailAttackAction : AttackAction {

    /// <summary>
    /// Sabotage on loss
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public override void additionalEffect(Targetable target, bool isWin) {
        if (isWin) return;

        NodeData thisNode = gameObject.GetComponent<NodeData>();
        SabotageAction.doDecrease(thisNode, (NodeData) target, 0.6f);
    }
}
