using UnityEngine;
using System.Collections;
using Assets.Player;
using Assets.Graph.Nodes;

public class EdgeData : Targetable {
	public GameObject nodeOne;
	public GameObject nodeTwo;

	public EdgeDirection direction { get; set; }

    public float visIncreaseModifier { get; set; }

    private float vis;
    public float Visibility {
        get {
            return vis;
        }

        set {
            vis = Mathf.Min(Mathf.Max(0, value), 1);
        }
    }

    protected void Awake()
    {
        direction = EdgeDirection.Neutral;
        Visibility = 0;
        visIncreaseModifier = 1.0f;
    }

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        turnController.OnTurnEnd += () => Visibility *= 0.95f;
	}

	public enum EdgeDirection {
		OneToTwo, TwoToOne, Neutral, Unusable
	};

    public void applyRandomEdgeVisibilityIncrease(float scaleParameter, float maxVisibilityIncrease, bool respectModifier = true)
    {
        Visibility += getRandomEdgeVisibilityIncrease(scaleParameter, maxVisibilityIncrease, respectModifier);
    }

    private float getRandomEdgeVisibilityIncrease(float scaleParameter, float maxVisibilityIncrease, bool respectModifier = true)
    {
        if (maxVisibilityIncrease <= 0) return 0;

        float unscaled = -1 * (scaleParameter * Mathf.Log(UnityEngine.Random.value)) % maxVisibilityIncrease;
        if (respectModifier) unscaled *= visIncreaseModifier;
        return unscaled;
    }

    public float getMaxEdgeVisibilityIncrease(float maxVisibilityIncrease, bool respectModifier = true)
    {
        if (respectModifier) maxVisibilityIncrease *= visIncreaseModifier;
        return maxVisibilityIncrease;
    }

    public float getMinEdgeVisibilityIncrease()
    {
        return 0;
    }

    public float getExpectedVisibilityIncrease(float scaleParameter, float maxVisibilityIncrease, bool respectModifier = true)
    {
        float lambda = 1 / scaleParameter;
        float x = respectModifier ? maxVisibilityIncrease * visIncreaseModifier : maxVisibilityIncrease;
        return x / lambda * Mathf.Exp(-lambda * x);
    }

    public PlayerData getOwner() {
        bool isOwned = (direction == EdgeDirection.OneToTwo || direction == EdgeDirection.TwoToOne);
        if (!isOwned) return null;
        return nodeOne.GetComponent<NodeData>().Owner;
    }
}
