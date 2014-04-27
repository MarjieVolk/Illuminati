using UnityEngine;
using System.Collections;

public abstract class Targetable : Highlightable {
	
	public delegate void OnClickHandler();
	public event OnClickHandler OnClicked;
	
	void OnMouseUpAsButton() {
		if (OnClicked != null) OnClicked();
	}
}
