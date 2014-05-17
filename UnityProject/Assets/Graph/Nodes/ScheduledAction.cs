using UnityEngine;
using System.Collections;
using Assets.Player;

public class ScheduledAction : Highlightable {

    private const float WIDTH = 50;
    private const float MARGIN = 5;
    private const float MAX_Y = MARGIN + WIDTH;
    private const float MIN_Y = MARGIN;

	public Action action;
	public PlayerData player;

	private ScheduledAction sister;

    private bool dragable = false;
    private bool isDrag = false;
    private float dragStartTime = 0;
    private float dragEndTime = 0;

	public override bool viewAsOwned(VisibilityController.Visibility visibility) {
		return false;
	}

	public void setSister(ScheduledAction other) {
		other.sister = this;
		this.sister = other;
	}

    public void setDragable(bool isDragable) {
        dragable = isDragable;
    }

	void OnMouseUpAsButton() {
        if (!dragable || (isDrag == false && dragEndTime != Time.time) || Time.time - dragStartTime < 0.3f) {
		    // Cancel action
            clearHighlights();
            isDrag = false;
		    player.cancelAction(action);
        }
	}

	void OnMouseEnter() {
        if (!isDrag) showHighlights();
	}

	void OnMouseExit() {
        if (!isDrag) clearHighlights();
	}

    void OnMouseDown() {
        isDrag = dragable;
        dragStartTime = Time.time;
    }

    void OnMouseUp() {
        isDrag = false;
        dragEndTime = Time.time;
        clearHighlights();
    }

    void Update() {
        if (isDrag) {
            Vector3 newPos = Input.mousePosition;
            newPos.y = Mathf.Min(MAX_Y, Mathf.Max(MIN_Y, newPos.y));
            newPos.x = Mathf.Min((MARGIN + WIDTH) * player.nScheduledActions(), Mathf.Max(MIN_Y, newPos.x));
            setScreenPosition(newPos);

            int desiredIndex = (int) Mathf.Floor(newPos.x / (MARGIN + WIDTH));
            player.setActionIndex(action, desiredIndex);
        }
    }

    public void updateSelfAsListTag(int index) {
        if (isDrag) return;
        Vector3 screenPos = new Vector3(MARGIN * (index + 1) + (WIDTH * (index + 0.5f)), MARGIN + (WIDTH / 2.0f), 0);
        setScreenPosition(screenPos);
    }

    private void setScreenPosition(Vector3 screenPos) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, -5);
    }

    private void showHighlights() {
        setHighlighted(true);
        sister.setHighlighted(true);
        if (action.Target != null) {
            action.Target.setHighlighted(true);
            action.Target.showTargetInfoText(action.getAdditionalTextForTarget(action.Target));
        }
    }

    private void clearHighlights() {
        setHighlighted(false);
        sister.setHighlighted(false);
        if (action.Target != null) {
            action.Target.setHighlighted(false);
            action.Target.hideTargetInfoText();
        }
    }
}
