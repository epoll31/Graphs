using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs
{
    static class Program
    {
        public static void PrintGraph<T>(this UnweightedUndirectedGraph<T> graph)
        {
            foreach (UnweightedUndirectedVertex<T> vertex in graph.Vertices)
            {
                Console.Write($"Vertex {vertex.Value} is connected to: ");

                foreach (UnweightedUndirectedVertex<T> vertex1 in vertex.Edges)
                {
                    Console.Write($"{vertex1.Value}, ");
                }
                Console.CursorLeft -= 2;
                Console.Write(" \n\n");
            }
        }
        public static void PrintGraph<T>(this WeightedDirectedGraph<T> graph)
        {
            foreach (WeightedDirectedVertex<T> vertex in graph.Vertices)
            {
                Console.Write($"Vertex {vertex.Value} is connected to: ");

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> vertex1 in vertex.Edges)
                {
                    Console.Write($"{vertex1.Key.Value}({vertex1.Value}), ");
                }
                Console.CursorLeft -= 2;
                Console.Write(" \n\n");
            }
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("Data.txt");
            WeightedDirectedGraph<int> graph = new WeightedDirectedGraph<int>();

            Random random = new Random();
            for (int i = 0; i < int.Parse(lines[0]); i++)
            {
                graph.AddVertex(i);
            }
            for (int i = 0; i < int.Parse(lines[1]); i++)
            {
                graph.AddEdge(int.Parse(lines[i + 2].Split(" ")[0]), int.Parse(lines[i + 2].Split(" ")[1]), random.Next(1, 5));
            }

            graph.PrintGraph();
        }
    }
}
