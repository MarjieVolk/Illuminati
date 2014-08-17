using Assets.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Graph;
using Assets.Graph.Edges;

namespace Assets.Player
{
    public class VisibilityDisplayer : DependencyResolvingComponent
    {
        private EdgeData hoveredEdgeData = null;
        private GUIStyle visibilityStyle;

        void Awake()
        {
            visibilityStyle = new GUIStyle();
            visibilityStyle.normal.textColor = Color.black;
            visibilityStyle.fontStyle = FontStyle.Bold;
            visibilityStyle.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.9f));
            visibilityStyle.alignment = TextAnchor.MiddleCenter;
        }

        void Start()
        {
            foreach (EdgeData edge in FindObjectsOfType<EdgeData>())
            {
                EdgeGUI guiObj = edge.gameObject.GetComponent<EdgeGUI>();
                EdgeData edgeCopy = edge; //so it can be captured in the closure
                guiObj.OnHover += () => hoveredEdgeData = edgeCopy;
                guiObj.OnEndHover += () => hoveredEdgeData = null;
            }
        }

        void OnGUI()
        {
            if (hoveredEdgeData != null)
            {
                float margin = 10;
                string text = "Visibility: " + (int)(hoveredEdgeData.Visibility * 100) + "%";

                bool isOwned = hoveredEdgeData.direction == EdgeData.EdgeDirection.OneToTwo || hoveredEdgeData.direction == EdgeData.EdgeDirection.TwoToOne;
                if (isOwned && hoveredEdgeData.nodeOne.GetComponent<NodeData>().Owner == turnController.CurrentPlayer)
                {
                    text += "\nIncrease Rate: " + Mathf.Round(100 * hoveredEdgeData.visIncreaseModifier) + "%";
                }
                Vector2 textSize = visibilityStyle.CalcSize(new GUIContent(text));

                GUI.Label(new Rect(Screen.width - textSize.x - margin - 5, Screen.height - textSize.y - margin - 2.5f, textSize.x + 10, textSize.y + 5), text, visibilityStyle);
            }
        }
    }
}
