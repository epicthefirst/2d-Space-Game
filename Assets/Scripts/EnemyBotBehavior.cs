using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBotBehavior : MonoBehaviour
{
    //General Bot Logic
    public GameInformation.PlayerClass bot;
    public UIManager uIManager;
    public MapGeneration mapGenerationScript;

    private List<GameObject> ownedStars = new List<GameObject>();

    public void init(GameInformation.PlayerClass bot, List<GameObject> ownedStars)
    {
        this.bot = bot;
        this.ownedStars = ownedStars;

        uIManager.NewTick += newTick;
        
    }

    public void newTick(object sender, CycleEvent e)
    {

    }

    public void checkToExpand()
    {
/*        mapGenerationScript.graphFullSpeed*/
    }

}
