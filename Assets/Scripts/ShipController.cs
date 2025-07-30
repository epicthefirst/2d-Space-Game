
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;



public class ShipController : MonoBehaviour
{
    private GameObject startStar;
    private GameObject endStar;
    private GameObject dockedStar;
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

    //RoutePlanner

    //In order of going to visit
    public List<GameObject> starWaypoints;

    public void Init(Button nextTickButton, GameObject startStar, int shipShipCountAdd, int carrierCount, PlayerScript ownerScript)
    {
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
    void NewTick()
    {

        Debug.Log(timeLeft);
        timeLeft--;

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
        gameObject.GetComponent<Renderer>().enabled = false;
        
        dockedStar = endStar;
        StarScript starScript = dockedStar.GetComponent<StarScript>();
        starScript.ShipInbound(ShipCount, Owner, gameObject);
        gameObject.transform.parent = dockedStar.transform;
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
