using UnityEngine;
using System.Collections;
using Assets.Graph.Nodes;

public abstract class Targetable : Highlightable {
	
	public delegate void OnClickHandler();
	public event OnClickHandler OnClicked;

	private bool isShowText = false;
	private string text = "";
	private GUIStyle style;

	protected override void Start () {
		base.Start();

		style = new GUIStyle();
		style.normal.textColor = new Color(0.5f, 0, 0);
		style.fontSize = 12;
		style.fontStyle = FontStyle.Bold;
		style.alignment = TextAnchor.LowerCenter;
        style.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.9f));
	}

	void OnMouseUpAsButton() {
		if (OnClicked != null) OnClicked();
	}

	public void showTargetInfoText(string text) {
		isShowText = true;
		this.text = text;
	}

	public void hideTargetInfoText() {
		isShowText = false;
	}

	void OnGUI() {
		if (isShowText) {
            Vector2 textSize = style.CalcSize(new GUIContent(text));
            Vector3 worldPosition = transform.position;
            worldPosition.y += GetComponent<CircleCollider2D>().radius;
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
			GUI.Label(new Rect(screenPosition.x - textSize.x / 2.0f, Screen.height - screenPosition.y - textSize.y, textSize.x, textSize.y), text, style);
		}
	}
}
