using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphUtility : MonoBehaviour {
	
	public static GraphUtility instance { get; private set; }

	private Dictionary<NodeData, List<EdgeData>> graph;

	// Use this for initialization
	void Awake () {
        instance = this;
        graph = new Dictionary<NodeData, List<EdgeData>>();
        EdgeData[] edges = Object.FindObjectsOfType<EdgeData>();
        foreach (EdgeData edge in edges)
        {
            addEdge(edge.nodeOne.GetComponent<NodeData>(), edge);
            addEdge(edge.nodeTwo.GetComponent<NodeData>(), edge);
        }
	}

    void Start()
    {
    }

    void Update()
    {

    }

	private void addEdge(NodeData node, EdgeData edge) {
		if (!graph.ContainsKey(node)) {
			graph[node] = new List<EdgeData>();
		}
		graph[node].Add(edge);
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

        recGetNodesAndEdgesReachableFrom(origin, reverseEdgeDirection, new HashSet<NodeData>(), ret);

        return ret;
    }

    public HashSet<NodeData> getNodesReachableFrom(NodeData origin, bool reverseEdgeDirection)
    {
        HashSet<NodeData> ret = new HashSet<NodeData>();

        ret.Add(origin);

        recGetNodesAndEdgesReachableFrom(origin, reverseEdgeDirection, ret, new HashSet<EdgeData>());

        return ret;
    }

    private void recGetNodesAndEdgesReachableFrom(NodeData origin, bool reverseEdgeDirection, HashSet<NodeData> visitedNodes, HashSet<EdgeData> visitedEdges)
    {
        List<NodeData> children;
        if (!reverseEdgeDirection) children = getInfluencedNodes(origin);
        else children = getInfluencingNodes(origin);
        foreach (NodeData child in children)
        {
            EdgeData connectingEdge = getConnectingEdge(origin, child);

            //only follow each edge once
            if (visitedEdges.Contains(connectingEdge)) continue;
            visitedEdges.Add(connectingEdge);

            //only visit each node once
            if (visitedNodes.Contains(child)) continue;
            visitedNodes.Add(child);

            recGetNodesAndEdgesReachableFrom(child, reverseEdgeDirection, visitedNodes, visitedEdges);
        }
    }

    /// <summary>
    /// Fix up the graph by removing orphaned edges and nodes
    /// </summary>
    public void TidyGraph()
    {
        //remove ownership of nodes that player's can't actually reach
        foreach(PlayerData player in FindObjectsOfType<PlayerData>())
        {
            HashSet<NodeData> reachableNodes = getNodesReachableFrom(player.StartingNode, false);
            //stop owning nodes you can't reach
            foreach (NodeData node in graph.Keys)
            {
                if (node.Owner == player && !reachableNodes.Contains(node))
                {
                    Debug.Log("Trimming unreachable node.");
                    node.Owner = null;
                }
            }
        }

        //remove directionality and domination type of edges that no longer connect two same-player-owned nodes
        foreach (EdgeData edge in FindObjectsOfType<EdgeData>())
        {
            if (edge.direction != EdgeData.EdgeDirection.Neutral && edge.direction != EdgeData.EdgeDirection.Unusable)
            {
                if (edge.nodeOne.GetComponent<NodeData>().Owner != edge.nodeTwo.GetComponent<NodeData>().Owner)
                {
                    edge.direction = EdgeData.EdgeDirection.Neutral;
                }
            }
        }
    }

    public List<NodeData> TopologicalSortOnEdgeSubset(NodeData origin, HashSet<EdgeData> includedEdges)
    {
        Stack<NodeData> result = new Stack<NodeData>();
        recTopoSort(origin, includedEdges, new HashSet<NodeData>(), result);

        List<NodeData> ret = new List<NodeData>();
        while (result.Count > 0)
        {
            ret.Add(result.Pop());
        }

        return ret;
    }

    private void recTopoSort(NodeData origin, HashSet<EdgeData> includedEdges, HashSet<NodeData> visitedNodes, Stack<NodeData> result)
    {
        if (visitedNodes.Contains(origin)) return;

        visitedNodes.Add(origin);
        foreach (NodeData node in getInfluencedNodes(origin))
        {
            EdgeData edge = getConnectingEdge(origin, node);
            if (!includedEdges.Contains(edge)) continue;

            recTopoSort(node, includedEdges, visitedNodes, result);
        }

        result.Push(origin);
    }
}
