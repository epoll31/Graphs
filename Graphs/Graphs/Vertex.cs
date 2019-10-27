using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    public class Vertex<T>
    {
        public T Value { get; internal set; }
        public List<Vertex<T>> Edges { get; internal set; }
        public int Count => Edges.Count;

        public Vertex(T value, params Vertex<T>[] edges)
        {
            Value = value;

            Edges = new List<Vertex<T>>(edges.Length);
            for (int i = 0; i < edges.Length; i++)
            {
                Edges.Add(edges[i]);
            }
        }
    }
}
