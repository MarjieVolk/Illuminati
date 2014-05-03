using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TestWindow : EditorWindow {

	private static NodeData prev;
    private static GraphUtility util;

    private static Dictionary<string, Vector2> pinOffsets = new Dictionary<string, Vector2>
        {
            {"corporate", new Vector2(-0.05f, 0.7f)},
            {"government", new Vector2(-0.1f, 0.7f)},
            {"media", new Vector2(-0.45f, 0.3f)}
        };

	[MenuItem("Window/Enlightened Graph")]
	public static void Init() {
		TestWindow window = GetWindow<TestWindow>();
		window.title = "Enlightened Graph";
        util = new GraphUtility();
        util.Awake();
	}

	void OnEnable() {
		SceneView.onSceneGUIDelegate += OnScene;
        if (util == null) {
            util = new GraphUtility();
            util.Awake();
        }
	}
	
	void OnDisable() {
		SceneView.onSceneGUIDelegate -= OnScene;
	}
	
	void OnGUI() {
        bool all = GUILayout.Button("Update All");

        EditorGUILayout.Space();
        
        bool edgePositions = GUILayout.Button("Update Edge Positions");
        bool edgeNames = GUILayout.Button("Update Edge Names");
        bool nodeNames = GUILayout.Button("Update Node Names");

        if (edgePositions || all) {
            updateEdgePositions();
        }

        if (nodeNames || all) {
            updateNodeNames();
        }

        if (edgeNames || all) {
            updateEdgeNames();
        }
	}
	
	private static void OnScene(SceneView sceneView) {
		Event e = Event.current;
		GameObject obj = (GameObject) Selection.activeObject;
		NodeData cur = obj == null ? null : obj.GetComponent<NodeData>();

		if (e.control) {
			if (obj == null) {
				prev = null;
				return;
			}

			if (prev != null && cur != null && cur != prev) {
				UnityEngine.Object edgePrefab = Resources.Load("Edge");
				GameObject edge = (GameObject) PrefabUtility.InstantiatePrefab(edgePrefab);

				EdgeData edgeData = edge.GetComponent<EdgeData>();
				edgeData.nodeOne = prev.gameObject;
				edgeData.nodeTwo = cur.gameObject;

                positionEdge(edgeData);
                nameEdge(edgeData);

				prev = null;
			}

		} else {
			if (obj != null) {
				prev = obj.GetComponent<NodeData>();
			}
		}

        foreach (GameObject obje in Selection.gameObjects) {
            NodeData node = obje.GetComponent<NodeData>();
            if (node != null) {
                foreach (EdgeData edge in util.getConnectedEdges(node)) {
                    positionEdge(edge);
                }
            }
        }
	}

	void OnDestroy() {
		SceneView.onSceneGUIDelegate -= OnScene;
	}

    private static void updateEdgePositions() {
        foreach (EdgeData edge in UnityEngine.Object.FindObjectsOfType<EdgeData>()) {
            positionEdge(edge);
        }
    }

    private static void updateNodeNames() {
        Dictionary<string, int> names = new Dictionary<string, int>();

        foreach (NodeData node in UnityEngine.Object.FindObjectsOfType<NodeData>()) {
            string[] pieces = node.gameObject.name.Split('-');
            int n;
            if (int.TryParse(pieces[pieces.Length - 1], out n)) {
                // node name ends with "-" and number: remove that shit
                node.gameObject.name = node.gameObject.name.Substring(0, node.gameObject.name.Length - (pieces[pieces.Length - 1].Length + 1));
            }

            if (!names.ContainsKey(node.gameObject.name)) {
                names[node.gameObject.name] = 0;
            }

            int uses = names[node.gameObject.name];
            names[node.gameObject.name] = uses + 1;
            node.gameObject.name += "-" + uses;
        }
    }

    private static void updateEdgeNames() {
        foreach (EdgeData edge in UnityEngine.Object.FindObjectsOfType<EdgeData>()) {
            nameEdge(edge);
        }
    }

    private static void nameEdge(EdgeData edge) {
        edge.gameObject.name = edge.nodeOne.name + " => " + edge.nodeTwo.name;
    }

    private static void positionEdge(EdgeData edgeData) {
        float zValue = edgeData.gameObject.transform.position.z;

        //get the position of each node
        Vector2 nodeOnePosition = new Vector2(edgeData.nodeOne.transform.position.x, edgeData.nodeOne.transform.position.y);
        Vector2 nodeTwoPosition = new Vector2(edgeData.nodeTwo.transform.position.x, edgeData.nodeTwo.transform.position.y);

        //adjust for pin location
        Vector2 nodeOneOffset = pinOffsets[edgeData.nodeOne.GetComponent<NodeData>().archetype];
        Vector2 nodeTwoOffset = pinOffsets[edgeData.nodeTwo.GetComponent<NodeData>().archetype];

        nodeOnePosition += nodeOneOffset;
        nodeTwoPosition += nodeTwoOffset;

        //move the center of the edge to the average of their positions
        Vector2 avgPosition = (nodeOnePosition + nodeTwoPosition) / 2.0f;
        edgeData.gameObject.transform.position = avgPosition;

        //get a vector from one position to the other
        Vector2 edgeVector = nodeTwoPosition - nodeOnePosition;

        //scale the edge
        float edgeWidth = edgeData.gameObject.GetComponent<BoxCollider2D>().size.x * edgeData.gameObject.transform.localScale.x;
        float distance = edgeVector.magnitude;
        float scaleFactor = edgeWidth / distance;
        Vector3 scale = edgeData.gameObject.transform.localScale;
        edgeData.gameObject.transform.localScale = new Vector3(scale.x / scaleFactor, scale.y, scale.z);
        Debug.Log("Width: " + edgeWidth + ", Distance: " + distance + " scaleFactor: " + scaleFactor);

        //rotate the edge to be parallel to that vector
        float edgeAngle = Vector2.Angle(Vector2.right, edgeVector);
        if (Vector3.Cross(Vector2.right, edgeVector).z < 0) {
            edgeAngle *= -1;
        }

        edgeData.gameObject.transform.rotation = Quaternion.identity;
        edgeData.gameObject.transform.Rotate(Vector3.forward, edgeAngle);

        edgeData.gameObject.transform.position = new Vector3(edgeData.gameObject.transform.position.x, edgeData.gameObject.transform.position.y, zValue);
    }
}
