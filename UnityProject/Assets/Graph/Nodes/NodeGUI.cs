using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Graph.Nodes
{
    public class NodeGUI : Tooltippable
    {
        private NodeData Node;

        protected override void Start() {
            base.Start();
            Node = GetComponent<NodeData>();
            VisibilityController.instance.VisibilityChanged += (VisibilityController.Visibility visibility) => {
                bool inRange = true;
                if (visibility == VisibilityController.Visibility.Private) {
                    inRange = turnController.CurrentPlayer == Node.Owner;
                    if (!inRange) {
                        foreach (NodeData adjacent in graphUtility.getConnectedNodes(Node)) {
                            if (turnController.CurrentPlayer == adjacent.Owner) {
                                inRange = true;
                                break;
                            }
                        }
                    }
                }

                if (inRange) {
                    // Show normally
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                } else {
                    // Grey out
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            };
        }

        public override bool viewAsOwned(VisibilityController.Visibility visibility)
        {
            bool isPrivate = visibility == VisibilityController.Visibility.Private;
            return isPrivate && Node.Owner == turnController.CurrentPlayer;
        }

        protected override Vector3 getTipTextOffset()
        {
            return new Vector3(0, GetComponent<CircleCollider2D>().radius, 0);
        }
    }
}
