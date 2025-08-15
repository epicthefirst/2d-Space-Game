using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject currentCarrier;
    private List<GameObject> tempList = new List<GameObject>();
    private List<GameObject> preFabList = new List<GameObject>();
    private Vector2 originalSizeDelta;

    Pathfinder.Graph graph;


    // Start is called before the first frame update
    void Start()
    {
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.SetActive(false);
    }
    public void init(GameObject carrier)
    {
        gameObject.SetActive(true);
        isActive = true;
        currentCarrier = carrier;
        tempList = carrier.GetComponent<ShipController>().starWaypoints;
        updateUI(tempList);
        uIManager.isRoutePlannerActive = true;

        graph = new Pathfinder.Graph(uIManager.starList, 10, 69);
        graph.calculateGridSquaresTree(1024, 4);
    }
    public void addStar(GameObject star)
    {
        if (tempList.Count != 0 && Mathf.RoundToInt(Vector2.Distance(star.transform.position, tempList[tempList.Count-1].transform.position)) > tempList[tempList.Count - 1].GetComponent<StarScript>().Range)
        {
            Debug.Log("We runnin da calcs");

            graph.calculateGraph(graph.dumbedListCalculator(tempList[tempList.Count - 1], star));
            pathfinder.calculate(graph, 0, 10);
        }
        else
        {
            tempList.Add(star);
        }
        

        updateUI(tempList);
    }
    public void updateUI(List<GameObject> starList)
    {
        Vector2 pos = originalSizeDelta;
        for (int i = 0; i < starList.Count; i++)
        {
            //, new Vector3(0, -80 - (i * 40)), Quaternion.identity
            GameObject obj = Instantiate(preFab, gameObject.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -90 - (i * 40));
            pos += new Vector2(0, 40);
            obj.GetComponent<TextObjectResizer>().write(starList[i].GetComponent<StarScript>().Name);
            preFabList.Add(obj);


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
}
