using System;
using System.Collections.Generic;
using System.Text;

namespace Graphs
{
    public class WeightedDirectedVertex<T> : IComparable where T : IComparable
    {
        public T Value { get; internal set; }
        public Dictionary<WeightedDirectedVertex<T>, float> Edges { get; internal set; }
        public int Count => Edges.Count;

        public WeightedDirectedVertex(T value)
        {
            Value = value;

            Edges = new Dictionary<WeightedDirectedVertex<T>, float>();
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}
