using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphUtility : MonoBehaviour {
	
	public static GraphUtility instance { get; private set; }

	private Dictionary<NodeData, List<EdgeData>> graph;

	// Use this for initialization
	void Awake () {
		instance = this;
	}

    void Start()
    {
        graph = new Dictionary<NodeData, List<EdgeData>>();
        EdgeData[] edges = Object.FindObjectsOfType<EdgeData>();
        foreach (EdgeData edge in edges)
        {
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

	public EdgeData getConnectingEdge(NodeData one, NodeData two) {
		List<EdgeData> thisEdges = GraphUtility.instance.getConnectedEdges(one);
		foreach (EdgeData edge in thisEdges) {
			if (edge.nodeOne == two.gameObject || edge.nodeTwo == two.gameObject) {
				return edge;
			}
		}
		return null;
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
				NodeData other = edge.nodeOne.GetComponent<NodeData>();
				if (node == other) {
					other = edge.nodeTwo.GetComponent<NodeData>();
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

    public HashSet<EdgeData> getEdgesBetweenNodes(NodeData source, NodeData target)
    {
        HashSet<EdgeData> upstreamEdges = getEdgesReachableFrom(source, false);
        HashSet<EdgeData> downstreamEdges = getEdgesReachableFrom(target, true);

        upstreamEdges.IntersectWith(downstreamEdges);

        return upstreamEdges;
    }

    public HashSet<EdgeData> getEdgesReachableFrom(NodeData origin, bool reverseEdgeDirection)
    {
        HashSet<EdgeData> ret = new HashSet<EdgeData>();

        recGetEdgesReachableFrom(origin, reverseEdgeDirection, ret);

        return ret;
    }

    private void recGetEdgesReachableFrom(NodeData origin, bool reverseEdgeDirection, HashSet<EdgeData> visited)
    {
        List<NodeData> children;
        if (!reverseEdgeDirection) children = getInfluencedNodes(origin);
        else children = getInfluencingNodes(origin);

        foreach (NodeData child in children)
        {
            EdgeData connectingEdge = getConnectingEdge(origin, child);
            //only visit each thing once
            if (visited.Contains(connectingEdge)) continue;

            visited.Add(connectingEdge);
            recGetEdgesReachableFrom(child, reverseEdgeDirection, visited);
        }
    }
}
