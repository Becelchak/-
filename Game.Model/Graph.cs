using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
	public class Graph
	{
		private Node[] nodes;

		public Graph(int nodesCount)
		{
			nodes = Enumerable.Range(0, nodesCount).Select(z => new Node(z)).ToArray();
		}

		public int Length => nodes.Length;

		public Node this[int index] => nodes[index];

		public IEnumerable<Node> Nodes => nodes;

		public void Connect(int index1, int index2)
		{
			Node.Connect(nodes[index1], nodes[index2], this);
		}

		public void Delete(Edge edge)
		{
			Node.Disconnect(edge);
		}

		public void Add(Node node)
		{
			nodes=nodes.Select(x=>x).Append(node).ToArray();
		}

		public IEnumerable<Edge> Edges
		{
			get
			{
				return nodes.SelectMany(z => z.IncidentEdges).Distinct();
			}
		}

		public static Graph MakeGraph(params int[] incidentNodes)
		{
			var graph = new Graph(incidentNodes.Max() + 1);
			for (int i = 0; i < incidentNodes.Length - 1; i += 2)
				graph.Connect(incidentNodes[i], incidentNodes[i + 1]);
			return graph;
		}
	}
}