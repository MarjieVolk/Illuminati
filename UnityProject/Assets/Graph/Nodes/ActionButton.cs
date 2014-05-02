using UnityEngine;
using System.Collections;
using Assets.Player;
using Assets.Graph.Nodes;

public class ActionButton : MonoBehaviour {

	private const float toolTipTime = 0.7f;

    public event Targetable.OnClickHandler OnClick;

    public Sprite normal, hover, selected;
	public string tooltip;

	private float mouseEnterTime = -1;
	private GUIStyle style;

    private bool _actionEnabled = true;

    public bool ActionEnabled
    {
        get
        {
            return _actionEnabled;
        }
        set
        {
            _actionEnabled = value;
            if (!_actionEnabled)
                GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            if (_actionEnabled)
                GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
		style = new GUIStyle();
		style.fontSize = 16;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.black;
        style.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (!ActionEnabled) return;
		if (mouseEnterTime != -1 && (Time.time - mouseEnterTime) >= toolTipTime) {
			// Show Tooltip
			GUI.depth = 1;
            string paddedTooltip = " " + tooltip;
            Vector2 tooltipSize = style.CalcSize(new GUIContent(paddedTooltip));
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 4, tooltipSize.x + 5.0f, tooltipSize.y), paddedTooltip, style);
		}
	}

	/// <summary>
	/// Returns the sprite to normal, regardless of situation
	/// </summary>
	public void clear() {
		this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
	}

    public void OnMouseEnter()
    {
        if (!ActionEnabled) return;
		mouseEnterTime = Time.time;
		if (!ActionController.instance.inSelectionState && hover != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = hover;
		}
	}

    public void OnMouseExit()
    {
        if (!ActionEnabled) return;
		mouseEnterTime = -1;
		if (!ActionController.instance.inSelectionState && normal != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
		}
	}

    public void OnMouseUpAsButton()
    {
        if (!ActionEnabled) return;
		if (ActionController.instance.inSelectionState) {
            ActionController.instance.clearSelectionState();
		}

		if (selected != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = selected;
		}

        if (null != OnClick) OnClick();
	}
}
