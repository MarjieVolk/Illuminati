using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

	private string playerOne = "Player 1";
	private string playerTwo = "Player 2";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		float textWidth = 600;
		float textHeight = 500;

		GUI.Label(new Rect((Screen.width / 2.0f) - (textWidth / 2.0f), 70, textWidth, textHeight), "<color=#000000ff>" + text + "</color>");
		
		float buttonWidth = 100;
		float buttonHeight = 40;

		playerOne = GUI.TextArea(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f) - 200, Screen.height - 180, buttonWidth, buttonHeight), playerOne);
		playerTwo = GUI.TextArea(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f) + 200, Screen.height - 180, buttonWidth, buttonHeight), playerTwo);

		if (GUI.Button(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f), Screen.height - 100, buttonWidth, buttonHeight), "Start!")) {
			GameObject.Find("Player1").GetComponent<PlayerData>().Name = playerOne;
			GameObject.Find("Player2").GetComponent<PlayerData>().Name = playerTwo;
			Destroy(gameObject);
		}
	}

	private string text = "Welcome to Enlightened!\n\nIn this game you will play as a secret society attempting to gain control over as many puppets as you can, and eventually overpower your rival!  (Currently Enlightened is for 2 players only).  You will start with one puppet, and acquire more by means of bribery, blackmail, and threats.  Once you eliminate your rival, you win!\n\nOn your turn, you will queue up actions, execute all queued actions and see the results, and finally end your turn.  Each puppet has a number of actions it can perform.  Bribe, Blackmail, and Threat actions allow them to gain control over more puppets that report back to you.  Instruct and Assist actions boost the capabilities of other puppets you already control, allowing them to gain control over puppets more efficiently.  There are three classes of puppet - Corporate, Media, and Government - and each class also has a special action.  Corporate can Invest, giving you additional actions to use on a later turn.  Media can Investigate a connection, dramatically increasing the visibility of the connection, especially if it belongs to your rival.  Government can Stake Out a connection, possibly rendering it unusable for several turns.\n\nYou may view the Enlightened map via one of two perspectives: the Surface view, and the Enlightened view.  The Surface view shows the map as seen by the public, and the Enlightened view shows it as seen by you.  Click the All-Seeing Eye in the top right to switch between views.  The Enlightened view shows puppets that you control, and the connections that connect them (but never your rival's puppets).  The Surface view does not show any puppet ownership.  However, if there is activity along connections between puppets, those connections will begin to appear dark in Surface view - that is, their visibility is increasing.  If your rival is not careful, they may reveal their location to you by allowing their activity to become too visible!\n\nWhen you end your turn, the game will switch to Surface view.  Do not switch to Enlightened view on your rival's turn, or you will see where their puppets are!  Look away from the screen throughout your rival's turn.";
}
