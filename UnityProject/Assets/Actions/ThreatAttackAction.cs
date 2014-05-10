using UnityEngine;
using System.Collections;

public class ThreatAttackAction : AttackAction {

    /// <summary>
    /// Instruct on win
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public override void additionalEffect(Targetable target, bool isWin) {
        if (!isWin) return;

        NodeData node = this.gameObject.GetComponent<NodeData>();
        PermanentSupportAction.doIncrease(node, (NodeData)target, 0.6f);
    }
}
