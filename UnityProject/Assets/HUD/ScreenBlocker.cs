using UnityEngine;
using System.Collections;

public class ScreenBlocker : MonoBehaviour {

    public static ScreenBlocker instance { get; private set; }

    private int requestCounter = 0;

	// Use this for initialization
	void Start () {
        instance = this;
        gameObject.collider2D.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setBlocking(bool block) {
        if (block) requestCounter++; else requestCounter--;
        gameObject.collider2D.enabled = requestCounter > 0;
    }
}
