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
        public static void PrintGraph<T>(this WeightedDirectedGraph<T> graph) where T : IComparable
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

            for (int i = 0; i < int.Parse(lines[0]); i++)
            {
                graph.AddVertex(i);
            }
            for (int i = 0; i < int.Parse(lines[1]); i++)
            {
                graph.AddEdge(int.Parse(lines[i + 2].Split(" ")[0]), int.Parse(lines[i + 2].Split(" ")[1]), int.Parse(lines[i + 2].Split(" ")[2]));
            }

            graph.PrintGraph();

            /*
            WeightedDirectedVertex<int>[] path = graph.GetPathDF(12, 0);

            if (path == null)
            {
                Console.WriteLine("This Path Does Not Exist");
            }
            else
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Console.Write($"{path[i].Value} -> ");
                }
                Console.CursorLeft -= 3;
                Console.WriteLine("  ");
            }

            path = graph.GetPathBF(12, 0);

            if (path == null)
            {
                Console.WriteLine("This Path Does Not Exist");
            }
            else
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Console.Write($"{path[i].Value} -> ");
                }
                Console.CursorLeft -= 3;
                Console.WriteLine("  ");
            }
            */
            (LinkedList<WeightedDirectedVertex<int>>, float) dijkstras = 
                graph.GetShortestPathBellmanFord(0, 3);

            if (dijkstras.Item1 == null)
            {
                Console.WriteLine("This Path Does Not Exist");
            }
            else
            {
                Console.WriteLine($"Cost: {dijkstras.Item2}");
                foreach (WeightedDirectedVertex<int> vertex in dijkstras.Item1)
                {
                    Console.Write($"{vertex.Value} -> ");
                }
                Console.CursorLeft -= 3;
                Console.WriteLine("  ");
            }
        }
    }
}
