using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class Skill : MonoBehaviour {
    public DominationType type;
    public int value;

    private List<TemporaryIncrease> increases;

    // Use this for initialization
    void Start() {
        increases = new List<TemporaryIncrease>();
        TurnController.instance.OnTurnStart += onTurnStart;
    }

    // Update is called once per frame
    void Update() {

    }

    public int getWorkingValue() {
        int val = value;
        foreach (TemporaryIncrease inc in increases) {
            val += inc.amount;
        }
        return val;
    }

    private void onTurnStart() {
        if (TurnController.instance.CurrentPlayer != gameObject.GetComponent<NodeData>().Owner) {
            // Only decrement at the start of your turn
            return;
        }

        List<TemporaryIncrease> toRemove = new List<TemporaryIncrease>();
        foreach (TemporaryIncrease inc in increases) {
            inc.nTurns -= 1;
            if (inc.nTurns <= 0) {
                toRemove.Add(inc);
            }
        }

        foreach (TemporaryIncrease inc in toRemove) {
            increases.Remove(inc);
        }
    }

    public void temporaryIncrease(int amount, int nTurns) {
        if (nTurns <= 0) {
            return;
        }
        TemporaryIncrease inc = new TemporaryIncrease();
        inc.amount = amount;
        inc.nTurns = nTurns;
        increases.Add(inc);
    }

    private class TemporaryIncrease {
        public int amount;
        public int nTurns;
    }
}
