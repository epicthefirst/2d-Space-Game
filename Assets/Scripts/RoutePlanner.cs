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

    [SerializeField] MapGeneration mapGeneration;

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
    GameObject line;


    Pathfinder.Graph graph;


    // Start is called before the first frame update
    void Start()
    {
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.SetActive(false);
    }
    public void newTickClear(object sender, CycleEvent e)
    {
        clear();
    }
    public void init(GameObject carrier, GameObject currentStar)
    {


        uIManager.NewTick += newTickClear;
        carrierScript = carrier.GetComponent<ShipController>();



        tempPath = lineDrawer.makeTempPathObject();
        lr = tempPath.GetComponent<LineRenderer>();

        Debug.Log("Running init for: " + carrier.ToString() + " | " + currentStar.name);
        this.currentStar = currentStar;
        gameObject.SetActive(true);
        isActive = true;
        
        currentCarrier = carrier;
        tempList = carrierScript.GetWaypoints();

        if (tempList.Count > 0)
        {
            uIManager.MoveCircle(tempList[tempList.Count - 1].transform.position, tempList[tempList.Count - 1].GetComponent<StarScript>().Range);
        }

        Debug.Log(carrier.GetComponent<ShipController>().starWaypoints.Count);
        updateUI(tempList);
        uIManager.isRoutePlannerActive = true;
        /*        graph = new Pathfinder.Graph(uIManager.starList, 10, 69);*/
        /*        graph.calculateGridSquaresTree(1024, 4);*/

        /*        gridObjects = pathfinder.calculateGridSquaresTree(1024, 4, uIManager.starList);*/
        graph = mapGeneration.graphFullSpeed;

        lineDrawer.removeCarrierPath(carrierScript);
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
                /*Pathfinder.Graph tinyGraph = new Pathfinder.Graph(Pathfinder.dumbedListCalculator(gridObjects, star, currentStar), 10, 67);*/
                tempList.AddRange(pathfinder.calculate(graph, graph.findStarIndex(star), graph.findStarIndex(currentStar)));

/*                tempList.AddRange(pathfinder.calculate(graph, graph.findStarIndex(star), graph.findStarIndex(currentStar)));*/
            }
        }
        else if (Mathf.RoundToInt(Vector2.Distance(star.transform.position, tempList[tempList.Count - 1].transform.position)) > tempList[tempList.Count - 1].GetComponent<StarScript>().Range)
        {
            Debug.Log("We runnin da calcs");

            /*graph.calculateGraph(graph.dumbedListCalculator(tempList[tempList.Count - 1], star));*/
            tempList.AddRange(pathfinder.calculate(graph, graph.findStarIndex(star), graph.findStarIndex(tempList[tempList.Count - 1])));
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

        if (lineDrawer.linePathDictionary.TryGetValue(carrierScript, out line))
        {
            line.SetActive(false);
        }
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

        Destroy(tempPath);
        uIManager.isRoutePlannerActive = false;
        gameObject.SetActive(false);
        tempList.Clear();

        if (line != null)
        {
            line.SetActive(true);
        }
        
    }

    public void undoOne()
    {
        if (tempList.Count > 0)
        {
            if (carrierScript.inTransit && tempList.Count < 2)
            {
                return;
            }
            Debug.Log(tempList.Count);
            
            tempList.RemoveAt(tempList.Count - 1);
            if (tempList.Count > 0)
            {
                uIManager.MoveCircle(tempList[tempList.Count - 1].transform.position, tempList[tempList.Count - 1].GetComponent<StarScript>().Range);
            }
            else
            {
                uIManager.RemoveCircle();
            }

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
        lineDrawer.updateCarrier(carrierScript);
        lineDrawer.linePathDictionary[carrierScript].SetActive(true);
        if (tempList.Count > 0)
        {
            currentCarrier.GetComponent<ShipController>().ResetWaiting();
            currentCarrier.GetComponent<ShipController>().StartJourney();
        }

        clear();
    }
}
