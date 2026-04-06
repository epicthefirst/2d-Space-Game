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
    private Pathfinder.ObjectBinaryHeap carrierSizeHeap = new Pathfinder.ObjectBinaryHeap();
    private Pathfinder.ObjectBinaryHeap idleCarrierHeap = new Pathfinder.ObjectBinaryHeap();


    private List<GameObject> ownedStars = new List<GameObject>();
    private Dictionary<int, StarScript> IdToScript = new Dictionary<int, StarScript>();
    private Pathfinder.Graph knownGraph;
    private List<GameObject> targetStars = new List<GameObject>();
    private List<GameObject> candidateStars = new List<GameObject>();


    private System.Random random;
    private int money;

    private Pathfinder.ObjectBinaryHeap garrisonHeap = new Pathfinder.ObjectBinaryHeap();
    private Pathfinder.ObjectBinaryHeap econCostHeap = new Pathfinder.ObjectBinaryHeap();
    private Pathfinder.ObjectBinaryHeap industryCostHeap = new Pathfinder.ObjectBinaryHeap();
    private Pathfinder.ObjectBinaryHeap scienceCostHeap = new Pathfinder.ObjectBinaryHeap();

    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars, System.Random random, GameInformation gameInformation, MapGeneration mapGenerationScript, Pathfinder pathfinder)
    {
        //Change me later, controls bot's vision
        //knownGraph = mapGenerationScript.graphFullSpeed;


        this.gameInformation = gameInformation;
        this.random = random;
        this.bot = bot;
        this.ownedStars = ownedStars;
        money = gameInformation.playerMoney;
        this.mapGenerationScript = mapGenerationScript;
        this.pathfinderScript = pathfinder;
        CycleEventManager.OnPreTick += preTick;
        CycleEventManager.OnTick += newTick;
        CycleEventManager.OnCycle += newCycle;


        checkStars();
    }

    public void preTick(object sender, PreTickEvent e)
    {
        if (targetStars.Count < Mathf.CeilToInt(Mathf.Sqrt(ownedStars.Count)))
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
        Debug.LogError("Checked stars: " + ownedStars.Count);
        econCostHeap = new Pathfinder.ObjectBinaryHeap();
        industryCostHeap = new Pathfinder.ObjectBinaryHeap();
        scienceCostHeap = new Pathfinder.ObjectBinaryHeap();
        garrisonHeap = new Pathfinder.ObjectBinaryHeap();

        foreach (GameObject star in ownedStars)
        {
            StarScript s = star.GetComponent<StarScript>();
            econCostHeap.Insert(star, s.GetEconPrice());
            industryCostHeap.Insert(star, s.GetIndustryPrice());
            scienceCostHeap.Insert(star, s.GetSciencePrice());

            garrisonHeap.Insert(star, s.GarrisonCount);
            /////////WORK ON ME PLEASE
        }
    }
    public void buyInfrastructure()
    {
        Debug.LogWarning(econCostHeap.Size);
        int allocatedFunds = Mathf.RoundToInt(money / 3);
        Debug.LogWarning(allocatedFunds);
        buyEcon(allocatedFunds);
        buyIndustry(allocatedFunds);
        buyScience(allocatedFunds);
        
    }
    public void buyEcon(int funds)
    {
        Debug.Log(econCostHeap.Size);
        GameObject node;
        while (funds > econCostHeap.elements[0].value)
        {
            funds -= econCostHeap.elements[0].value;
            money -= econCostHeap.elements[0].value;
            node = econCostHeap.elements[0].node;
            StarScript poppedStarScript = econCostHeap.Pop().GetComponent<StarScript>();
            poppedStarScript.EconCount++;
            econCostHeap.Insert(node, poppedStarScript.GetEconPrice());
        }
    }
    public void buyIndustry(int funds)
    {
        GameObject node;
        while (funds > industryCostHeap.elements[0].value)
        {
            funds -= industryCostHeap.elements[0].value;
            money -= industryCostHeap.elements[0].value;
            node = industryCostHeap.elements[0].node;
            StarScript poppedStarScript = industryCostHeap.Pop().GetComponent<StarScript>();
            poppedStarScript.IndustryCount++;
            econCostHeap.Insert(node, poppedStarScript.GetIndustryPrice());
        }
    }
    public void buyScience(int funds)
    {
        GameObject node;
        while (funds > scienceCostHeap.elements[0].value)
        {
            funds -= scienceCostHeap.elements[0].value;
            money -= scienceCostHeap.elements[0].value;
            node = scienceCostHeap.elements[0].node;
            StarScript poppedStarScript = scienceCostHeap.Pop().GetComponent<StarScript>();
            poppedStarScript.ScienceCount++;
            econCostHeap.Insert(node, poppedStarScript.GetSciencePrice());
        }
    }


    public void checkToExpand()
    {
        knownGraph = mapGenerationScript.graphFullSpeed;

        int target = Mathf.CeilToInt(Mathf.Sqrt(ownedStars.Count));
        if (candidateStars.Count < target)
        {
            foreach(GameObject star in ownedStars)
            {
                candidateStars.AddRange(knownGraph.getStarNeighbors(star).Except(ownedStars));
            }
        }
        List<GameObject> tempList = candidateStars.GetRange(0, target);

        foreach(GameObject star in tempList)
        {
            if (idleCarrierHeap.Size > 0)
            {
                Debug.LogWarning("Sending carrier");
                ShipController tempCarrierScript = idleCarrierHeap.Pop().GetComponent<ShipController>();
                //tempCarrierScript.SetNewWaypoints(pathfinderScript.calculate(knownGraph, knownGraph.findStarIndex(tempCarrierScript.dockedStar), knownGraph.findStarIndex(star)));
                tempCarrierScript.SetNewWaypoints(pathfinderScript.calculate(knownGraph, knownGraph.findStarIndex(star), knownGraph.findStarIndex(tempCarrierScript.dockedStar)));
                tempCarrierScript.StartJourney();
            }
            //Fix this part later
            if(money >= gameInformation.carrierCost && garrisonHeap.elements[0].value > 0 && carrierList.Count <= ownedStars.Count + 5)
            {
                money -= gameInformation.carrierCost;
                GameObject poppedStar = garrisonHeap.Pop();
                StarScript poppedStarScript = poppedStar.GetComponent<StarScript>();

                GameObject c = GameObject.Instantiate(gameInformation.shipPrefab, poppedStar.transform.position, Quaternion.identity) as GameObject;
                c.transform.parent = poppedStar.transform;
                ShipController shipController = c.GetComponent<ShipController>();

                poppedStarScript.AttachCarrier(c);
                shipController.dockedStar = poppedStar;
                shipController.Init(carrierNameGenerator(), poppedStar, poppedStarScript.GarrisonCount, bot);

                Debug.Log(knownGraph.starList.Count);
                //shipController.SetNewWaypoints(pathfinderScript.calculate(knownGraph, knownGraph.findStarIndex(poppedStar), knownGraph.findStarIndex(star)));
                shipController.SetNewWaypoints(pathfinderScript.calculate(knownGraph, knownGraph.findStarIndex(star), knownGraph.findStarIndex(poppedStar)));
                shipController.StartJourney();

                garrisonHeap.Insert(poppedStar, poppedStarScript.GarrisonCount);
            }
            else
            {
                break;
            }
        }
        







        ////Old////


        //debug.logerror("amount of stars in emprie: " + ownedstars.count);
        //list<gameobject> templist = new list<gameobject>();
        //foreach (gameobject star in ownedstars)
        //{
        //    starscript starscript = star.getcomponent<starscript>();
        //    debug.logerror("checked star");
        //    starscript.refresh();
        //    if (starscript.carriershiptally + starscript.garrisoncount >= 100)
        //    {
        //        if (starscript.garrisoncount >= 100 && money >= gameinformation.carriercost)
        //        {
        //            debug.logerror("passed check");

        //            templist = mapgenerationscript.graphfullspeed.getstarneighbors(star).except(ownedstars).tolist();
        //            templist = mapgenerationscript.graphfullspeed.getstarneighbors(star);

        //            if (templist.count == 0)
        //            {
        //                templist = mapgenerationscript.graphfullspeed.getstarneighbors(star);
        //            }

        //            gameobject chosenstar = templist[random.next(0, templist.count - 1)];



        //            money -= gameinformation.carriercost;
        //            gameobject c = instantiate(gameinformation.shipprefab, star.transform.position, quaternion.identity);
        //            c.transform.parent = star.transform;
        //            shipcontroller shipcontroller = c.getcomponent<shipcontroller>();

        //            star.getcomponent<starscript>().attachcarrier(c);
        //            shipcontroller.dockedstar = star;

        //            shipcontroller.init(carriernamegenerator(), star, 100, bot);
        //            list<gameobject> temp = shipcontroller.getwaypoints();
        //            temp.add(chosenstar);
        //            shipcontroller.setnewwaypoints(temp);
        //            shipcontroller.startjourney();
        //        }
        //        else if (star.getcomponent<starscript>().carriercount > 0)
        //        {
        //            list<gameobject> cl = star.getcomponent<starscript>().carrierlist;
        //            gameobject movedcarrier = null;
        //            for (int i = 0; i < star.getcomponent<starscript>().carriercount; i++)
        //            {
        //                if (cl[i].getcomponent<shipcontroller>().getwaypoints().count == 0)
        //                {
        //                    movedcarrier = cl[i];
        //                    break;
        //                }
        //            }
        //            if (movedcarrier != null)
        //            {
        //                debug.logerror("moving preexisting carrier");
        //                templist = mapgenerationscript.graphfullspeed.getstarneighbors(star).except(ownedstars).tolist();
        //                templist = mapgenerationscript.graphfullspeed.getstarneighbors(star);

        //                if (templist.count == 0)
        //                {
        //                    templist = mapgenerationscript.graphfullspeed.getstarneighbors(star);
        //                }

        //                gameobject chosenstar = templist[random.next(0, templist.count - 1)];

        //                shipcontroller sc = movedcarrier.getcomponent<shipcontroller>();

        //                list<gameobject> temp = sc.getwaypoints();
        //                temp.add(chosenstar);
        //                sc.setnewwaypoints(temp);
        //                sc.startjourney();


        //            }
        //        }
        //        else
        //        {
        //            debug.logerror("bad stuff here");
        //        }
        //    }
        //}

        ///*        mapgenerationscript.graphfullspeed*/
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
            ShipController carrierScript = carrier.GetComponent<ShipController>();
            carrierList.Add(carrier);
            carrierSizeHeap.Insert(carrier, carrierScript.ShipCount);

            if(carrierScript.idle == true)
            {
                idleCarrierHeap.Insert(carrier, carrierScript.ShipCount);
            }
        }
        else
        {
            Debug.LogError("Bad");
        }
        
    }
    public void updateCarrier(GameObject carrier)
    {

        ShipController carrierScript = carrier.GetComponent<ShipController>();

        carrierSizeHeap.RemoveNode(carrier);
        carrierSizeHeap.Insert(carrier, carrierScript.ShipCount);

        idleCarrierHeap.RemoveNode(carrier);
        if (carrierScript.idle)
        {
            idleCarrierHeap.Insert(carrier, carrierScript.ShipCount);
        }
        else
        {
            idleCarrierHeap.RemoveNode(carrier);
        }
    }
    public void removeCarrier(GameObject carrier)
    {
        carrierList.Remove(carrier);
        carrierSizeHeap.RemoveNode(carrier);

        idleCarrierHeap.RemoveNode(carrier);
    }
    public void addStar(GameObject star)
    {
        candidateStars.Remove(star);
        if (!ownedStars.Contains(star))
        {
            StarScript s = star.GetComponent<StarScript>();
            ownedStars.Add(star);

            garrisonHeap.Insert(star, s.GarrisonCount);
            econCostHeap.Insert(star, s.GetEconPrice());
            industryCostHeap.Insert(star, s.GetIndustryPrice());
            scienceCostHeap.Insert(star, s.GetSciencePrice());

        }
        else
        {
            Debug.LogError("Updating star");
            updateStar(star);
        }

        candidateStars.AddRange(knownGraph.getStarNeighbors(star).Except(ownedStars));
        
    }
    public void updateStar(GameObject star)
    {
        StarScript s = star.GetComponent<StarScript>();

        garrisonHeap.RemoveNode(star);
        econCostHeap.RemoveNode(star);
        industryCostHeap.RemoveNode(star);
        scienceCostHeap.RemoveNode(star);

        garrisonHeap.Insert(star, s.GarrisonCount);
        econCostHeap.Insert(star, s.GetEconPrice());
        industryCostHeap.Insert(star, s.GetIndustryPrice());
        scienceCostHeap.Insert(star, s.GetSciencePrice());

        candidateStars.Remove(star);

    }
    public void removeStar(GameObject star)
    {
        ownedStars.Remove(star);
        candidateStars.Add(star);

        garrisonHeap.RemoveNode(star);
        econCostHeap.RemoveNode(star);
        industryCostHeap.RemoveNode(star);
        scienceCostHeap.RemoveNode(star);
    }

}
