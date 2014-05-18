using Assets.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class RemainingActionsDisplayer : MonoBehaviour
    {
        public GUISkin skin;

        void OnGUI()
        {
            if (TurnController.instance == null) return;

            GUI.skin = skin;

            int actionPoints = TurnController.instance.CurrentPlayer.actionPointsRemaining();
            string text = "  Actions: <b>" + actionPoints + "</b>";

            Vector2 size = GUI.skin.FindStyle("Small-Box").CalcSize(new GUIContent(text));
            GUI.Box(new Rect(-10, (Screen.height * 0.885f), size.x + 20, size.y), text, GUI.skin.FindStyle("Small-Box"));
        }
    }
}
