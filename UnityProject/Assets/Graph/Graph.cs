using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour {
	
	public static Graph instance { get; private set; }

	private Dictionary<NodeData, List<EdgeData>> graph;

	// Use this for initialization
	void Start () {
		instance = this;
		graph = new Dictionary<NodeData, List<EdgeData>>();
		EdgeData[] edges = Object.FindObjectsOfType<EdgeData>();
		foreach (EdgeData edge in edges) {
			addEdge(edge.nodeOne.GetComponent<NodeData>(), edge);
			addEdge(edge.nodeTwo.GetComponent<NodeData>(), edge);
		}
	}

	private void addEdge(NodeData node, EdgeData edge) {
		if (!graph.ContainsKey(node)) {
			graph[node] = new List<EdgeData>();
		}
		graph[node].Add(edge);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<EdgeData> getConnectedEdges(NodeData node) {
		return graph[node];
	}

	/// <summary>
	/// Gets nodes connected to this node by any edge type (even neutral edges)
	/// </summary>
	/// <returns>The connected nodes.</returns>
	/// <param name="node">Node.</param>
	public List<NodeData> getConnectedNodes(NodeData node) {
		List<EdgeData> edges = getConnectedEdges(node);
		List<NodeData> nodes = new List<NodeData>();
		foreach (EdgeData edge in edges) {
			if (edge.nodeOne.GetComponent<NodeData>() == node) {
				nodes.Add(edge.nodeTwo.GetComponent<NodeData>());
			} else if (edge.nodeTwo.GetComponent<NodeData>() == node) {
				nodes.Add(edge.nodeOne.GetComponent<NodeData>());
			} else {
				Debug.LogError("Edge listed as connected to Node, but it is not -- one:" + edge.nodeOne + ", two:" + edge.nodeTwo + ", desired:" + node);
			}
		}
		return nodes;
	}

	/// <summary>
	/// Gets nodes that are being influenced by this node
	/// </summary>
	/// <returns>The influenced nodes.</returns>
	/// <param name="node">Node.</param>
	public List<NodeData> getInfluencedNodes(NodeData node) {
		List<EdgeData> edges = getConnectedEdges(node);
		List<NodeData> nodes = new List<NodeData>();
		foreach (EdgeData edge in edges) {
			if (getFrom(edge) == node) {
				nodes.Add(getTo(edge));
			}
		}
		return nodes;
	}

	/// <summary>
	/// Gets nodes that are influencing this node
	/// </summary>
	/// <returns>The influencing nodes.</returns>
	/// <param name="node">Node.</param>
	public List<NodeData> getInfluencingNodes(NodeData node) {
		List<EdgeData> edges = getConnectedEdges(node);
		List<NodeData> nodes = new List<NodeData>();
		foreach (EdgeData edge in edges) {
			if (getTo(edge) == node) {
				nodes.Add(getFrom(edge));
			}
		}
		return nodes;
	}

	/// <summary>
	/// Gets nodes connected to this node by neutral edges (the nodes themselves are not necessarily neutral)
	/// </summary>
	/// <returns>The neutral connected nodes.</returns>
	/// <param name="node">Node.</param>
	public List<NodeData> getNeutralConnectedNodes(NodeData node) {
		List<EdgeData> edges = getConnectedEdges(node);
		List<NodeData> nodes = new List<NodeData>();
		foreach (EdgeData edge in edges) {
			if (edge.direction == EdgeData.EdgeDirection.Neutral) {
				NodaData other = edge.nodeOne;
				if (node == other) {
					other = edge.nodeTwo;
				}
				nodes.Add(other);
			}
		}
		return nodes;
	}

	public NodeData getFrom(EdgeData edge) {
		if (EdgeData.EdgeDirection.OneToTwo == edge.direction) {
			return edge.nodeOne.GetComponent<NodeData>();
		} else if (EdgeData.EdgeDirection.TwoToOne == edge.direction) {
			return edge.nodeTwo.GetComponent<NodeData>();
		} else {
			return null;
		}
	}

	public NodeData getTo(EdgeData edge) {
		if (EdgeData.EdgeDirection.OneToTwo == edge.direction) {
			return edge.nodeTwo.GetComponent<NodeData>();
		} else if (EdgeData.EdgeDirection.TwoToOne == edge.direction) {
			return edge.nodeOne.GetComponent<NodeData>();
		} else {
			return null;
		}
	}
}
