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
        }

        public override bool viewAsOwned(Player.VisibilityController.Visibility visibility)
        {
            bool isPrivate = visibility == VisibilityController.Visibility.Private;
            return isPrivate && Node.Owner == TurnController.instance.CurrentPlayer;
        }

        protected override Vector3 getTipTextOffset()
        {
            return new Vector3(0, GetComponent<CircleCollider2D>().radius, 0);
        }
    }
}
