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


    private List<GameObject> ownedStars = new List<GameObject>();
    private Pathfinder.Graph knownGraph;
    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars)
    {
        this.bot = bot;
        this.ownedStars = ownedStars;

        uIManager.NewTick += newTick;
        
    }

    public void newTick(object sender, CycleEvent e)
    {
        
        checkToExpand();
    }

    public void checkToExpand()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (GameObject star in ownedStars)
        {
            if (star.GetComponent<StarScript>().GarrisonCount >= 100)
            {
                tempList = mapGenerationScript.graphFullSpeed.getStarNeighbors(star).Except(ownedStars).ToList();
                tempList.Sort();


            }
            tempList.AddRange(mapGenerationScript.graphFullSpeed.getStarNeighbors(star));
        }
        tempList = tempList.Distinct().ToList().Except(ownedStars).ToList();

/*        mapGenerationScript.graphFullSpeed*/
    }

}
