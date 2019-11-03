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
            /*string[] lines = File.ReadAllLines("Data.txt");
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
            
            (LinkedList<WeightedDirectedVertex<int>>, float) dijkstras = 
                graph.GetShortestPathAStar(12, 0);

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
            */

            WeightedDirectedGraph<(int x, int y)> graph = new WeightedDirectedGraph<(int x, int y)>();

            int size = 10;

            void AddEdge((int x, int y) value1, (int x, int y) value2, float weight)
            {
                if (value1.x == size/2 && value1.y < size/2 || value2.x == size / 2 && value2.y < size / 2)
                {
                    return;
                }

                graph.AddEdge(value1, value2, weight);
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    graph.AddVertex((i, j));
                }
            }

            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i == 0 && j == 0)//top left
                    {
                        AddEdge((i, j), (i + 1, j), 1);
                        AddEdge((i, j), (i, j + 1), 1);

                        AddEdge((i, j), (i + 1, j + 1), 0.9f);
                    }
                    else if (i == size - 1 && j == size - 1)//bottom right
                    {
                        AddEdge((i, j), (i - 1, j), 1);
                        AddEdge((i, j), (i, j - 1), 1);

                        AddEdge((i, j), (i - 1, j - 1), 0.9f);
                    }
                    else if (i == 0 && j == size - 1)//top right
                    {
                        AddEdge((i, j), (i + 1, j), 1);
                        AddEdge((i, j), (i, j - 1), 1);

                        AddEdge((i, j), (i + 1, j - 1), 0.9f);
                    }
                    else if (i == size - 1 && j == 0)//bottom left
                    {
                        AddEdge((i, j), (i - 1, j), 1);
                        AddEdge((i, j), (i, j + 1), 1);

                        AddEdge((i, j), (i - 1, j + 1), 0.9f);
                    }
                    else if (i == 0)//top middle
                    {
                        AddEdge((i, j), (i, j + 1), 1);
                        AddEdge((i, j), (i, j - 1), 1);
                        AddEdge((i, j), (i + 1, j), 1);

                        AddEdge((i, j), (i + 1, j + 1), 0.9f);
                        AddEdge((i, j), (i + 1, j - 1), 0.9f);
                    }
                    else if (j == 0)//middle left
                    {
                        AddEdge((i, j), (i - 1, j), 1);
                        AddEdge((i, j), (i + 1, j), 1);
                        AddEdge((i, j), (i, j + 1), 1);

                        AddEdge((i, j), (i + 1, j + 1), 0.9f);
                        AddEdge((i, j), (i - 1, j + 1), 0.9f);
                    }
                    else if (i == size - 1)//bottom middle
                    {
                        AddEdge((i, j), (i, j + 1), 1);
                        AddEdge((i, j), (i, j - 1), 1);
                        AddEdge((i, j), (i - 1, j), 1);

                        AddEdge((i, j), (i - 1, j + 1), 0.9f);
                        AddEdge((i, j), (i - 1, j - 1), 0.9f);
                    }
                    else if (j == size - 1)//middle right
                    {
                        AddEdge((i, j), (i - 1, j), 1);
                        AddEdge((i, j), (i + 1, j), 1);
                        AddEdge((i, j), (i, j - 1), 1);

                        AddEdge((i, j), (i + 1, j - 1), 0.9f);
                        AddEdge((i, j), (i - 1, j - 1), 0.9f);
                    }
                    else//middle
                    {
                        AddEdge((i, j), (i, j + 1), 1);
                        AddEdge((i, j), (i, j - 1), 1);
                        AddEdge((i, j), (i + 1, j), 1);
                        AddEdge((i, j), (i - 1, j), 1);

                        AddEdge((i, j), (i + 1, j + 1), 0.9f);
                        AddEdge((i, j), (i + 1, j - 1), 0.9f);
                        AddEdge((i, j), (i - 1, j + 1), 0.9f);
                        AddEdge((i, j), (i - 1, j - 1), 0.9f);
                    }
                }
            }

            float ManhattanHeuristic(WeightedDirectedVertex<(int x, int y)> startVertex, WeightedDirectedVertex<(int x, int y)> endVertex)
            {
                return 1 * (Math.Abs(startVertex.Value.x - endVertex.Value.x) + Math.Abs(startVertex.Value.y - endVertex.Value.y));
            }
            float DiagonalHeuristic(WeightedDirectedVertex<(int x, int y)> startVertex, WeightedDirectedVertex<(int x, int y)> endVertex)
            {
                float dx = Math.Abs(startVertex.Value.x - endVertex.Value.x);
                float dy = Math.Abs(startVertex.Value.y - endVertex.Value.y);
                float min = MathF.Min(dx, dy);
                return 1 * (dx + dy) + (1 - 2 * 1) * min;
            }
            float EuclideanHeuristic(WeightedDirectedVertex<(int x, int y)> startVertex, WeightedDirectedVertex<(int x, int y)> endVertex)
            {
                float dx = Math.Abs(startVertex.Value.x - endVertex.Value.x);
                float dy = Math.Abs(startVertex.Value.y - endVertex.Value.y);
                float min = MathF.Min(dx, dy);
                return 1 * (float)Math.Sqrt(dx * dx + dy * dy);
            }

            (LinkedList<WeightedDirectedVertex<(int x, int y)>> path, float cost) = graph.GetShortestPathAStar((0, 0), (9, 0), EuclideanHeuristic);

            Console.WriteLine($"Cost: {cost}");
            foreach (WeightedDirectedVertex<(int x, int y)> vertex in path)
            {
                Console.Write($"({vertex.Value.x}, {vertex.Value.y}) -> ");
            }
            Console.CursorLeft -= 3;
            Console.WriteLine("   ");
        }
    }
}
