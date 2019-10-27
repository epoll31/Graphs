using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    public class UnweightedUndirectedGraph<T>
    {
        public List<UnweightedUndirectedVertex<T>> Vertices { get; private set; }

        public int VertexCount => Vertices.Count;
        public int EdgeCount { get; internal set; }

        public UnweightedUndirectedGraph()
        {
            Vertices = new List<UnweightedUndirectedVertex<T>>();
        }

        public void AddVertex(T value)
        {
            if (Contains(value))
            {
                throw new Exception("The value you passed in already exists in this graph.");
            }

            Vertices.Add(new UnweightedUndirectedVertex<T>(value));
        }


        public void AddEdge(T v, T w) => AddEdge(Find(v), Find(w));
        public void AddEdge(UnweightedUndirectedVertex<T> v, UnweightedUndirectedVertex<T> w)
        {
            if (v == null || w == null)
            {
                throw new Exception("One of the parameters passed was null");
            }

            v.Edges.Add(w);
            w.Edges.Add(v);
            EdgeCount++;
        }

        public bool RemoveVertex(T value)
        {
            if (!Contains(value))
            {
                return false;
            }

            for (int i = 0; i < VertexCount; i++)
            {
                Vertices[i].Edges.RemoveAll(v => v.Value.Equals(value));
            }
            Vertices.RemoveAll(v => v.Value.Equals(value));

            return true;
        }

        public bool RemoveEdge(T v, T w) => RemoveEdge(Find(v), Find(w));
        public bool RemoveEdge(UnweightedUndirectedVertex<T> v, UnweightedUndirectedVertex<T> w)
        {
            //and should work, maybe want or
            return v.Edges.Remove(w) && w.Edges.Remove(v);
        }

        public bool Contains(T value) => Find(value) != null;

        public UnweightedUndirectedVertex<T> Find(T value)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                if (value.Equals(Vertices[i].Value))
                {
                    return Vertices[i];
                }
            }
            return null;
        }

        public T[] DepthFirstTraversal(UnweightedUndirectedVertex<T> startNode)
        {
            List<T> returnList = new List<T>(VertexCount);

            Stack<UnweightedUndirectedVertex<T>> stack = new Stack<UnweightedUndirectedVertex<T>>();
            stack.Push(startNode);

            while (stack.Count != 0)
            {
                UnweightedUndirectedVertex<T> currentVertex = stack.Pop();
                returnList.Add(currentVertex.Value);

                foreach(UnweightedUndirectedVertex<T> vertex in currentVertex.Edges)
                {
                    if (returnList.Contains(vertex.Value))
                    {
                        continue;
                    }

                    stack.Push(vertex);
                }
            }

            return returnList.ToArray();
        }

        public T[] BreadthFirstTraversal(UnweightedUndirectedVertex<T> startNode)
        {
            List<T> returnList = new List<T>(VertexCount);

            Queue<UnweightedUndirectedVertex<T>> queue = new Queue<UnweightedUndirectedVertex<T>>();
            queue.Enqueue(startNode);

            while (queue.Count != 0)
            {
                UnweightedUndirectedVertex<T> currentVertex = queue.Dequeue();
                returnList.Add(currentVertex.Value);

                foreach(UnweightedUndirectedVertex<T> vertex in currentVertex.Edges)
                {
                    if (returnList.Contains(vertex.Value))
                    {
                        continue;
                    }

                    queue.Enqueue(vertex);
                }
            }

            return returnList.ToArray();
        }
    }
}
