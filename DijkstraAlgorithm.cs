using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class DijkstraAlgorithm
{
    /// <summary>
    /// Класс дуги
    /// </summary>
    class Edge
    {
        /// <summary>
        /// Узел с которого начинается дуга
        /// </summary>
        public Node Start;
        /// <summary>
        /// Конечный узел дуги
        /// </summary>
        public Node End;
        /// <summary>
        /// Вес дуги
        /// </summary>
        public float Weight;
    }

    /// <summary>
    /// Класс узла
    /// </summary>
    class Node
    {
        /// <summary>
        /// Индекс узла в матрице смежности
        /// </summary>
        public int Index;
        /// <summary>
        /// Дуги которые принадлежат этому узлу
        /// </summary>
        public List<Edge> Edges = new List<Edge>();
        /// <summary>
        /// Вес узла
        /// </summary>
        public float Weight;
        /// <summary>
        /// Предыдущий узел с которого перешли в этот узел
        /// </summary>
        public Node PrevNode;
    }

    /// <summary>
    /// Инициализация алгоритма
    /// </summary>
    /// <param name="matrix">Матрица смежности</param>
    /// <param name="start">Индекс стартовой точки</param>
    /// <param name="end">Индекс конечной точки</param>
    public DijkstraAlgorithm(float[,] matrix, int start, int end)
    {
        this._end = end;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            Node newNode = new Node();
            _nodes.Add(newNode);
        }

        //Инициализация узлов и дуг прилежным данным узлам
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            _nodes[i].Index = i;

            //Присваиваем начальной точке минимальный вес что бы алгоритм принял ее за стартовую
            if (i == start)
                _nodes[i].Weight = 0;
            else
                _nodes[i].Weight = float.PositiveInfinity;

            //Присваивание узлам дуги
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
    /// Запуск алгоритма
    /// </summary>
    /// <returns></returns>
    public int[] Run()
    {
        //Узлы по которым алгоритм еще не проходился
        List<Node> notUsed = new List<Node>(_nodes);

        while (true)
        {
            //Инициализация текущего узла по которому будет проходить алгоритм
            Node CurrentNode = Min(notUsed);

            if (IsEndNode(CurrentNode))
                return GetPath(CurrentNode);

            //Проходим по каждому ребру текущего узла
            foreach (var edge in CurrentNode.Edges)
            {
                //Если переход на следующий узел имеет меньший вес, обновляем вес и записываем точку с которой пришли
                if (CurrentNode.Weight + edge.Weight < edge.End.Weight)
                {
                    edge.End.Weight = CurrentNode.Weight + edge.Weight;
                    edge.End.PrevNode = CurrentNode;
                }
            }

            //Удаляем из списка использованый узел
            notUsed.Remove(CurrentNode);
        }
    }

    /// <summary>
    /// Возвращает узел с наименьшим весом
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
    /// Является ли узел конечным
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
    /// Восстановление пути
    /// </summary>
    /// <param name="node">Конечный узел</param>
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
                //Задаем списку обратный порядок т.к. путь восстанавливается с конца
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
    /// Лист всех узлов
    /// </summary>
    private List<Node> _nodes = new List<Node>();

    /// <summary>
    /// Индекс конечной точки
    /// </summary>
    private int _end;

    #endregion
}

