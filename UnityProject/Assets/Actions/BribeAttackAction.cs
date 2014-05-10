using UnityEngine;
using System.Collections;

public class BribeAttackAction : MonoBehaviour {

    /// <summary>
    /// Assist on win
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    public void additionalEffect(Targetable target, bool isWin) {
        if (!isWin) return;

        NodeData thisNode = gameObject.GetComponent<NodeData>();
        TemporarySupportAction.doIncrease(thisNode, (NodeData) target, 0.6f);
    }
}
