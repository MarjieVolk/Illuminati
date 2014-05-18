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
        private GUIStyle style;

        private void Awake()
        {
            style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.fontSize = 16;
            style.normal.background = InvestigateAction.MakeTextureOfColor(Color.gray);
        }

        void OnGUI()
        {
            if (TurnController.instance == null) return;

            int actionPoints = TurnController.instance.CurrentPlayer.actionPointsRemaining();
            GUI.Label(new Rect(0, Screen.height - (Screen.height * 0.115f), 85, 20), "  Actions: <b>" + actionPoints + "</b>", style);
        }
    }
}
