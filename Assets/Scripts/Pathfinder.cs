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
        public List<GameObject> starList;
        public List<GameObject> dumbStarList;
        public int speed;
        public int tickCreated;

        List<GridObject> lowLevelGrid = new List<GridObject>();
        public Graph(List<GameObject> list, int speed, int tick)
        {
            this.starList = list;
            this.speed = speed;
            this.tickCreated = tick;
            Debug.Log("Created graph script on tick:" + tick);

            /*        for (int i = 0; i < vertices; i++)
                    {
                        adjacencyList[i] = new List<Node>();
                    }*/






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

        public void calculateGridSquaresTree(int squareSize, int subdivisionCount)
        {
            /*            Vector2Int v1 = new Vector2Int(squareSize, squareSize); //Top right
                        Vector2Int v2 = new Vector2Int(squareSize, -squareSize); //Bottom right
                        Vector2Int v3 = new Vector2Int(-squareSize, -squareSize); //Bottom left
                        Vector2Int v4 = new Vector2Int(-squareSize, squareSize); //Top Right*/

            
/*            int currentSize = squareSize;*/
            GridObject gridTreeParent = new GridObject(null, 0, squareSize, new Vector2Int(0,0));
            for (int i = 1; i < subdivisionCount; i++)
            {

                int currentSize = squareSize / (4*i);
            }

        }


        //RECURSIVE, BEWARE!!!
        public void calculateChildrenSquares(GridObject gridObject, int desiredLevel)
        {

            Vector2Int parentPosition = gridObject.position;
            gridObject.children = new GridObject[4]
            {
                    
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLenght/4, new Vector2Int(parentPosition.x + (parentPosition.x/4), parentPosition.y + (parentPosition.y/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLenght/4, new Vector2Int(parentPosition.x + (parentPosition.x/4), parentPosition.y - (parentPosition.y/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLenght/4, new Vector2Int(parentPosition.x - (parentPosition.x/4), parentPosition.y - (parentPosition.y/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLenght/4, new Vector2Int(parentPosition.x - (parentPosition.x/4), parentPosition.y + (parentPosition.y/4)) )
            };


            if (desiredLevel > gridObject.level + 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    calculateChildrenSquares(gridObject.children[i], desiredLevel);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    lowLevelGrid.Add(gridObject.children[i]);
                }
            }


            }

        public class GridObject
        {
            public GridObject parent;
            public GridObject[] children;
            public int level;
            public bool isLowest;
            public List<GameObject> stars;
            public int sideLenght;
            public Vector2Int position;
            public GridObject(GridObject parent, int level, int sideLenght, Vector2Int position)
            {
                this.parent = parent;
                this.level = level;
                this.sideLenght = sideLenght;
                this.position = position;

            }
            public void addStars(List<GameObject> starsList)
            {
                stars = starsList;
            }
        }

        public void calculateGraph()
        {


            //Brute Forceish
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
                    if ((Mathf.Pow((dumbStarList[j].transform.position.x - dumbStarList[i].transform.position.x), 2) + Mathf.Pow((dumbStarList[j].transform.position.y - dumbStarList[i].transform.position.y), 2) <= Mathf.Pow(dumbStarList[i].GetComponent<StarScript>().Range, 2)))
                    {
                        AddEdge(i, j, tripCalc(dumbStarList[i], dumbStarList[j], speed));
                        /*                        Debug.Log("Added edge");*/
                    }

                }

            }
        }


        public Dictionary<Vector2Int, List<Vector2>> gridGraph(List<GameObject> starList, int offset, int numOfCircles)
        {
            Dictionary<Vector2Int, List<Vector2>> grid = new Dictionary<Vector2Int, List<Vector2>>();

/*            Vector2Int topLeft = new Vector2Int(-((offset + 2) * numOfCircles), (offset + 2) * numOfCircles);
            Vector2Int bottomRight = new Vector2Int((offset + 2) * numOfCircles, -((offset + 2) * numOfCircles));*/



            return grid;
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


