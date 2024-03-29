﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PriorityQueue;

namespace Graphs
{
    public class WeightedDirectedGraph<T> where T : IComparable
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


        public void AddEdge(T v, T w, float weight) =>
            AddEdge(Find(v), Find(w), weight);
        public void AddEdge(WeightedDirectedVertex<T> v, WeightedDirectedVertex<T> w, float weight)
        {
            if (v == null || w == null)
            {
                throw new Exception("One of the parameters passed was null");
            }

            Debug.WriteLine($"{ v.Value }, { w.Value }");

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

        public bool TryGetEdgeWeight(T startValue, T endValue, out float weight) => TryGetEdgeWeight(Find(startValue), Find(endValue), out weight);
        public bool TryGetEdgeWeight(WeightedDirectedVertex<T> startValue, WeightedDirectedVertex<T> endValue, out float weight)
        {
            if (startValue.Edges.ContainsKey(endValue))
            {
                weight = startValue.Edges[endValue];
                return true;
            }
            weight = float.NaN;
            return false;
        }

        public T[] DepthFirstTraversal(WeightedDirectedVertex<T> startVertex)
        {
            List<T> returnList = new List<T>(VertexCount);

            Stack<WeightedDirectedVertex<T>> stack = new Stack<WeightedDirectedVertex<T>>();
            stack.Push(startVertex);

            while (stack.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = stack.Pop();
                returnList.Add(currentVertex.Value);

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> vertex in currentVertex.Edges)
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

        public T[] BreadthFirstTraversal(WeightedDirectedVertex<T> startVertex)
        {
            List<T> returnList = new List<T>(VertexCount);

            Queue<WeightedDirectedVertex<T>> queue = new Queue<WeightedDirectedVertex<T>>();
            queue.Enqueue(startVertex);

            while (queue.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = queue.Dequeue();
                returnList.Add(currentVertex.Value);

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> vertex in currentVertex.Edges)
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

        public WeightedDirectedVertex<T>[] GetPathDF(T startValue, T endValue) => GetPathDF(Find(startValue), Find(endValue));
        public WeightedDirectedVertex<T>[] GetPathDF(WeightedDirectedVertex<T> startVertex, WeightedDirectedVertex<T> endVertex)
        {
            List<(WeightedDirectedVertex<T>, int)> data = new List<(WeightedDirectedVertex<T>, int)>();

            Stack<(WeightedDirectedVertex<T>, int)> stack = new Stack<(WeightedDirectedVertex<T>, int)>();
            stack.Push((startVertex, 0));

            List<WeightedDirectedVertex<T>> reversedPath = new List<WeightedDirectedVertex<T>>();

            int GetIndex(WeightedDirectedVertex<T> vertex)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Item1 == vertex)
                    {
                        return i;
                    }
                }
                return -1;
            }

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();
                data.Add((vertex.Item1, vertex.Item2));
                if (vertex.Item1 == endVertex)
                {
                    //follow parents back up the list until -1

                    while (vertex.Item2 != GetIndex(vertex.Item1))
                    {
                        reversedPath.Add(vertex.Item1);
                        vertex = data[vertex.Item2];
                    }
                    reversedPath.Add(vertex.Item1);

                    break;
                }
                else
                {
                    //push all edges with the index of vertex.Item2

                    foreach (var item in vertex.Item1.Edges)
                    {
                        if (GetIndex(item.Key) == -1)
                        {
                            stack.Push((item.Key, GetIndex(vertex.Item1)));
                        }
                    }
                }
            }

            if (reversedPath.Count == 0)
            {
                return null;
            }

            WeightedDirectedVertex<T>[] returnList = new WeightedDirectedVertex<T>[reversedPath.Count];
            for (int i = 0; i < returnList.Length; i++)
            {
                returnList[i] = reversedPath[reversedPath.Count - 1 - i];
            }

            return returnList;
        }

        public WeightedDirectedVertex<T>[] GetPathBF(T startValue, T endValue) => GetPathBF(Find(startValue), Find(endValue));
        public WeightedDirectedVertex<T>[] GetPathBF(WeightedDirectedVertex<T> startVertex, WeightedDirectedVertex<T> endVertex)
        {
            if (startVertex is null)
            {
                throw new ArgumentNullException(nameof(startVertex));
            }

            List<(WeightedDirectedVertex<T>, int)> data = new List<(WeightedDirectedVertex<T>, int)>();

            Queue<(WeightedDirectedVertex<T>, int)> queue = new Queue<(WeightedDirectedVertex<T>, int)>();
            queue.Enqueue((startVertex, 0));

            List<WeightedDirectedVertex<T>> reversedPath = new List<WeightedDirectedVertex<T>>();

            int GetIndex(WeightedDirectedVertex<T> vertex)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Item1 == vertex)
                    {
                        return i;
                    }
                }
                return -1;
            }

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                data.Add((vertex.Item1, vertex.Item2));
                if (vertex.Item1 == endVertex)
                {
                    //follow parents back up the list until -1

                    while (vertex.Item2 != GetIndex(vertex.Item1))
                    {
                        reversedPath.Add(vertex.Item1);
                        vertex = data[vertex.Item2];
                    }
                    reversedPath.Add(vertex.Item1);

                    break;
                }
                else
                {
                    //push all edges with the index of vertex.Item2

                    foreach (var item in vertex.Item1.Edges)
                    {
                        if (GetIndex(item.Key) == -1)
                        {
                            queue.Enqueue((item.Key, GetIndex(vertex.Item1)));
                        }
                    }
                }
            }

            if (reversedPath.Count == 0)
            {
                return null;
            }

            WeightedDirectedVertex<T>[] returnList = new WeightedDirectedVertex<T>[reversedPath.Count];
            for (int i = 0; i < returnList.Length; i++)
            {
                returnList[i] = reversedPath[reversedPath.Count - 1 - i];
            }

            return returnList;
        }

        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathDijkstras(T startValue, T endValue) => GetShortestPathDijkstras(Find(startValue), Find(endValue));
        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathDijkstras(WeightedDirectedVertex<T> startVertex, WeightedDirectedVertex<T> endVertex)
        {
            Dictionary<WeightedDirectedVertex<T>, (bool hasVisited, float cumulativeDistanceFromStart, WeightedDirectedVertex<T> founder)> vertexInfo = new Dictionary<WeightedDirectedVertex<T>, (bool hasVisited, float cumulativeDistanceFromStart, WeightedDirectedVertex<T> founder)>();

            for (int i = 0; i < VertexCount; i++)
            {
                vertexInfo[Vertices[i]] = (false, float.PositiveInfinity, null);
            }

            vertexInfo[startVertex] = (false, 0, null);
            PriorityQueue<float, WeightedDirectedVertex<T>> priorityQueue = new PriorityQueue<float, WeightedDirectedVertex<T>>();

            priorityQueue.Insert(startVertex, vertexInfo[startVertex].cumulativeDistanceFromStart);

            while (vertexInfo[endVertex].hasVisited == false && priorityQueue.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = priorityQueue.Pop();

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> keyValuePair in currentVertex.Edges)
                {
                    float tentativeCost = vertexInfo[currentVertex].cumulativeDistanceFromStart + keyValuePair.Value;
                    if (tentativeCost < vertexInfo[keyValuePair.Key].cumulativeDistanceFromStart)
                    {
                        vertexInfo[keyValuePair.Key] = (false, tentativeCost, currentVertex);
                    }

                    if (vertexInfo[keyValuePair.Key].hasVisited == false && !priorityQueue.Contains(keyValuePair.Key))
                    {
                        priorityQueue.Insert(keyValuePair.Key, vertexInfo[keyValuePair.Key].cumulativeDistanceFromStart);
                    }
                }
                vertexInfo[currentVertex] = (true, vertexInfo[currentVertex].cumulativeDistanceFromStart, vertexInfo[currentVertex].founder);
            }

            if (priorityQueue.Count == 0)
            {
                return (null, float.NaN);
            }

            LinkedList<WeightedDirectedVertex<T>> path = new LinkedList<WeightedDirectedVertex<T>>();

            WeightedDirectedVertex<T> current = endVertex;
            float cost = 0;
            while (current != null)
            {
                path.AddFirst(current);

                WeightedDirectedVertex<T> lastCurrent = current;
                current = vertexInfo[current].founder;

                if (current != null)
                {
                    cost += current.Edges[lastCurrent];
                }
            }

            return (path, cost);
        }

        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathBellmanFord(T startValue, T endValue) => GetShortestPathBellmanFord(Find(startValue), Find(endValue));
        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathBellmanFord(WeightedDirectedVertex<T> startVertex, WeightedDirectedVertex<T> endVertex)
        {
            Dictionary<WeightedDirectedVertex<T>, (float cumulativeDistanceFromStart, WeightedDirectedVertex<T> founder)> vertexInfo = new Dictionary<WeightedDirectedVertex<T>, (float cumulativeDistanceFromStart, WeightedDirectedVertex<T> founder)>();

            for (int i = 0; i < VertexCount; i++)
            {
                if (Vertices[i].Equals(startVertex))
                {
                    vertexInfo[Vertices[i]] = (0, null);
                }
                else
                {
                    vertexInfo[Vertices[i]] = (float.PositiveInfinity, null);
                }
            }

            foreach (WeightedDirectedVertex<T> vertex in Vertices)
            {
                if (vertex.Equals(startVertex))
                {
                    //continue;
                }
                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> edge in vertex.Edges)
                {
                    TryGetEdgeWeight(vertex, edge.Key, out float edgeWeight);
                    float tempDistance = vertexInfo[vertex].cumulativeDistanceFromStart + edgeWeight;
                    if (tempDistance < vertexInfo[edge.Key].cumulativeDistanceFromStart)
                    {
                        //throw new Exception("There is a negative cycle");
                        vertexInfo[edge.Key] = (tempDistance, vertex);
                    }
                }


            }

            Queue<WeightedDirectedVertex<T>> nextVertexQueue = new Queue<WeightedDirectedVertex<T>>();
            List<KeyValuePair<WeightedDirectedVertex<T>, float>> visitedEdges = new List<KeyValuePair<WeightedDirectedVertex<T>, float>>(EdgeCount);
            nextVertexQueue.Enqueue(startVertex);

            while (nextVertexQueue.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = nextVertexQueue.Dequeue();

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> edge in currentVertex.Edges)
                {
                    if (vertexInfo[currentVertex].cumulativeDistanceFromStart + edge.Value < vertexInfo[edge.Key].cumulativeDistanceFromStart)
                    {
                        throw new Exception("There is a negative cycle");
                    }
                    if (!visitedEdges.Contains(edge))
                    {
                        nextVertexQueue.Enqueue(edge.Key);
                        visitedEdges.Add(edge);
                    }
                }

            }
            /*
            for (int i = 0; i < VertexCount; i++)
            {
                foreach (WeightedDirectedVertex<T> currentVertex in Vertices)
                {
                    if (currentVertex == startVertex)
                    {
                        continue;
                    }
                    foreach (KeyValuePair<WeightedDirectedVertex<T>, float> keyValuePair in currentVertex.Edges)
                    {
                        float tentativeCost = currentVertex.CumulativeDistanceFromStart + keyValuePair.Value;
                        if (tentativeCost < keyValuePair.Key.CumulativeDistanceFromStart)
                        {
                            keyValuePair.Key.CumulativeDistanceFromStart = tentativeCost;
                            keyValuePair.Key.Founder = currentVertex;
                        }
                    }
                }
            }
            */


            LinkedList<WeightedDirectedVertex<T>> linkedList = new LinkedList<WeightedDirectedVertex<T>>();

            WeightedDirectedVertex<T> tempVertex = endVertex;

            while (tempVertex != null)
            {
                linkedList.AddFirst(tempVertex);
                tempVertex = vertexInfo[tempVertex].founder;
                Debug.WriteLine("still going");
            }

            return (linkedList, vertexInfo[endVertex].cumulativeDistanceFromStart);
        }

        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathAStar(T startValue, T endValue, Func<WeightedDirectedVertex<T>, WeightedDirectedVertex<T>, float> heuristicFunc) => GetShortestPathAStar(Find(startValue), Find(endValue), heuristicFunc);

        public (LinkedList<WeightedDirectedVertex<T>>, float) GetShortestPathAStar(WeightedDirectedVertex<T> startVertex, WeightedDirectedVertex<T> endVertex, Func<WeightedDirectedVertex<T>, WeightedDirectedVertex<T>, float> heuristicFunc)
        {
            Dictionary<WeightedDirectedVertex<T>, (float cumulativeDistanceFromStart, float finalDistance, WeightedDirectedVertex<T> founder, bool hasVisited)> vertexInfo = new Dictionary<WeightedDirectedVertex<T>, (float cumulativeDistanceFromStart, float finalDistance, WeightedDirectedVertex<T> founder, bool hasVisited)>();

            for (int i = 0; i < VertexCount; i++)
            {
                vertexInfo[Vertices[i]] = (float.PositiveInfinity, float.PositiveInfinity, null, false);
            }
            vertexInfo[startVertex] = (0, heuristicFunc(startVertex, endVertex), null, false);

            PriorityQueue<float, WeightedDirectedVertex<T>> queue = new PriorityQueue<float, WeightedDirectedVertex<T>>();

            queue.Insert(startVertex, vertexInfo[startVertex].finalDistance);

            while (queue.Count != 0)
            {
                WeightedDirectedVertex<T> currentVertex = queue.Pop();

                foreach (KeyValuePair<WeightedDirectedVertex<T>, float> edge in currentVertex.Edges)
                {
                    if (vertexInfo[edge.Key].hasVisited)
                    {
                        continue;
                    }

                    float tentativeDistance = vertexInfo[currentVertex].cumulativeDistanceFromStart + edge.Value;
                    if (tentativeDistance < vertexInfo[edge.Key].cumulativeDistanceFromStart)
                    {
                        vertexInfo[edge.Key] = (tentativeDistance, tentativeDistance + heuristicFunc(edge.Key, endVertex), currentVertex, vertexInfo[edge.Key].hasVisited);
                    }

                    if (!vertexInfo[edge.Key].hasVisited)
                    {
                        queue.Insert(edge.Key, vertexInfo[edge.Key].finalDistance);
                    }
                }

                vertexInfo[currentVertex] = (vertexInfo[currentVertex].cumulativeDistanceFromStart, vertexInfo[currentVertex].finalDistance, vertexInfo[currentVertex].founder, true);

                if (vertexInfo[endVertex].hasVisited)
                {
                    break;
                }
            }

            LinkedList<WeightedDirectedVertex<T>> path = new LinkedList<WeightedDirectedVertex<T>>();

            WeightedDirectedVertex<T> current = endVertex;
            float cost = 0;
            while (current != null)
            {
                path.AddFirst(current);

                WeightedDirectedVertex<T> lastCurrent = current;
                current = vertexInfo[current].founder;

                if (current != null)
                {
                    cost += current.Edges[lastCurrent];
                }
            }

            return (path, cost);
        }
    }
}
