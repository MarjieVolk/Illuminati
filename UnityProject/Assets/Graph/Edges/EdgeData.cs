using UnityEngine;
using System.Collections;

public class EdgeData : Highlightable {

	public GameObject nodeOne;
	public GameObject nodeTwo;

	public DominationType type {get; private set;}
	public EdgeDirection direction {get; private set;}

	float visibility;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		direction = EdgeDirection.Neutral;
		type = DominationType.Bribe;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public enum EdgeDirection {
		OneToTwo, TwoToOne, Neutral
	};
}
