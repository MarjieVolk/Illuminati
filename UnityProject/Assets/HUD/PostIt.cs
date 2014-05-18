using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Eppy;

public class PostIt : MonoBehaviour {

    private const float SNAP_DISTANCE = 0.2f;
    private const float NODE_OFFSET_X = -0.5f;
    private const float NODE_OFFSET_Y = 0;
    private const float NODE_OFFSET_Z = -1;
    private static Vector3 NODE_OFFSET;

    private delegate void PositionChangeHandler(NodeData node, PostIt owner);
    private static event PositionChangeHandler PositionChanged;

    private bool isDrag = true;
    private NodeData currentNode = null;
    private VisibilityController.VisibilityChangeHandler visHandler;
    private PositionChangeHandler positionHandler;
    private PlayerData owner;

	// Use this for initialization
	void Start () {
        NODE_OFFSET = new Vector3(NODE_OFFSET_X, NODE_OFFSET_Y, NODE_OFFSET_Z);
        owner = TurnController.instance.CurrentPlayer;

        this.gameObject.SetActive(VisibilityController.instance.visibility == VisibilityController.Visibility.Private);
        visHandler = (VisibilityController.Visibility vis) => {
            this.gameObject.SetActive(vis == VisibilityController.Visibility.Private && TurnController.instance.CurrentPlayer == owner);
        };
        VisibilityController.instance.VisibilityChanged += visHandler;

        positionHandler = (NodeData node, PostIt claimant) => {
            if (node == currentNode && claimant != this) {
                destroySelf();
            }
        };
        PositionChanged += positionHandler;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && isDrag) {
            if (endDrag()) return;
        }

        if (isDrag) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] overlapping = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y), SNAP_DISTANCE);
            currentNode = null;
            float closestDistance = -1;
            foreach (Collider2D collider in overlapping) {
                NodeData node = collider.gameObject.GetComponent<NodeData>();
                if (node != null) {
                    float distance = (this.transform.position - node.transform.position).magnitude;
                    if (currentNode == null || (distance < closestDistance && distance < SNAP_DISTANCE)) {
                        currentNode = node;
                        closestDistance = distance;
                    }
                }
            }

            if (currentNode != null) {
                pos = currentNode.transform.position;
            }

            pos.z = -5;
            transform.position = pos;
        }
	}

    void OnMouseDown() {
        isDrag = true;
    }

    void OnMouseUp() {
        if (isDrag) endDrag();
    }

    private bool endDrag() {
        if (currentNode == null) {
            destroySelf();
            return true;
        }

        PositionChanged(currentNode, this);
        isDrag = false;
        return false;
    }

    private void destroySelf() {
        VisibilityController.instance.VisibilityChanged -= visHandler;
        PositionChanged -= positionHandler;
        Destroy(gameObject);
    }
}
