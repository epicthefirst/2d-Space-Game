using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinder : MonoBehaviour
{

    public GameObject blueCircleDot;


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

        /*        Debug.Log(distances[minIndex]);
                Debug.Log(minIndex);
                Debug.Log(shortestPathTreeSet.Length);*/

        string text = "|";
        for (int i = 0; i < shortestPathTreeSet.Length; i++)
        {
            if (shortestPathTreeSet[i] == true)
            {
                text = text + "X";
            }
            else
            {
                text = text + " . ";
            }
        }
        Debug.Log(text);
    }

    public void test()
    {
        Graph graph = new Graph(null, 10, 0);
        graph.calculateGridSquaresTree(1024, 4);
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
        public List<GameObject> shortStarList;
        public int speed;
        public int tickCreated;

        List<GridObject> lowLevelGrid = new List<GridObject>();
        List<GameObject> slimStarList = new List<GameObject>();
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

        public int findStarIndex(GameObject star)
        {
            /*            for (int i = 0; i > slimStarList.Count; i++)
                        {
                            if (star = slimStarList[i])
                            {
                                return i;
                            }
                        }*/
            int thing = slimStarList.IndexOf(star);
            Debug.Log(thing);
            if (thing == -1)
            {
                Debug.LogError("FIX ME");
            }
            return thing;
        }
        public void calculateGridSquaresTree(int squareSize, int subdivisionCount)
        {
            /*            Vector2Int v1 = new Vector2Int(squareSize, squareSize); //Top right
                        Vector2Int v2 = new Vector2Int(squareSize, -squareSize); //Bottom right
                        Vector2Int v3 = new Vector2Int(-squareSize, -squareSize); //Bottom left
                        Vector2Int v4 = new Vector2Int(-squareSize, squareSize); //Top Right*/

            
/*            int currentSize = squareSize;*/
            GridObject gridTreeParent = new GridObject(null, 0, squareSize, new Vector2Int(0,0));
            gridTreeParent.starsInSquare = starList;
            calculateChildrenSquares(gridTreeParent, subdivisionCount);
            Debug.Log("Done");
            Debug.Log(lowLevelGrid.Count);

        }


        //RECURSIVE, BEWARE!!!
        public void calculateChildrenSquares(GridObject gridObject, int desiredLevel)
        {
            Debug.LogError("calculateChildrenSquares");

            Vector2Int parentPosition = gridObject.position;
            gridObject.children = new GridObject[4]
            {
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLength/2, new Vector2Int(parentPosition.x + (gridObject.sideLength/4), parentPosition.y + (gridObject.sideLength/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLength/2, new Vector2Int(parentPosition.x + (gridObject.sideLength/4), parentPosition.y - (gridObject.sideLength/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLength/2, new Vector2Int(parentPosition.x - (gridObject.sideLength/4), parentPosition.y - (gridObject.sideLength/4)) ),
                new GridObject(gridObject, gridObject.level + 1, gridObject.sideLength/2, new Vector2Int(parentPosition.x - (gridObject.sideLength/4), parentPosition.y + (gridObject.sideLength/4)) )
            };

            foreach (GridObject child in gridObject.children)
            {
                int radius = 4;

                GameObject circleObject = new GameObject("circleObject");
                LineRenderer circleMaker = circleObject.AddComponent<LineRenderer>();
                circleMaker.material = new Material(Shader.Find("Sprites/Default"));

                circleMaker.startColor = Color.cyan;
                circleMaker.endColor = Color.cyan;
                circleMaker.startWidth = 1f;
                circleMaker.endWidth = 1f;

                int steps = (int)MathF.Round(2 * MathF.PI * radius);
                circleMaker.positionCount = (steps) + 2;

                for (int i = 0; i < (steps) + 2; i++)
                {
                    float circumferenceProgress = (float)i / steps;

                    float currentRadian = (circumferenceProgress * 2 * Mathf.PI);

                    float xScaled = Mathf.Cos(currentRadian);
                    float yScaled = Mathf.Sin(currentRadian);

                    float x = xScaled * radius;
                    float y = yScaled * radius;

                    Vector2 position = new Vector2(x, y) + child.position;

                    circleMaker.SetPosition(i, position);
                }
                circleMaker.material = new Material(Shader.Find("Sprites/Default"));

                
                child.calculateStarsInSquare(gridObject);
                Debug.LogWarning(gridObject.starsInSquare.Count);
            }

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

        public List<GameObject> dumbedListCalculator(GameObject startStar, GameObject endstar)
        {
            Debug.LogError("Running");

            Vector2 difference = endstar.transform.position - startStar.transform.position;

            float length = Mathf.Sqrt((difference.x * difference.x) + (difference.y * difference.y));

            Vector2 normalAdjust = new Vector2(difference.x / length, difference.y / length);

            Vector2 temp1 = new Vector2(startStar.transform.position.x, startStar.transform.position.y) - (normalAdjust * length);
            Vector2 temp2 = new Vector2(endstar.transform.position.x, endstar.transform.position.y) + (normalAdjust * length);

            Vector2 perpendicularOffset = new Vector2(-normalAdjust.y, normalAdjust.x) * length;

            Vector2 v1 = temp1 - perpendicularOffset;
            Vector2 v2 = temp1 + perpendicularOffset;
            Vector2 v3 = temp2 + perpendicularOffset;
            Vector2 v4 = temp2 - perpendicularOffset;
            Debug.Log(lowLevelGrid.Count);
            Debug.Log("Vectors");
            Debug.Log(v1);
            Debug.Log(v2);
            Debug.Log(v3);
            Debug.Log(v4);
            Debug.Log(perpendicularOffset);

            Instantiate(endstar, v1, Quaternion.identity, null);
            Instantiate(endstar, v2, Quaternion.identity, null);
            Instantiate(endstar, v3, Quaternion.identity, null);
            Instantiate(endstar, v4, Quaternion.identity, null);

            Debug.LogError(lowLevelGrid.Count);
            foreach (GridObject obj in lowLevelGrid)
            {





                Vector2 u = v2 - v1;
                Vector2 v = v4 - v1;
                Vector2 w = obj.position - v1;

                float s = Vector2.Dot(w, u) / Vector2.Dot(u, u);
                float t = Vector2.Dot(w, v) / Vector2.Dot(v, v);

                ///////ADD SHIT HERE LATER FOR WORMHOLES
                if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
                {
                    foreach (GameObject star in obj.starsInSquare)
                    {
                        slimStarList.Add(star);
                    }
                }
            }

            //Debug
            Debug.Log("SlimStarList.Count = " + slimStarList.Count);
            foreach(GameObject tempStar in slimStarList)
            {
                Debug.Log("Starname: " + tempStar.name);
                int radius = 10;

                GameObject circleObject = new GameObject("circleObject");
                LineRenderer circleMaker = circleObject.AddComponent<LineRenderer>();
                circleMaker.material = new Material(Shader.Find("Sprites/Default"));

                circleMaker.startColor = Color.yellow;
                circleMaker.endColor = Color.yellow;
                circleMaker.startWidth = 1f;
                circleMaker.endWidth = 1f;

                int steps = (int)MathF.Round(2 * MathF.PI * radius);
                circleMaker.positionCount = (steps) + 2;

                for (int i = 0; i < (steps) + 2; i++)
                {
                    float circumferenceProgress = (float)i / steps;

                    float currentRadian = (circumferenceProgress * 2 * Mathf.PI);

                    float xScaled = Mathf.Cos(currentRadian);
                    float yScaled = Mathf.Sin(currentRadian);

                    float x = xScaled * radius;
                    float y = yScaled * radius;

                    Vector2 position = new Vector3(x, y) + tempStar.transform.position;

                    circleMaker.SetPosition(i, position);
                }
                circleMaker.material = new Material(Shader.Find("Sprites/Default"));
            }

            return slimStarList;




            /*            //y = mx + b type shi fr

                        float m = (startStar.transform.position.y - endstar.transform.position.y) / (startStar.transform.position.x - endstar.transform.position.x);
                        float b = -(m * startStar.transform.position.x) + startStar.transform.position.y;
                        float bTop = b + (lineLength / 2);
                        float bBottom = b - (lineLength / 2);

            *//*            float mHigh*//*
                        float bHigh = m + lineLength;
                        float bLow = m - lineLength;

                        if (  ()  )*/


        }




        public class GridObject
        {
            public GridObject parent;
            public GridObject[] children;
            public int level;
            public bool isLowest;
            public int sideLength;
            public Vector2Int position;
            public List<GameObject> starsInSquare;
            public GridObject(GridObject parent, int level, int sideLength, Vector2Int position)
            {
                this.parent = parent;
                this.level = level;
                this.sideLength = sideLength;
                this.position = position;
                
            }
            public void calculateStarsInSquare(GridObject p)
            {
                starsInSquare = new List<GameObject>();
                foreach (GameObject star in p.starsInSquare)
                {
                    Vector2 pos = star.transform.position;
                    Vector2Int objPos = this.position;
                    float side = this.sideLength / 2;
                    if ((pos.x <= objPos.x + side) && (pos.y <= objPos.y + side) && (pos.x >= objPos.x - side) && (pos.y >= objPos.y - side))
                    {
                        starsInSquare.Add(star);
                    }
                }

            }
        }

        public void calculateGraph(List<GameObject> dumbStarList)
        {
            adjacencyList = new List<Node>[dumbStarList.Count];

            //Brute Forceish
            for (int i = 0; i < dumbStarList.Count; i++)
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
            Debug.Log("Adjacency length: "+adjacencyList.Length);
            return adjacencyList;
        }
    }


}


