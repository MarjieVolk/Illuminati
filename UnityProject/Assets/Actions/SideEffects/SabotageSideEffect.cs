using UnityEngine;
using System.Collections;

public class SabotageSideEffect : SideEffect {

    /// <summary>
    /// Sabotage on loss
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public override void additionalEffect(NodeData performer, Targetable target, bool isWin) {
        if (isWin) return;
        SabotageAction.doDecrease(performer, (NodeData) target, 0.6f);
    }
}
