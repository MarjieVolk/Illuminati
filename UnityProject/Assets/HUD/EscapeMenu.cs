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
        }

        if (GUILayout.Button("Instructions")) {
            instruct.enabled = true;
            show = false;
        }

        if (GUILayout.Button("Exit")) {
            Application.Quit();
        }
    }
}
