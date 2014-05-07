using UnityEngine;
using System.Collections;
using Assets.HUD;
using Assets.Player;

public class Instructions : MonoBehaviour {

    public GUISkin skin;

    private Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, transform.position.y, - 12);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        GUI.skin = skin;

        Rect windowBox = GUIUtilities.getRect(Screen.width * 0.6f, Screen.height * 0.9f);
        GUI.Box(windowBox, "Welcome to Enlightened!");

        float margin = windowBox.height * 0.07f;
        Rect textBox = new Rect(windowBox.x + margin, windowBox.y + 60, windowBox.width - (margin * 2), windowBox.height * 0.75f);

        float height = skin.FindStyle("Label").CalcHeight(new GUIContent(text), textBox.width - 20);
        Rect insideBox = new Rect(0, 0, textBox.width - 20, height);

        scrollPosition = GUI.BeginScrollView(textBox, scrollPosition, insideBox);
		GUI.Label(insideBox, text);
        GUI.EndScrollView();

		float buttonHeight = 40;
        float buttonYBase = windowBox.y + windowBox.height - buttonHeight;

        if (GUI.Button(new Rect((Screen.width / 2.0f) - 50, buttonYBase - (windowBox.height * 0.05f), 100, buttonHeight), "Okay")) {
            if (ScreenBlocker.instance != null) ScreenBlocker.instance.setBlocking(false);
            enabled = false;
		}
	}

    private string text = "In this game you will play as a secret society attempting to gain control over as many puppets as you can, and eventually overpower your rivals!  You will start with only a few puppets, and acquire more by means of bribery, blackmail, and threats.  Once you eliminate all your rivals, you win!\n\n<b>Connections</b>\n\nYou send orders to your puppets by means of connections.  On the map, connections appear as grey or red lines between two puppets.  You must perform actions across these connections to gain control over more puppets and send orders across long chains of connections to instruct more distant puppets.  The more you utilize connections you control, the more visible and suspicious they will appear to the public!\n\n<b>Turn Overview</b>\n\nOn your turn, you will queue up actions, execute all queued actions and see the results, and finally end your turn.  When you end your turn, the game will switch to Surface view.  If you are playing against a human component, do not switch to Enlightened view on your rival's turn, or you will see where their puppets are!  Look away from the screen throughout your rival's turn.\n\n<b>Puppet Classes</b>\n\nThere are three classes of puppet - Corporate, Media, and Government.  Each class is strong in one area of attack, is weak in one area of defence, and has a special action.  Corporate can Invest, giving you additional actions to use on a later turn.  Media can Investigate a connection, dramatically increasing the visibility of the connection, especially if it belongs to your rival.  Government can Stake Out a connection, possibly rendering it unusable for several turns.\n\n<b>Surface and Enlightened Views</b>\n\nYou may view the Enlightened map via one of two perspectives: the Surface view, and the Enlightened view.  The Surface view shows the map as seen by the public, and the Enlightened view shows it as seen by you.  Click the All-Seeing Eye in the top right to switch between views.  The Enlightened view shows puppets that you control, and the connections that connect them (but never your rivals' puppets).  The Surface view does not show any puppet ownership.  However, if there is activity along connections between puppets, those connections will begin to appear red in Surface view - that is, they have a higher visibility.  If your rival is not careful, they may reveal their location to you by allowing their activity to become too visible!  More visible edges are also susceptible to government Stake Outs.\n\n<b>Actions</b>\n\n<i><color=maroon>Bribe</color></i>		            Attempt to gain control over another puppet via bribery.\n<i><color=maroon>Blackmail</color></i>  	    Attempt to gain control over another puppet via blackmail.\n<i><color=maroon>Threat</color></i>		        Attempt to gain control over another puppet via threats.\n<i><color=maroon>Instruct</color></i>	    	    Permanently increase the stats of another puppet you control.  The amount of increase is determined by the difference between the stats of the instructing and instructed puppet.  Also reduce the rate at which the connection between these two puppets becomes visible.\n<i><color=maroon>Assist</color></i>		            Temporarily increase the stats of another puppet you control.  The amount of increase is determined only by the stats of the assisting puppet.  Also reduce the visibility of the connection between these two puppets.\n<i><color=maroon>Invest</color></i>		            Gives two additional actions in two turns\n<i><color=maroon>Investigate</color></i>	    Increase the visibility of a connection.  The increase tends to be significantly higher if the connection is controlled by a rival society.\n<i><color=maroon>Stakeout</color></i>		    Attempt to break a connection, rendering it unusable by anyone for several turns.  More visible edges are more susceptible.";
}
