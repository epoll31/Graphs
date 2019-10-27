using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs
{
    static class Program
    {
        public static void PrintGraph<T>(this UnweightedUndirectedGraph<T> graph)
        {
            foreach (Vertex<T> vertex in graph.Vertices)
            {
                Console.Write($"Vertex {vertex.Value} is connected to: ");

                foreach (Vertex<T> vertex1 in vertex.Edges)
                {
                    Console.Write($"{vertex1.Value}, ");
                }
                Console.CursorLeft -= 2;
                Console.Write(" \n\n");
            }
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("Data.txt");
            UnweightedUndirectedGraph<int> graph = new UnweightedUndirectedGraph<int>();

            for (int i = 0; i < int.Parse(lines[0]); i++)
            {
                graph.AddVertex(i);
            }
            for (int i = 0; i < int.Parse(lines[1]); i++)
            {
                graph.AddEdge(int.Parse(lines[i + 2].Split(" ")[0]), int.Parse(lines[i + 2].Split(" ")[1]));
            }

            graph.PrintGraph();
        }
    }
}
