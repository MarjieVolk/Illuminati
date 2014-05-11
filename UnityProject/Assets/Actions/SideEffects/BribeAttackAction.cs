using UnityEngine;
using System.Collections;

public class BribeAttackAction : SideEffect {

    /// <summary>
    /// Assist on win
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public void additionalEffect(NodeData performer, Targetable target, bool isWin) {
        if (!isWin) return;
        TemporarySupportAction.doIncrease(performer, (NodeData) target, 0.6f);
    }
}
