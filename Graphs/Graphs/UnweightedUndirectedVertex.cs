using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    public class UnweightedUndirectedVertex<T>
    {
        public T Value { get; internal set; }
        public List<UnweightedUndirectedVertex<T>> Edges { get; internal set; }
        public int Count => Edges.Count;

        public UnweightedUndirectedVertex(T value, params UnweightedUndirectedVertex<T>[] edges)
        {
            Value = value;

            Edges = new List<UnweightedUndirectedVertex<T>>(edges.Length);
            for (int i = 0; i < edges.Length; i++)
            {
                Edges.Add(edges[i]);
            }
        }
    }
}
