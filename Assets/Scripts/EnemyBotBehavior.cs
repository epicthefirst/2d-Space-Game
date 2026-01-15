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


    private List<GameObject> ownedStars = new List<GameObject>();
    private Pathfinder.Graph knownGraph;
    private System.Random random;
    private int money;
    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars, System.Random random, GameInformation gameInformation)
    {
        this.gameInformation = gameInformation;
        this.random = random;
        this.bot = bot;
        this.ownedStars = ownedStars;
        money = bot.playerScript.playerMoney;
        uIManager.NewTick += newTick;
        
    }

    public void newTick(object sender, CycleEvent e)
    {
        if (money > gameInformation.carrierCost)
        {
            checkToExpand();
        }
    }

    public void checkToExpand()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (GameObject star in ownedStars)
        {
            if (star.GetComponent<StarScript>().GarrisonCount >= 100)
            {
                tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star).Except(ownedStars).ToList();

                GameObject chosenStar = tempList[random.Next(0, tempList.Count - 1)];

                if(money <= gameInformation.carrierCost)
                {
                    money -= gameInformation.carrierCost;
                    GameObject c = Instantiate(gameInformation.shipPrefab, star.transform.position, Quaternion.identity);
                    c.transform.parent = star.transform;
                    ShipController shipController = c.GetComponent<ShipController>();

                    star.GetComponent<StarScript>().AttachCarrier(c);
                    shipController.dockedStar = star;

                    shipController.Init(carrierNameGenerator(), nextTickButton, currentStar, inputedShipCount, carrierCount, playerScript, lineDrawer);
                }

            }
        }
        tempList = tempList.Distinct().ToList().Except(ownedStars).ToList();

/*        mapGenerationScript.graphFullSpeed*/
    }

    public string carrierNameGenerator()
    {
        carrierNameIncrement++;
        return bot.name + " " + carrierNameIncrement.ToString();
    }

}
