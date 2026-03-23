using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBotBehavior : MonoBehaviour
{
    //Testing
    int stupidCounter = 0;



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

    private Pathfinder.BinaryHeap econCostHeap;
    private Pathfinder.BinaryHeap industryCostHeap;
    private Pathfinder.BinaryHeap scienceCostHeap;

    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars, System.Random random, GameInformation gameInformation, MapGeneration mapGenerationScript)
    {
        this.gameInformation = gameInformation;
        this.random = random;
        this.bot = bot;
        this.ownedStars = ownedStars;
        money = gameInformation.playerMoney;
        this.mapGenerationScript = mapGenerationScript;
        CycleEventManager.OnPreTick += preTick;
        CycleEventManager.OnTick += newTick;
        CycleEventManager.OnCycle += newCycle;



    }

    public void preTick(object sender, PreTickEvent e)
    {
        stupidCounter++;
        if (stupidCounter % 3 == 0)
        {
            checkToExpand();
        }
        
        //if (money > gameInformation.carrierCost)
        //{
        //    checkToExpand();
        //}
    }
    public void newTick(object sender, NewTickEvent e)
    {
        money += 50;

    }
    public void newCycle(object sender, NewCycleEvent e)
    {
        checkStars();
        buyInfrastructure();
    }
    public void checkStars()
    {
        int i = 0;
        econCostHeap = new Pathfinder.BinaryHeap();
        industryCostHeap = new Pathfinder.BinaryHeap();
        scienceCostHeap = new Pathfinder.BinaryHeap();

        foreach (GameObject star in ownedStars)
        {
            StarScript s = star.GetComponent<StarScript>();
            econCostHeap.Insert(i, s.GetEconPrice());
            industryCostHeap.Insert(i, s.GetIndustryPrice());
            scienceCostHeap.Insert(i, s.GetSciencePrice());
            /////////WORK ON ME PLEASE
            i++;
        }
    }
    public void buyInfrastructure()
    {
        int allocatedFunds = Mathf.RoundToInt(money / 4);

        buyEcon(allocatedFunds);
        buyIndustry(allocatedFunds);
        buyScience(allocatedFunds);
        
    }
    public void buyEcon(int funds)
    {
        while (funds > econCostHeap.elements[0].distance)
        {
            funds -= econCostHeap.elements[0].distance;
            money -= econCostHeap.elements[0].distance;
            ownedStars[econCostHeap.Pop()].GetComponent<StarScript>().EconCount++;
        }
    }
    public void buyIndustry(int funds)
    {
        while (funds > industryCostHeap.elements[0].distance)
        {
            funds -= industryCostHeap.elements[0].distance;
            money -= econCostHeap.elements[0].distance;
            ownedStars[industryCostHeap.Pop()].GetComponent<StarScript>().IndustryCount++;
        }
    }
    public void buyScience(int funds)
    {
        while (funds > scienceCostHeap.elements[0].distance)
        {
            funds -= scienceCostHeap.elements[0].distance;
            money -= econCostHeap.elements[0].distance;
            ownedStars[scienceCostHeap.Pop()].GetComponent<StarScript>().ScienceCount++;
        }
    }


    public void checkToExpand()
    {
        Debug.LogError("Amount of stars in emprie: " + ownedStars.Count);
        List<GameObject> tempList = new List<GameObject>();
        foreach (GameObject star in ownedStars)
        {
            StarScript starScript = star.GetComponent<StarScript>();
            Debug.LogError("Checked star");
            starScript.Refresh();
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
                    List<GameObject> cl = star.GetComponent<StarScript>().CarrierList;
                    GameObject movedCarrier = null;
                    for (int i = 0; i < star.GetComponent<StarScript>().CarrierCount; i++)
                    {
                        if (cl[i].GetComponent<ShipController>().GetWaypoints().Count == 0)
                        {
                            movedCarrier = cl[i];
                            break;
                        }
                    }
                    if (movedCarrier != null)
                    {
                        Debug.LogError("Moving preexisting carrier");
                        //tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star).Except(ownedStars).ToList();
                        tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);

                        if (tempList.Count == 0)
                        {
                            tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star);
                        }

                        GameObject chosenStar = tempList[random.Next(0, tempList.Count - 1)];

                        ShipController sc = movedCarrier.GetComponent<ShipController>();

                        List<GameObject> temp = sc.GetWaypoints();
                        temp.Add(chosenStar);
                        sc.SetNewWaypoints(temp);
                        sc.StartJourney();


                    }
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
