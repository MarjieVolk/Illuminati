using UnityEngine;
using System.Collections;
using Assets.HUD;

public class EscapeMenu : MonoBehaviour {

    public GUISkin skin;

    private bool show = false;
    private Instructions instruct;

	// Use this for initialization
	void Start () {
        instruct = gameObject.GetComponent<Instructions>();
        instruct.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape)) {
            show = true;
            ScreenBlocker.instance.setBlocking(true);
        }
	}

    void OnGUI() {
        if (!show) return;

        GUI.skin = skin;
        GUI.Window(0, GUIUtilities.getRect(250, 250), layoutWindow, "Menu");
    }

    private void layoutWindow(int id) {
        if (GUILayout.Button("Continue")) {
            show = false;
            ScreenBlocker.instance.setBlocking(false);
        }

        if (GUILayout.Button("Instructions")) {
            show = false;
            instruct.enabled = true;
        }

        if (GUILayout.Button("Exit")) {
            Application.Quit();
        }
    }
}
