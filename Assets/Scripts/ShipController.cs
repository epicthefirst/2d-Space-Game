
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;



public class ShipController : MonoBehaviour
{
    private GameObject startStar;
    private GameObject endStar;
    public GameObject dockedStar;
    private Vector2 vector;
    public int speedPerTick = 10;
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
    public int totalWaitTimeLeft;

    public bool isLeavingNextTick;
    public bool inTransit;

    public GameObject linePath;
    public Material dottedLineMaterial;

    //RoutePlanner

    //In order of going to visit
    public List<GameObject> starWaypoints;

    public void Init(string name, Button nextTickButton, GameObject startStar, int shipShipCountAdd, int carrierCount, PlayerScript ownerScript, DrawLine drawline)
    {
        Name = name;
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
/*        startStarScript.AttachCarrier(gameObject);*/
    }
    public void StartJourney()
    {

        


        if (starWaypoints.Count == 0)
        {
            Debug.LogError("This ain't supposed to happen twin");
        }
        else
        {

            endStar = starWaypoints[0];
            if(startStar == endStar)
            {
                int counter = 0;

                while (counter < starWaypoints.Count)
                {
                    if(startStar == starWaypoints[counter])
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }
                Debug.Log("Waiting for " + counter + " ticks");
                WaitAtStar(counter);
                return;
            }
            time = Pathfinder.tripCalc(startStar, endStar, speedPerTick);
            timeLeft = Pathfinder.tripCalc(startStar, endStar, speedPerTick);


            isLeavingNextTick = true;
            gameObject.transform.eulerAngles = new Vector3(0, 0, 270 + (Mathf.Atan2(endStar.transform.position.y - startStar.transform.position.y, endStar.transform.position.x - startStar.transform.position.x) * Mathf.Rad2Deg));

            totalTimeLeft = 0;
            GameObject prevObj = startStar;
            foreach (GameObject obj in starWaypoints)
            {
                totalTimeLeft += Pathfinder.tripCalc(prevObj, obj, speedPerTick);
                prevObj = obj;
            }
            Debug.Log(timeLeft);
            Debug.Log(totalTimeLeft); //Fix me
            nextTickButton.onClick.AddListener(NewTick);

        }
    }
    public void WaitAtStar(int length)
    {
        if (length == 0)
        {
            Debug.LogError("Bad");
        }
        dockedStar = endStar;
        totalWaitTimeLeft = length;
        nextTickButton.onClick.AddListener(WaitTick);
    }
    public void ResetWaiting()
    {
        nextTickButton.onClick.RemoveListener(WaitTick);
        totalTimeLeft = 0;
    }
    public void WaitTick()
    {
        startStar = starWaypoints[0];
        starWaypoints.RemoveAt(0);

        totalWaitTimeLeft--;
        if (totalWaitTimeLeft <= 0)
        {


            nextTickButton.onClick.RemoveListener(WaitTick);

            if (starWaypoints.Count > 0)
            {


                StartJourney();
                Debug.Log("StartedJourney");
            }
        }

    }

    public void SetNewWaypoints(List<GameObject> wayPoints)
    {
        starWaypoints.Clear();
        starWaypoints.AddRange(wayPoints);

/*        List<Vector2> tempVectorList = new List<Vector2>();
        tempVectorList.Add(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        foreach (GameObject waypoint in wayPoints)
        {
            tempVectorList.Add(new Vector2(waypoint.transform.position.x, waypoint.transform.position.y));
        }*/
        
        updateLine();
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
        if (isLeavingNextTick)
        {
            gameObject.transform.parent = null;
            gameObject.GetComponent<Renderer>().enabled = true;

            dockedStar.GetComponent<StarScript>().DetachCarrier(gameObject);
            isLeavingNextTick = false;
        }
        else
        {
            inTransit = true;
        }

           
        Debug.Log(timeLeft);
        timeLeft--;
        totalTimeLeft--;

        if (timeLeft <= 0) 
        {
            gameObject.transform.position = endStar.transform.position;
            updateLine();

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
        Debug.Log(gameObject.transform.position + "/"+ timeLeft);


        updateLine();
    }
    public void updateLine()
    {
        {
            List<Vector2> pointList = new List<Vector2>();
            pointList.Add(gameObject.transform.position);
            foreach (GameObject obj in starWaypoints)
            {
                pointList.Add(obj.transform.position);
            }
            pointList = pointList.Distinct().ToList();
            if (linePath != null)
            {
                LineRenderer lr = linePath.GetComponent<LineRenderer>();

                

                lr.positionCount = pointList.Count;
                for (int i = 0; i < pointList.Count; i++)
                {
                    lr.SetPosition(i, pointList[i]);
                }
            }
            else
            {
                linePath = drawLinePath(pointList);
            }
        }
    }

    public GameObject drawLinePath(List<Vector2> pointList)
    {
        GameObject dottedPath = new GameObject("dottedPath");
        LineRenderer lineMaker = dottedPath.AddComponent<LineRenderer>();
        lineMaker.material = dottedLineMaterial;
        lineMaker.material.mainTextureScale = new Vector2(1.0f, 1.0f);
        lineMaker.startWidth = 1;
        lineMaker.endWidth = 1;

        lineMaker.textureMode = LineTextureMode.Tile;
        lineMaker.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        lineMaker.numCornerVertices = 1;

        lineMaker.positionCount = pointList.Count;

        for (int i = 0; i < pointList.Count; i++)
        {
            lineMaker.SetPosition(i, pointList[i]);
        }
        Debug.Log("Made path");
        return dottedPath;
    }
    //void LeavingTick()
    //{
    //    // Remove the child from the parent
    //    gameObject.transform.parent = null;
    //    gameObject.GetComponent<Renderer>().enabled = true;

    //    dockedStar.GetComponent<StarScript>().DetachCarrier(gameObject);
    //    nextTickButton.onClick.RemoveListener(LeavingTick);
    //}
    void ArrivedAtStar()
    {

        //KEEP IN MIND, MUST NOT BE DISCRIMINATE TO OWNERSHIP

        slingshotMultCount = 0;
        Debug.Log("Arrived at star");
        dockedStar = endStar;
        inTransit = false;

        starWaypoints.RemoveAt(0);
        endStar = null;

        gameObject.GetComponent<Renderer>().enabled = false;
        
        
        startStar = dockedStar;
        StarScript starScript = dockedStar.GetComponent<StarScript>();
        starScript.ShipInbound(ShipCount, Owner, gameObject);
        gameObject.transform.parent = dockedStar.transform;
        
        if (starWaypoints.Count > 0)
        {
            endStar = starWaypoints[0];

            StartJourney();
            Debug.Log("StartedJourney");
        }
        else
        {
            linePath.SetActive(false);
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
