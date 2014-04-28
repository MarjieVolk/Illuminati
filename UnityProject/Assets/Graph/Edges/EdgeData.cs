using UnityEngine;
using System.Collections;
using Assets.Player;

public class EdgeData : Targetable {

    public float minVisibilityIncrease, maxVisibilityIncrease;

	public GameObject nodeOne;
	public GameObject nodeTwo;

	public GameObject arrowHead;

	public DominationType type {get; set;}
	public EdgeDirection direction {get; set;}

    public float Visibility { get; set; }
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		direction = EdgeDirection.Neutral;
		type = DominationType.Bribe;
        Visibility = 0;

        TurnController.instance.OnTurnEnd += () => Visibility *= 0.9f;
        TurnController.instance.OnTurnEnd += () => triggerEdge(0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public enum EdgeDirection {
		OneToTwo, TwoToOne, Neutral, Unusable
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

        Visibility += visibilityDelta;
    }

    private bool displayVisibility = false;

    void OnMouseEnter()
    {
        displayVisibility = true;
    }

    void OnMouseExit()
    {
        displayVisibility = false;
    }

    void OnGUI()
    {
        if (displayVisibility) GUI.Label(new Rect(500, 0, 1000, 20), "Visibility: " + (int)(Visibility * 100) + "%");
    }
	
	override public bool viewAsOwned(VisibilityController.Visibility vis) {
		bool isPrivate = vis == VisibilityController.Visibility.Private;
		bool isOwned = direction != EdgeDirection.Neutral;

		if (isPrivate && isOwned) {
			return nodeOne.GetComponent<NodeData>().Owner == TurnController.instance.CurrentPlayer;
		}

		return false;
	}
}
