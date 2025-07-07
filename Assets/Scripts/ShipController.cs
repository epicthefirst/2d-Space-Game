
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ShipController : MonoBehaviour
{
    private GameObject startStar;
    private GameObject endStar;
    private GameObject dockedStar;
    private Vector2 vector;
    private float speedPerTick = 10;
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

    public int ShipCount;
    public string Name;
    public bool hasSpecialist;
    public string Specialist = null;

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
        distance = Vector2.Distance(startStar.transform.position, endStar.transform.position);
        time = Mathf.RoundToInt(distance / speedPerTick);
        timeLeft = time;
        nextTickButton.onClick.AddListener(NewTick);
        nextTickButton.onClick.AddListener(LeavingTick);
    }
    void NewTick()
    {

        Debug.Log(timeLeft);
        timeLeft--;

        if (timeLeft == 0) 
        {
            nextTickButton.onClick.RemoveListener(NewTick);
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
        Debug.Log("Arrived at star");
        gameObject.GetComponent<Renderer>().enabled = false;
        
        dockedStar = endStar;
        StarScript starScript = dockedStar.GetComponent<StarScript>();
        starScript.ShipInbound(ShipCount, Owner, gameObject);
        gameObject.transform.parent = dockedStar.transform;
    }
}
