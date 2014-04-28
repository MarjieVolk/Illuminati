﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TestWindow : EditorWindow {

	private static NodeData prev;

	[MenuItem("Window/Test")]
	public static void Init() {
		TestWindow window = GetWindow<TestWindow>();
		window.title = "Test";
		SceneView.onSceneGUIDelegate += OnScene;
	}

	private static void OnScene(SceneView sceneView) {
		Event e = Event.current;

		if (e.control) {
			GameObject obj = (GameObject) Selection.activeObject;
			if (obj == null) {
				prev = null;
				return;
			}

			NodeData cur = obj.GetComponent<NodeData>();

			if (prev != null && cur != prev) {
				Object edgePrefab = Resources.Load("Edge");
				GameObject edge = (GameObject) PrefabUtility.InstantiatePrefab(edgePrefab);

				EdgeData edgeData = edge.GetComponent<EdgeData>();
				edgeData.nodeOne = prev.gameObject;
				edgeData.nodeTwo = cur.gameObject;

                //position the edge
                //get the position of each node
                Vector2 nodeOnePosition = new Vector2(edgeData.nodeOne.transform.position.x, edgeData.nodeOne.transform.position.y);
                Vector2 nodeTwoPosition = new Vector2(edgeData.nodeTwo.transform.position.x, edgeData.nodeTwo.transform.position.y);

                //TODO adjust for pin location
                
                //move the center of the edge to the average of their positions
                Vector2 avgPosition = (nodeOnePosition + nodeTwoPosition) / 2.0f;
                edge.transform.position = avgPosition;
                
                //get a vector from one position to the other
                Vector2 edgeVector = nodeTwoPosition - nodeOnePosition;

                //scale the edge
                float edgeWidth = edge.GetComponent<BoxCollider2D>().size.x * edge.transform.localScale.x;
                float distance = edgeVector.magnitude;
                float scaleFactor = edgeWidth / distance;
                Vector3 scale = edge.transform.localScale;
                edge.transform.localScale = new Vector3(scale.x / scaleFactor, scale.y, scale.z);
                Debug.Log("Width: " + edgeWidth + ", Distance: " + distance + " scaleFactor: " + scaleFactor);

                //rotate the edge to be parallel to that vector
                float edgeAngle = Vector2.Angle(Vector2.right, edgeVector);
                if (Vector3.Cross(Vector2.right, edgeVector).z < 0)
                {
                    edgeAngle *= -1;
                }
                edge.transform.Rotate(Vector3.forward, edgeAngle);

				prev = null;
			}

		} else {
			GameObject obj = (GameObject) Selection.activeObject;
			if (obj != null) {
				prev = obj.GetComponent<NodeData>();
			}
		}
	}

	void OnDestroy() {
		SceneView.onSceneGUIDelegate -= OnScene;
	}
}
