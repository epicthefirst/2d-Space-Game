using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBotBehavior : MonoBehaviour
{
    //General Bot Logic
    public GameInformation.PlayerClass bot;
    public UIManager uIManager;
    public MapGeneration mapGenerationScript;
    public Pathfinder pathfinderScript;
    public GameInformation gameInformation;
    public int carrierNameIncrement;

    private List<GameObject> carrierList = new List<GameObject>();


    private List<GameObject> ownedStars = new List<GameObject>();
    private Pathfinder.Graph knownGraph;
    private System.Random random;
    private int money;
    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars, System.Random random, GameInformation gameInformation, MapGeneration mapGenerationScript)
    {
        this.gameInformation = gameInformation;
        this.random = random;
        this.bot = bot;
        this.ownedStars = ownedStars;
        money = gameInformation.playerMoney;
        this.mapGenerationScript = mapGenerationScript;
        CycleEventManager.OnTick += newTick;
        
    }

    public void newTick(object sender, NewTickEvent e)
    {
        money += 50;
        checkToExpand();
        //if (money > gameInformation.carrierCost)
        //{
        //    checkToExpand();
        //}
    }

    public void checkToExpand()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (GameObject star in ownedStars)
        {
            StarScript starScript = star.GetComponent<StarScript>();
            Debug.LogError("Checked star");
            if (starScript.CarrierShipTally + starScript.GarrisonCount >= 100)
            {
                if (starScript.GarrisonCount >= 100 && money >= gameInformation.carrierCost)
                {
                    Debug.LogError("Passed check");

                    //tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star).Except(ownedStars).ToList();
                    tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);

                    if (tempList.Count == 0)
                    {
                        tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);
                    }

                    GameObject chosenStar = tempList[random.Next(0, tempList.Count - 1)];



                    money -= gameInformation.carrierCost;
                    GameObject c = Instantiate(gameInformation.shipPrefab, star.transform.position, Quaternion.identity);
                    c.transform.parent = star.transform;
                    ShipController shipController = c.GetComponent<ShipController>();

                    star.GetComponent<StarScript>().AttachCarrier(c);
                    shipController.dockedStar = star;

                    shipController.Init(carrierNameGenerator(), star, 100, bot);
                    List<GameObject> temp = shipController.GetWaypoints();
                    temp.Add(chosenStar);
                    shipController.SetNewWaypoints(temp);
                    shipController.StartJourney();
                }
                else if (star.GetComponent<StarScript>().CarrierCount > 0)
                {
                    //tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star).Except(ownedStars).ToList();
                    tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);

                    if (tempList.Count == 0)
                    {
                        tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);
                    }

                    GameObject chosenStar = tempList[random.Next(0, tempList.Count - 1)];

                    ShipController sc = star.GetComponent<StarScript>().CarrierList[0].GetComponent<ShipController>();

                    sc.Init(carrierNameGenerator(), star, 100, bot);
                    List<GameObject> temp = sc.GetWaypoints();
                    temp.Add(chosenStar);
                    sc.SetNewWaypoints(temp);
                    sc.StartJourney();


                }
                else
                {
                    Debug.LogError("BAD STUFF HERE");
                }
            }
        }

/*        mapGenerationScript.graphFullSpeed*/
    }

    public string carrierNameGenerator()
    {
        carrierNameIncrement++;
        return bot.name + " " + carrierNameIncrement.ToString();
    }

    public void addCarrier(GameObject carrier)
    {
        if(!carrierList.Contains(carrier))
        {
            carrierList.Add(carrier);
        }
        else
        {
            Debug.LogError("Bad");
        }
        
    }
    public void removeCarrier(GameObject carrier)
    {
        carrierList.Remove(carrier);
    }
    public void addStar(GameObject star)
    {
        if (!ownedStars.Contains(star))
        {
            ownedStars.Add(star);
        }
        else
        {
            Debug.LogError("BAD");
        }
        
    }
    public void removeStar(GameObject star)
    {
        ownedStars.Remove(star);
    }

}
