using UnityEngine;
using System.Collections;

public class AssistSideEffect : SideEffect {

    /// <summary>
    /// Assist on win
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public override void additionalEffect(NodeData performer, Targetable target, bool isWin) {
        if (!isWin) return;
        TemporarySupportAction.doIncrease(performer, (NodeData) target, 0.6f);
    }
}
