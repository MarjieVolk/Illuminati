using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Graph.Edges
{
    public class EdgeGUI : Tooltippable
    {
        // *****
        // Unity Editor Configuration
        // *****

        public GameObject arrowHead;
        public GameObject ex;

        // *****
        // UI State
        // *****
        private GameObject realArrowHead;
        private GameObject realEx;
        private EdgeData.EdgeDirection prevDirection;
        public EdgeData Edge;

        protected void Awake()
        {
            prevDirection = EdgeData.EdgeDirection.Neutral;
        }

        protected override void Start()
        {
            base.Start();
            Edge = GetComponent<EdgeData>();
            TurnController.OnTurnEnd += updateVisibilityRendering;
            VisibilityController.instance.VisibilityChanged += new VisibilityController.VisibilityChangeHandler(updateArrowHead);
        }

        // Update is called once per frame
        void Update()
        {
            if (realEx == null) realEx = (GameObject)Instantiate(ex, gameObject.transform.position, gameObject.transform.rotation);
            realEx.SetActive(Edge.direction == EdgeData.EdgeDirection.Unusable);
        }

        public override bool viewAsOwned(Player.VisibilityController.Visibility visibility)
        {
            bool isPrivate = visibility == VisibilityController.Visibility.Private;
            return isPrivate && Edge.getOwner() == TurnController.CurrentPlayer;
        }

        private void updateArrowHead(VisibilityController.Visibility vis)
        {
            if (realArrowHead == null)
            {
                realArrowHead = (GameObject)Instantiate(arrowHead, transform.position, Quaternion.identity);

                Vector2 nodeOnePosition = new Vector2(Edge.nodeOne.transform.position.x, Edge.nodeOne.transform.position.y);
                Vector2 nodeTwoPosition = new Vector2(Edge.nodeTwo.transform.position.x, Edge.nodeTwo.transform.position.y);
                Vector2 edgeVector = nodeTwoPosition - nodeOnePosition;
                float edgeAngle = Vector2.Angle(Vector2.right, edgeVector);
                if (Vector3.Cross(Vector2.right, edgeVector).z < 0)
                {
                    edgeAngle *= -1;
                }
                realArrowHead.transform.Rotate(Vector3.forward, edgeAngle);
            }

            realArrowHead.SetActive(viewAsOwned(vis));

            EdgeData.EdgeDirection simplePrev = (prevDirection == EdgeData.EdgeDirection.TwoToOne ? EdgeData.EdgeDirection.TwoToOne : EdgeData.EdgeDirection.OneToTwo);
            EdgeData.EdgeDirection simpleDir = (Edge.direction == EdgeData.EdgeDirection.TwoToOne ? EdgeData.EdgeDirection.TwoToOne : EdgeData.EdgeDirection.OneToTwo);
            if (simplePrev != simpleDir)
            {
                realArrowHead.transform.Rotate(Vector3.forward, 180);
                prevDirection = Edge.direction;
            }

            updateVisibilityRendering();
        }

        private void updateVisibilityRendering()
        {
            if (VisibilityController.instance.visibility == VisibilityController.Visibility.Public)
            {
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1.0f - Edge.Visibility, 1.0f - Edge.Visibility);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        protected override Vector3 getTipTextOffset()
        {
            return new Vector3(0, 0, 0);
        }
    }
}
