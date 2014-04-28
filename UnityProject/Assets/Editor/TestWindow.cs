using UnityEngine;
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
				edgeData.nodeOne = prev;
				edgeData.nodeTwo = cur;

				// Here goes the stuff

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
