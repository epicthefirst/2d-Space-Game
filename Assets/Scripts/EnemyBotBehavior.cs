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
    private Dictionary<int, StarScript> IdToScript = new Dictionary<int, StarScript>();
    private Pathfinder.Graph knownGraph;
    private System.Random random;
    private int money;

    private Pathfinder.BinaryHeap econCostHeap;
    private Pathfinder.BinaryHeap industryCostHeap;
    private Pathfinder.BinaryHeap scienceCostHeap;

    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars, System.Random random, GameInformation gameInformation, MapGeneration mapGenerationScript)
    {
        econCostHeap = new Pathfinder.BinaryHeap();
        industryCostHeap = new Pathfinder.BinaryHeap();
        scienceCostHeap = new Pathfinder.BinaryHeap();

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
        if (stupidCounter % 12 == 0)
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
        money += econCostHeap.Size;
    }
    public void newCycle(object sender, NewCycleEvent e)
    {
        //checkStars();
        money += econCostHeap.Size * 12;
        buyInfrastructure();
        
    }
    public void checkStars()
    {

        econCostHeap = new Pathfinder.BinaryHeap();
        industryCostHeap = new Pathfinder.BinaryHeap();
        scienceCostHeap = new Pathfinder.BinaryHeap();

        foreach (GameObject star in ownedStars)
        {
            StarScript s = star.GetComponent<StarScript>();
            econCostHeap.Insert(s.selfId, s.GetEconPrice());
            industryCostHeap.Insert(s.selfId, s.GetIndustryPrice());
            scienceCostHeap.Insert(s.selfId, s.GetSciencePrice());
            /////////WORK ON ME PLEASE
        }
    }
    public void buyInfrastructure()
    {
        int allocatedFunds = Mathf.RoundToInt(money / 3);
        Debug.LogWarning(allocatedFunds);
        buyEcon(allocatedFunds);
        buyIndustry(allocatedFunds);
        buyScience(allocatedFunds);
        
    }
    public void buyEcon(int funds)
    {
        int node = 0;
        while (funds > econCostHeap.elements[0].distance)
        {
            funds -= econCostHeap.elements[0].distance;
            money -= econCostHeap.elements[0].distance;
            node = econCostHeap.elements[0].node;
            StarScript poppedStar = IdToScript[econCostHeap.Pop()];
            poppedStar.EconCount++;
            econCostHeap.Insert(node, poppedStar.GetEconPrice());
        }
    }
    public void buyIndustry(int funds)
    {
        int node = 0;
        while (funds > industryCostHeap.elements[0].distance)
        {
            funds -= industryCostHeap.elements[0].distance;
            money -= industryCostHeap.elements[0].distance;
            node = industryCostHeap.elements[0].node;
            StarScript poppedStar = IdToScript[industryCostHeap.Pop()];
            poppedStar.IndustryCount++;
            industryCostHeap.Insert(node, poppedStar.GetIndustryPrice());
        }
    }
    public void buyScience(int funds)
    {
        int node = 0;
        while (funds > scienceCostHeap.elements[0].distance)
        {
            funds -= scienceCostHeap.elements[0].distance;
            money -= scienceCostHeap.elements[0].distance;
            node = scienceCostHeap.elements[0].node;
            StarScript poppedStar = IdToScript[scienceCostHeap.Pop()];
            poppedStar.ScienceCount++;
            scienceCostHeap.Insert(node, poppedStar.GetSciencePrice());
        }
    }


    public void checkToExpand()
    {
        //Debug.LogError("Amount of stars in emprie: " + ownedStars.Count);
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
            StarScript s = star.GetComponent<StarScript>();
            IdToScript.Add(s.selfId, s);
            ownedStars.Add(star);
            econCostHeap.Insert(s.selfId, s.GetEconPrice());
            industryCostHeap.Insert(s.selfId, s.GetIndustryPrice());
            scienceCostHeap.Insert(s.selfId, s.GetSciencePrice());
        }
        else
        {
            Debug.LogError("Updating star");
            updateStar(star);
        }
        
    }
    public void updateStar(GameObject star)
    {
        StarScript s = star.GetComponent<StarScript>();

        econCostHeap.RemoveNode(econCostHeap.FindNode(s.selfId));
        industryCostHeap.RemoveNode(industryCostHeap.FindNode(s.selfId));
        scienceCostHeap.RemoveNode(scienceCostHeap.FindNode(s.selfId));

        econCostHeap.Insert(s.selfId, s.GetEconPrice());
        industryCostHeap.Insert(s.selfId, s.GetIndustryPrice());
        scienceCostHeap.Insert(s.selfId, s.GetSciencePrice());

    }
    public void removeStar(GameObject star)
    {
        ownedStars.Remove(star);
    }

}
