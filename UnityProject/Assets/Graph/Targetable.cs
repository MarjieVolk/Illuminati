using UnityEngine;
using System.Collections;

public abstract class Targetable : Highlightable {
	
	public delegate void OnClickHandler();
	public event OnClickHandler OnClicked;

	private bool isShowText = false;
	private string text = "";
	private GUIStyle style;

	protected override void Start () {
		base.Start();

		style = new GUIStyle();
		style.normal.textColor = Color.green;
		style.fontSize = 16;
		style.fontStyle = FontStyle.Bold;
		style.alignment = TextAnchor.LowerCenter;
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
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
			GUI.Label(new Rect(screenPosition.x - 150, Screen.height - screenPosition.y - 240, 300, 200), text, style);
		}
	}
}
