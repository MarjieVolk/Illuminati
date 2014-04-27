using UnityEngine;
using System.Collections;
using Assets.Player;

public class EdgeData : Highlightable {

    public float minVisibilityIncrease, maxVisibilityIncrease;

	public GameObject nodeOne;
	public GameObject nodeTwo;

	public DominationType type {get; set;}
	public EdgeDirection direction {get; set;}

    public float visibility { get; private set; }
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		direction = EdgeDirection.Neutral;
		type = DominationType.Bribe;
        visibility = 0;

        TurnController.instance.OnTurnEnd += () => visibility *= 0.9f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public enum EdgeDirection {
		OneToTwo, TwoToOne, Neutral
	};

    public void triggerEdge(float baseTriggerProbability)
    {
        float rand = 1.0f;
        while (rand == 1.0f)
        {
            rand = Random.value;
        }

        if (rand < baseTriggerProbability)
            triggerEdge();
    }

    private void triggerEdge()
    {
        float rand = Random.value;
        float visibilityDelta = rand * (maxVisibilityIncrease - minVisibilityIncrease) + minVisibilityIncrease;

        visibility += visibilityDelta;
    }
}
