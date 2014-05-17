using UnityEngine;
using System.Collections;
using Assets.Player;
using Assets.Graph.Nodes;

public class EdgeData : Targetable {
	public GameObject nodeOne;
	public GameObject nodeTwo;

	public GameObject arrowHead;
	private GameObject realArrowHead;

    public GameObject ex;
    private GameObject realEx;

	public EdgeDirection direction { get; set; }
	private EdgeDirection prevDirection;

	private GUIStyle visibilityStyle;

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
        prevDirection = EdgeDirection.Neutral;
        Visibility = 0;
        visIncreaseModifier = 1.0f;

        visibilityStyle = new GUIStyle();
        visibilityStyle.normal.textColor = Color.black;
        visibilityStyle.fontStyle = FontStyle.Bold;
        visibilityStyle.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.9f));
        visibilityStyle.alignment = TextAnchor.MiddleCenter;
    }

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        TurnController.instance.OnTurnEnd += () => Visibility *= 0.95f;
		TurnController.instance.OnTurnEnd += updateVisibilityRendering;
		VisibilityController.instance.VisibilityChanged += new VisibilityController.VisibilityChangeHandler(updateArrowHead);
        OnHover += () => displayVisibility = true;
        OnEndHover += () => displayVisibility = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (realEx == null) realEx = (GameObject)Instantiate(ex, gameObject.transform.position, gameObject.transform.rotation);
        realEx.SetActive(direction == EdgeDirection.Unusable);
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

    private bool displayVisibility = false;

    public override void OnGUI()
    {
        base.OnGUI();
        if (displayVisibility) {
			float margin = 10;
            string text = "Visibility: " + (int)(Visibility * 100) + "%";

            bool isOwned = direction == EdgeDirection.OneToTwo || direction == EdgeDirection.TwoToOne;
            if (isOwned && nodeOne.GetComponent<NodeData>().Owner == TurnController.instance.CurrentPlayer) {
                text += "\nIncrease Rate: " + Mathf.Round(100 * visIncreaseModifier) + "%";
            }
            Vector2 textSize = visibilityStyle.CalcSize(new GUIContent(text));

			GUI.Label(new Rect(Screen.width - textSize.x - margin - 5, Screen.height - textSize.y - margin - 2.5f, textSize.x + 10, textSize.y + 5), text, visibilityStyle);
		}
    }
	
	override public bool viewAsOwned(VisibilityController.Visibility vis) {
		bool isPrivate = vis == VisibilityController.Visibility.Private;
		return isPrivate && getOwner() == TurnController.instance.CurrentPlayer;
	}

    public PlayerData getOwner() {
        bool isOwned = (direction == EdgeDirection.OneToTwo || direction == EdgeDirection.TwoToOne);
        if (!isOwned) return null;
        return nodeOne.GetComponent<NodeData>().Owner;
    }

	private void updateArrowHead(VisibilityController.Visibility vis) {
		if (realArrowHead == null) {
			realArrowHead = (GameObject) Instantiate(arrowHead, transform.position, Quaternion.identity);

			Vector2 nodeOnePosition = new Vector2(nodeOne.transform.position.x, nodeOne.transform.position.y);
			Vector2 nodeTwoPosition = new Vector2(nodeTwo.transform.position.x, nodeTwo.transform.position.y);
			Vector2 edgeVector = nodeTwoPosition - nodeOnePosition;
			float edgeAngle = Vector2.Angle(Vector2.right, edgeVector);
			if (Vector3.Cross(Vector2.right, edgeVector).z < 0)
			{
				edgeAngle *= -1;
			}
			realArrowHead.transform.Rotate(Vector3.forward, edgeAngle);
		}

		realArrowHead.SetActive(viewAsOwned(vis));

		EdgeDirection simplePrev = (prevDirection == EdgeDirection.TwoToOne ? EdgeDirection.TwoToOne : EdgeDirection.OneToTwo);
		EdgeDirection simpleDir = (direction == EdgeDirection.TwoToOne ? EdgeDirection.TwoToOne : EdgeDirection.OneToTwo);
		if (simplePrev != simpleDir) {
			realArrowHead.transform.Rotate(Vector3.forward, 180);
			prevDirection = direction;
		}

		updateVisibilityRendering();
	}

	private void updateVisibilityRendering() {
		if (VisibilityController.instance.visibility == VisibilityController.Visibility.Public) {
			this.GetComponent<SpriteRenderer>().color = new Color(1, 1.0f - Visibility, 1.0f - Visibility);
		} else {
			this.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

    protected override Vector3 getTipTextOffset()
    {
        return new Vector3(0, 0, 0);
    }
}
