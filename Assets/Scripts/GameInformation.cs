using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour
{
    public int playerCount;
    public List<PlayerClass> playerList = new List<PlayerClass>();


    public void addPlayer(PlayerClass player)
    {
        if (playerList.Contains(player))
        {
            return;
        }
        else
        {
            playerList.Add(player);
        }
    }


    public class PlayerClass
    {
        public string name;
        public string description;

        public Color primaryColour;
        public Color secondaryColour;
        public Material primaryMaterial;
        public Material secondaryMaterial;




        public PlayerClass(string name, Color primaryColour, Color secondaryColour, Material primaryMaterial, Material secondaryMaterial)
        {
            this.name = name;
            this.primaryColour = primaryColour;
            this.secondaryColour = secondaryColour;
            this.primaryMaterial = primaryMaterial;
            this.secondaryMaterial = secondaryMaterial;

        }

    }
}
