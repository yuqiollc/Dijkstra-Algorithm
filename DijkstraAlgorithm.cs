using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class DijkstraAlgorithm
{
    /// <summary>
    /// Edge class
    /// </summary>
    class Edge
    {
        /// <summary>
        /// Node of Edge start
        /// </summary>
        public Node Start;
        /// <summary>
        /// End Node of Edge
        /// </summary>
        public Node End;
        /// <summary>
        /// Edge weight
        /// </summary>
        public float Weight;
    }

    /// <summary>
    /// Node class
    /// </summary>
    class Node
    {
        /// <summary>
        /// Node index in adjacency matrix
        /// </summary>
        public int Index;
        /// <summary>
        /// Edges connected to this Node
        /// </summary>
        public List<Edge> Edges = new List<Edge>();
        /// <summary>
        /// Edge weight
        /// </summary>
        public float Weight;
        /// <summary>
        /// Previous Node, from which we travelled to the current Node
        /// </summary>
        public Node PrevNode;
    }

    /// <summary>
    /// Algorithm initialization 
    /// </summary>
    /// <param name="matrix">Adjacency matrix</param>
    /// <param name="start">Start Node Index</param>
    /// <param name="end">End Node Index</param>
    public DijkstraAlgorithm(float[,] matrix, int start, int end)
    {
        this._end = end;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            Node newNode = new Node();
            _nodes.Add(newNode);
        }

        //Nodes and Edges initialization for given Nodes, connected to specified Nodes
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            _nodes[i].Index = i;

            //Define weight for Start Node as minimal
            if (i == start)
                _nodes[i].Weight = 0;
            else
                _nodes[i].Weight = float.PositiveInfinity;

            //Connect Nodes with Edges
            for (int j = 0; j < _nodes.Count; j++)
            {
                if (matrix[i, j] > 0)
                {
                    Edge edge = new Edge();
                    edge.Weight = matrix[i, j];
                    edge.Start = _nodes[i];
                    edge.End = _nodes[j];
                    _nodes[i].Edges.Add(edge);
                }
            }
        }
    }

    /// <summary>
    /// Algorithm Launch
    /// </summary>
    /// <returns></returns>
    public int[] Run()
    {
        //Unparsed Edges
        List<Node> notUsed = new List<Node>(_nodes);

        while (true)
        {
            //Initialization of current Node parsed by Algorithm
            Node CurrentNode = Min(notUsed);

            if (IsEndNode(CurrentNode))
                return GetPath(CurrentNode);

            //Check all Edges of current Node
            foreach (var edge in CurrentNode.Edges)
            {
                //If weight of Edge is lesser, update the weight and write down the Previous node.
                if (CurrentNode.Weight + edge.Weight < edge.End.Weight)
                {
                    edge.End.Weight = CurrentNode.Weight + edge.Weight;
                    edge.End.PrevNode = CurrentNode;
                }
            }

            //Delete parsed Node from the list
            notUsed.Remove(CurrentNode);
        }
    }

    /// <summary>
    /// Returns the Node with smallest weight
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    private Node Min(List<Node> nodes)
    {
        Node temp = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].Weight < temp.Weight)
                temp = nodes[i];
        }

        return temp;
    }

    /// <summary>
    /// Test if the Node is End Node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool IsEndNode(Node node)
    {
        if (node == _nodes[_end])
            return true;
        else
            return false;
    }

    /// <summary>
    /// Trace recovery
    /// </summary>
    /// <param name="node">End node</param>
    /// <returns></returns>
    private int[] GetPath(Node node)
    {
        List<Node> nodesPath = new List<Node>();

        nodesPath.Add(node);

        Node currNode = node;

        while (true)
        {
            if(currNode.PrevNode != null)
            {
                nodesPath.Add(currNode.PrevNode);
                currNode = currNode.PrevNode;
            }
            else
            {
                //Inverse order of the list (trace recovered from End point)
                nodesPath.Reverse();

                int[] path = new int[nodesPath.Count];
                for (int i = 0; i < nodesPath.Count; i++)
                    path[i] = nodesPath[i].Index;

                return path;
            }
        }
    }

    #region private variables

    /// <summary>
    /// List of all Nodes
    /// </summary>
    private List<Node> _nodes = new List<Node>();

    /// <summary>
    /// Index of End Node
    /// </summary>
    private int _end;

    #endregion
}

