using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RoutePlanner : MonoBehaviour
{
    [SerializeField] Button undoButton;
    [SerializeField] Button undoAllButton;
    [SerializeField] Toggle loopToggle;
    [SerializeField] Button saveButton;
    [SerializeField] Button saveAndEditButton;
    [SerializeField] TMP_Text carrierNameText;
    [SerializeField] TMP_Text shipCountText;
    [SerializeField] TMP_Text ETAText;
    [SerializeField] GameObject preFab;
    [SerializeField] UIManager uIManager;
    [SerializeField] Pathfinder pathfinder;

    public bool isActive;
    public DrawLine lineDrawer;

    public GameObject currentCarrier;
    public GameObject currentStar;
    private List<GameObject> tempList = new List<GameObject>();
    private List<GameObject> preFabList = new List<GameObject>();
    private Vector2 originalSizeDelta;
    private ShipController carrierScript;
    private List<Pathfinder.Graph.GridObject> gridObjects;

    GameObject tempPath;
    LineRenderer lr;


    Pathfinder.Graph graph;


    // Start is called before the first frame update
    void Start()
    {
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.SetActive(false);
    }
    public void init(GameObject carrier, GameObject currentStar)
    {

        tempPath = lineDrawer.makeTempPathObject();
        lr = tempPath.GetComponent<LineRenderer>();

        Debug.Log("Running init for: " + carrier.ToString() + " | " + currentStar.name);
        this.currentStar = currentStar;
        gameObject.SetActive(true);
        isActive = true;
        carrierScript = carrier.GetComponent<ShipController>();
        currentCarrier = carrier;
        tempList = carrierScript.GetWaypoints();
        Debug.Log(carrier.GetComponent<ShipController>().starWaypoints.Count);
        updateUI(tempList);
        uIManager.isRoutePlannerActive = true;
        /*        graph = new Pathfinder.Graph(uIManager.starList, 10, 69);*/
        /*        graph.calculateGridSquaresTree(1024, 4);*/

        gridObjects = pathfinder.calculateGridSquaresTree(1024, 4, uIManager.starList);
    }
    public void addStar(GameObject star)
    {
        if (tempList.Count == 0)
        {
            if (Mathf.RoundToInt(Vector2.Distance(star.transform.position, currentStar.transform.position)) <= currentStar.GetComponent<StarScript>().Range)
            {
                tempList.Add(star);
            }
            else if (star == currentStar)
            {
                tempList.Add(star);
            }
            else
            {
                Debug.Log("Too far, running algorithm");

                /*graph.calculateGraph(graph.dumbedListCalculator(currentStar, star));*/
                Pathfinder.Graph tinyGraph = new Pathfinder.Graph(Pathfinder.dumbedListCalculator(gridObjects, star, currentStar), 10, 67);
                tempList.AddRange(pathfinder.calculate(tinyGraph, tinyGraph.findStarIndex(star), tinyGraph.findStarIndex(currentStar)));

/*                tempList.AddRange(pathfinder.calculate(graph, graph.findStarIndex(star), graph.findStarIndex(currentStar)));*/
            }
        }
        else if (Mathf.RoundToInt(Vector2.Distance(star.transform.position, tempList[tempList.Count - 1].transform.position)) > tempList[tempList.Count - 1].GetComponent<StarScript>().Range)
        {
            Debug.Log("We runnin da calcs");

            /*graph.calculateGraph(graph.dumbedListCalculator(tempList[tempList.Count - 1], star));*/
            pathfinder.calculate(graph, graph.findStarIndex(tempList[tempList.Count - 1]), 10);
        }
        else
        {
            tempList.Add(star);
        }


        /*        if(tempList == null)
                {
                    Debug.LogWarning("tempList = null");
                    if (Mathf.RoundToInt(Vector2.Distance(star.transform.position, carrierScript.dockedStar.transform.position)) < carrierScript.dockedStar.GetComponent<StarScript>().Range)
                    {
                        tempList.Add(star);
                    }
                    else
                    {
                        Debug.LogError("Error here?");
                    }
                }
                else if (Mathf.RoundToInt(Vector2.Distance(star.transform.position, tempList[tempList.Count-1].transform.position)) > tempList[tempList.Count - 1].GetComponent<StarScript>().Range)
                {
                    Debug.Log("We runnin da calcs");

                    graph.calculateGraph(graph.dumbedListCalculator(tempList[tempList.Count - 1], star));
                    pathfinder.calculate(graph, 0, 10);
                }
                else
                {
                    tempList.Add(star);
                }*/


        updateUI(tempList);
    }

    public void updateUI(List<GameObject> starList)
    {

        Vector2 pos = originalSizeDelta;
        foreach (GameObject p in preFabList)
        {
            Destroy(p);
        }
        preFabList.Clear();
        lr.positionCount = starList.Count + 1;
        lr.SetPosition(0, currentStar.transform.position);
        for (int i = 0; i < starList.Count; i++)
        {
            //, new Vector3(0, -80 - (i * 40)), Quaternion.identity
            GameObject obj = Instantiate(preFab, gameObject.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -90 - (i * 40));
            pos += new Vector2(0, 40);
            obj.GetComponent<TextObjectResizer>().write(starList[i].GetComponent<StarScript>().Name);
            preFabList.Add(obj);

            lr.SetPosition(i + 1, starList[i].transform.position);

        }
        gameObject.GetComponent<RectTransform>().sizeDelta = pos;






    }
/*    public void test()
    {
        Debug.LogWarning("Testing");
        pathfinder.calculate(graph, 0, 10);
    }*/
    public void clear()
    {
        foreach(GameObject p in preFabList)
        {
            Destroy(p);
        }

        uIManager.isRoutePlannerActive = false;
        gameObject.SetActive(false);
        tempList.Clear();
        

    }

    public void undoOne()
    {
        if (preFabList != null)
        {
            Debug.Log(tempList.Count);
            
            tempList.RemoveAt(tempList.Count - 1);
            uIManager.MoveCircle(tempList[tempList.Count - 1].transform.position, tempList[tempList.Count - 1].GetComponent<StarScript>().Range);
            Debug.Log(tempList.Count);
            updateUI(tempList);

            

        }
        else
        {
            Debug.Log("ts empty twin");
        }
    }

    public void save()
    {
        if (currentCarrier.GetComponent<ShipController>().starWaypoints != null)
        {
            Debug.Log("Yay");
            Debug.Log(tempList.Count);
            Debug.Log(currentCarrier.GetComponent<ShipController>().GetWaypoints().Count);
        }
        Destroy(tempPath);
        currentCarrier.GetComponent<ShipController>().SetNewWaypoints(tempList);
        currentCarrier.GetComponent<ShipController>().StartJourney();
        Debug.LogError("Good");
        clear();
    }
}
