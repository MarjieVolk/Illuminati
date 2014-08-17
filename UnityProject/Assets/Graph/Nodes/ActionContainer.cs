using UnityEngine;
using System.Collections;

public class ActionContainer : MonoBehaviour {

    public Action[] actions;

	void Awake () {
	    foreach (Action a in actions) {
            Action realA = (Action) Instantiate(a, transform.position, Quaternion.identity);
            realA.transform.parent = this.transform;
        }
	}
}
