
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;



public class ShipController : MonoBehaviour
{
    public DrawLine lineDrawer;
    private GameObject startStar;
    private GameObject endStar;
    public GameObject dockedStar;
    private Vector2 vector;
    private int speedPerTick = 10;
    private int time;
    private int timeLeft;
    private float angle;
    private float distance;
    private Vector2 currentPosition;
    private float moveX;
    private float moveY;
    public int Owner;
    private Button nextTickButton;
    private StarScript startStarScript;

    private PlayerScript ownerScript;
    private int slingshotMultCount;
    //Expires on x tick
    private bool wantToSlingshot;
    private GameObject slingStar;

    public int ShipCount;
    public string Name;
    public bool hasSpecialist;
    public string Specialist = null;

    public int totalTimeLeft;

    //RoutePlanner

    //In order of going to visit
    public List<GameObject> starWaypoints;

    public void Init(Button nextTickButton, GameObject startStar, int shipShipCountAdd, int carrierCount, PlayerScript ownerScript, DrawLine drawline)
    {
        lineDrawer = drawline;
        this.ownerScript = ownerScript;
        startStarScript = startStar.GetComponent<StarScript>();
        ShipCount += shipShipCountAdd;
        this.nextTickButton = nextTickButton;
        gameObject.GetComponent<Renderer>().enabled = false;
        Owner = startStarScript.Owner;
        this.startStar = startStar;

        //Debug.Log(ownerScript);
        //Debug.Log(carrierData.shipCount + carrierData.name + carrierData.hasSpecialist);
        //ownerScript.test();
        ownerScript.newCarrier(gameObject);


        
        startStarScript.ReduceShipCount(shipShipCountAdd);
        startStarScript.AttachCarrier(1, gameObject);
    }
    public void SendToStar(GameObject endStar)
    {
        this.endStar = endStar;

        timeLeft = Pathfinder.tripCalc(startStar, endStar, speedPerTick);
        nextTickButton.onClick.AddListener(NewTick);
        nextTickButton.onClick.AddListener(LeavingTick);

        Debug.Log(time);
    }
    public void StartJourney()
    {
        dockedStar.GetComponent<StarScript>().DetachCarrier(1, gameObject);
        if (starWaypoints.Count == 0)
        {
            Debug.LogError("This ain't supposed to happen twin");
        }
        else
        {

            endStar = starWaypoints[0];
            time = Pathfinder.tripCalc(startStar, endStar, speedPerTick);
            timeLeft = Pathfinder.tripCalc(startStar, endStar, speedPerTick);
            

            foreach (GameObject obj in starWaypoints)
            {
                totalTimeLeft += Pathfinder.tripCalc(startStar, endStar, speedPerTick);
            }
            Debug.Log(timeLeft);
            Debug.Log(totalTimeLeft);

            nextTickButton.onClick.AddListener(NewTick);
            nextTickButton.onClick.AddListener(LeavingTick);
        }
    }
    public void SetNewWaypoints(List<GameObject> wayPoints)
    {
        starWaypoints.Clear();
        starWaypoints.AddRange(wayPoints);
        List<Vector2> tempVectorList = new List<Vector2>();
        tempVectorList.Add(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        foreach (GameObject waypoint in wayPoints)
        {
            tempVectorList.Add(new Vector2(waypoint.transform.position.x, waypoint.transform.position.y));
        }
        lineDrawer.drawLinePath(tempVectorList, this);
        Debug.Log("Updated star waypoints for: "+this +", with a length of " + starWaypoints.Count);
    }
    public List<GameObject> GetWaypoints()
    {
        List<GameObject> CopyList = new List<GameObject>();
        CopyList.AddRange(starWaypoints);
        return CopyList;
    }
    void NewTick()
    {

        Debug.Log(timeLeft);
        timeLeft--;
        totalTimeLeft--;

        if (timeLeft == 0) 
        {
            
            nextTickButton.onClick.RemoveListener(NewTick);
            if (wantToSlingshot)
            {
                SlingshotAtStar();
                return;
            }
            ArrivedAtStar();
            return;
        }

        moveX = endStar.transform.position.x - startStar.transform.position.x;
        moveY = endStar.transform.position.y - startStar.transform.position.y;
        Vector2 vector = new Vector2(moveX, moveY);
        gameObject.transform.position += (Vector3)vector / time;
        Debug.Log(currentPosition +"/"+ timeLeft);


        
    }
    void LeavingTick()
    {
        // Remove the child from the parent
        gameObject.transform.parent = null;
        gameObject.GetComponent<Renderer>().enabled = true;

        startStarScript.DetachCarrier(1, gameObject);
        nextTickButton.onClick.RemoveListener(LeavingTick);
    }
    void ArrivedAtStar()
    {

        slingshotMultCount = 0;
        Debug.Log("Arrived at star");
        dockedStar = endStar;
        starWaypoints.RemoveAt(0);
        endStar = starWaypoints[0];

        gameObject.GetComponent<Renderer>().enabled = false;
        
        
        startStar = dockedStar;
        StarScript starScript = dockedStar.GetComponent<StarScript>();
        starScript.ShipInbound(ShipCount, Owner, gameObject);
        gameObject.transform.parent = dockedStar.transform;
        
        if (starWaypoints.Count > 0)
        {
            StartJourney();
        }
    }
    void SlingshotAtStar()
    {
        if (endStar.GetComponent<StarScript>().isGoingToSlingshot(0))
        {
            Debug.Log("Commencing gravity assist on star: " + endStar.name);
            slingshotMultCount++;
            slingStar = endStar;
            StarScript starScript = slingStar.GetComponent<StarScript>();
            starScript.startSlingshot(gameObject);
            gameObject.transform.parent = dockedStar.transform;
        }
        else
        {
            Debug.LogWarning("Slingshot failed, please check this");
            ArrivedAtStar();
        }
    }
}
