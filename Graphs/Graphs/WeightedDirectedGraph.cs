using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    public class WeightedDirectedGraph<T>
    {
        public List<WeightedDirectedVertex<T>> Vertices { get; private set; }

        public int VertexCount => Vertices.Count;
        public int EdgeCount { get; internal set; }

        public WeightedDirectedGraph()
        {
            Vertices = new List<WeightedDirectedVertex<T>>();
        }

        public void AddVertex(T value)
        {
            if (Contains(value))
            {
                throw new Exception("The value you passed in already exists in this graph.");
            }

            Vertices.Add(new WeightedDirectedVertex<T>(value));
        }


        public void AddEdge(T v, T w, float weight) => AddEdge(Find(v), Find(w), weight);
        public void AddEdge(WeightedDirectedVertex<T> v, WeightedDirectedVertex<T> w, float weight)
        {
            if (v == null || w == null)
            {
                throw new Exception("One of the parameters passed was null");
            }

            v.Edges.Add(w, weight);
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
                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> keyValuePair in Vertices[i].Edges)
                {
                    if (keyValuePair.Key.Value.Equals(value))
                    {
                        Vertices[i].Edges.Remove(keyValuePair.Key);
                        break;
                    }
                }
                
            }
            Vertices.RemoveAll(v => v.Value.Equals(value));

            return true;
        }

        public bool RemoveEdge(T v, T w) => RemoveEdge(Find(v), Find(w));
        public bool RemoveEdge(WeightedDirectedVertex<T> v, WeightedDirectedVertex<T> w)
        {
            //and should work, maybe want or
            return v.Edges.Remove(w) && w.Edges.Remove(v);
        }

        public bool Contains(T value) => Find(value) != null;

        public WeightedDirectedVertex<T> Find(T value)
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

        public T[] DepthFirstTraversal(WeightedDirectedVertex<T> startNode)
        {
            List<T> returnList = new List<T>(VertexCount);

            Stack<WeightedDirectedVertex<T>> stack = new Stack<WeightedDirectedVertex<T>>();
            stack.Push(startNode);

            while (stack.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = stack.Pop();
                returnList.Add(currentVertex.Value);

                foreach(KeyValuePair<WeightedDirectedVertex<T>, float> vertex in currentVertex.Edges)
                {
                    if (returnList.Contains(vertex.Key.Value))
                    {
                        continue;
                    }

                    stack.Push(vertex.Key);
                }
            }

            return returnList.ToArray();
        }

        public T[] BreadthFirstTraversal(WeightedDirectedVertex<T> startNode)
        {
            List<T> returnList = new List<T>(VertexCount);

            Queue<WeightedDirectedVertex<T>> queue = new Queue<WeightedDirectedVertex<T>>();
            queue.Enqueue(startNode);

            while (queue.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = queue.Dequeue();
                returnList.Add(currentVertex.Value);

                foreach(KeyValuePair<WeightedDirectedVertex<T>, float> vertex in currentVertex.Edges)
                {
                    if (returnList.Contains(vertex.Key.Value))
                    {
                        continue;
                    }

                    queue.Enqueue(vertex.Key);
                }
            }

            return returnList.ToArray();
        }
    }
}
