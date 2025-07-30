using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{




    //Output time in ticks to destination. Also used as weight
    public static int tripCalc(GameObject startStar, GameObject endStar, int speedPerTick)
    {
        float distance = Vector2.Distance(startStar.transform.position, endStar.transform.position);
        int time = Mathf.CeilToInt(distance / speedPerTick);
        return time;
    }


    //Dijkstra's
    public /*List<Node>*/void calculate(Graph graph, int start, float travelSpeed)
    {
        List<Node> travelList = new List<Node>();
        int vertices = graph.GetAdjacencyList().Length;
        bool[] shortestPathTreeSet = new bool[vertices];
        int[] distances = new int[vertices];

        for (int i = 0; i < vertices; i++)
        {
            distances[i] = int.MaxValue;
            shortestPathTreeSet[i] = false;
        }

        distances[start] = 0;

        for (int count = 0; count < vertices - 1; count++)
        {
            int u = MinimumDistance(distances, shortestPathTreeSet);
            shortestPathTreeSet[u] = true;

            foreach (var neighbor in graph.GetAdjacencyList()[u])
            {
                int v = neighbor.vertex;
                int weight = neighbor.weight;

                if (!shortestPathTreeSet[v] && distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                {

                    distances[v] = distances[u] + weight;
/*                    Debug.Log(graph.GetAdjacencyList()[u].ToString());*/
                }
            }
        }
        int min = distances[0];
        int minIndex = 0;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                minIndex = i;
            }
        }

        Debug.Log(distances[minIndex]);


    }

    private static int MinimumDistance(int[] distances, bool[] shortestPathTreeSet)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < distances.Length; v++)
        {
            if (!shortestPathTreeSet[v] && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    //////////////



    public class Node
    {
        public GameObject star;
        public bool hasStargate;
        public bool isWormhole;
        public int range;

        public int weight;
        public int vertex;

        /*    //Make this a tuple?
            public List<GameObject> connectionsList;*/
    }
    public class Graph
    {
        private int vertices;
        private List<Node>[] adjacencyList;
        public Graph(List<GameObject> list, int speed)
        {
            vertices = list.Count;
            adjacencyList = new List<Node>[vertices];

            /*        for (int i = 0; i < vertices; i++)
                    {
                        adjacencyList[i] = new List<Node>();
                    }*/


            //Brute Force
            for (int i = 0; i < vertices; i++)
            {

                adjacencyList[i] = new List<Node>();
                /*            Node node = new Node()
                            {
                                star = tempStar,
                                range = tempStar.GetComponent<StarScript>().Range,
                            };*/


                for (int j = 0; j < vertices; j++)
                {
                    if ((Mathf.Pow((list[j].transform.position.x - list[i].transform.position.x), 2) + Mathf.Pow((list[j].transform.position.y - list[i].transform.position.y), 2) <= Mathf.Pow(list[i].GetComponent<StarScript>().Range, 2)))
                    {
                        AddEdge(i, j, tripCalc(list[i], list[j], speed));
/*                        Debug.Log("Added edge");*/
                    }

                }

            }



            /*            }
                    }*/

            //Smarter Method
            //Needs List<List<RingData>> list
            /*        for (int i = 0; i < list.Count; i++)
                    {

                        for (int j = 0; j < list[i].Count; j++)
                        {
                            GameObject tempStar = list[i][j].star;
                            Node node = new Node()
                            {
                                star = tempStar,
                                range = tempStar.GetComponent<StarScript>().Range,
                                connectionsList = new List<GameObject>()
                            };

                            `

                            for (int k = 0; k < Mathf.cei; k++)
                            {
                                list[i][j]
            *//*                    if ((Mathf.Pow((s.star.transform.position.x - list[i][j].position.x), 2) + Mathf.Pow((s.star.transform.position.y - list[i][j].position.y), 2) <= Mathf.Pow(node.range, 2){
                                    node.connectionsList.Add(s.star);

                                }*//*
                            }
                        }
            *//*            Mathf.RoundToInt(Vector2.Distance(tempStar.transform.position, currentStar.transform.position)) > TStarScript.Range);

                    (x - center_x) ^ 2 + (y - center_y) ^ 2 <= radius ^ 2*//*
                    }*/


            /*        float distance = Vector2.Distance(start.star.transform.position, endStar.transform.position);*/
        }

        public void AddEdge(int u, int vertex, int weight)
        {
            adjacencyList[u].Add(new Node()
            {
                vertex = vertex,
                weight = weight
            });
        }

        public List<Node>[] GetAdjacencyList()
        {
            return adjacencyList;
        }
    }


}


