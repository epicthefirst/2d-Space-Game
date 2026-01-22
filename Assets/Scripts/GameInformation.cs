using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour
{
    public int playerCount;
    public List<PlayerClass> playerList = new List<PlayerClass>();

    public int tickCounter = 0;
    public int playerMoney = 500;
    public int cycleLength = 20; // Change this in the future
    public int carrierCost = 25; //This too

    public int baseIncomePerCycle = 250;

    public GameObject shipPrefab;



    public void AddPlayer(PlayerClass player)
    {
        if (playerList.Contains(player))
        {
            Debug.LogError("Already in list");
            return;
        }
        else
        {
            playerList.Add(player);
        }
    }
    public PlayerClass GetPlayerByID(int playerNumberID)
    {
        return playerList[playerNumberID];
    }
    public PlayerClass GetPlayerByName(string name)
    {
        foreach (PlayerClass player in playerList)
        {
            if (player.name.Equals(name)){
                return player;
            }
        }
        return null;
    }


    public class PlayerClass
    {
        public string name;
        public string description;

        //Playerscript or botscript
        public PlayerScript playerScript;
        public EnemyBotBehavior botScript;
        public bool isBot;

        public Color primaryColour;
        public Color secondaryColour;
        public Material primaryMaterial;
        public Material secondaryMaterial;





        public PlayerClass(string name, bool isBot, PlayerScript playerScript, EnemyBotBehavior enemyBotBehavior, Color primaryColour, Color secondaryColour, Material primaryMaterial, Material secondaryMaterial)
        {
            this.name = name;
            this.isBot = isBot;
            if (isBot)
            {
                this.botScript = enemyBotBehavior;
                Debug.Log("Clanker made");
            }
            else
            {
                this.playerScript = playerScript;
            }
                
            this.primaryColour = primaryColour;
            this.secondaryColour = secondaryColour;
            this.primaryMaterial = primaryMaterial;
            this.secondaryMaterial = secondaryMaterial;



        }

        public void AddCarrierToOwner(GameObject carrier)
        {
            if (!isBot)
            {
                playerScript.newCarrier(carrier);
            }
            else
            {
                botScript.addCarrier(carrier);
            }
        }

    }
}
