using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class PostItPad : Highlightable {

    private static int COUNT = 0;
    private const float MARGIN = 5;
    private const float WIDTH = 50;

    public PostIt toGenerate;
    private int index;

    protected override void Start() {
        base.Start();
        index = COUNT;
        COUNT++;

        this.gameObject.SetActive(VisibilityController.instance.visibility == VisibilityController.Visibility.Private);
        VisibilityController.instance.VisibilityChanged += (VisibilityController.Visibility vis) => {
            this.gameObject.SetActive(vis == VisibilityController.Visibility.Private);
        };
    }

    void Update() {
        setScreenPosition(getPos(index));
    }

    void OnMouseDown() {
        GameObject postit = (GameObject) Instantiate(toGenerate.gameObject, transform.position, Quaternion.identity);
        postit.transform.position = this.transform.position;
    }

    private Vector3 getPos(int index) {
        return new Vector3(MARGIN * (index + 1) + (WIDTH * (index + 0.5f)), Screen.height - (MARGIN + (WIDTH / 2.0f)), 0);
    }

    private void setScreenPosition(Vector3 screenPos) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, -5);
    }

    public override bool viewAsOwned(VisibilityController.Visibility visibility) {
        return false;
    }

    void OnDestroy() {
        COUNT = 0;
    }
}
