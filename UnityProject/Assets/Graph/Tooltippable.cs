using Assets.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Graph
{
    public abstract class Tooltippable : Highlightable
    {
        private bool isShowText = false;
        private string text = "";
        private GUIStyle style;

        protected override void Start()
        {
            base.Start();

            style = new GUIStyle();
            style.normal.textColor = new Color(0.5f, 0, 0);
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.LowerCenter;
            style.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.7f));
        }

        public virtual void OnGUI()
        {
            if (isShowText)
            {
                Vector2 textSize = style.CalcSize(new GUIContent(text));
                Vector3 worldPosition = transform.position;
                worldPosition += getTipTextOffset();
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                GUI.Label(new Rect(screenPosition.x - textSize.x / 2.0f, Screen.height - screenPosition.y - textSize.y, textSize.x, textSize.y), text, style);
            }
        }

        public void showTargetInfoText(string text)
        {
            isShowText = true;
            this.text = text;
        }

        public void hideTargetInfoText()
        {
            isShowText = false;
        }

        /// <summary>
        /// The world space offset from the transform.position to the bottom middle of the desired text location
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 getTipTextOffset();
    }
}
