using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerColourScript : MonoBehaviour
{
    [SerializeField] Material playerBorder;
    [SerializeField] Material enemyBorder;
    [SerializeField] Material unownedBorder;
    private Material playerMaterial;
    private Material enemyMaterial;
    private Material unownedMaterial;
    public Dictionary<int, Material[]> materialDictionary = new Dictionary<int, Material[]>();
    public Material[] playerArray;
    public Material[] unownedArray;
    public Material[] enemyArray;
    private bool hasRan = false;
    private void init()
    {
        hasRan = true;
        playerMaterial = new Material(playerBorder);
        playerMaterial.color = new Color(playerBorder.color.r / 2, playerBorder.color.g / 2, playerBorder.color.b / 2);
        playerMaterial.renderQueue = playerBorder.renderQueue + 50;
        enemyMaterial = new Material(enemyBorder); ;
        enemyMaterial.color = new Color(enemyBorder.color.r / 2, enemyBorder.color.g / 2, enemyBorder.color.b / 2);
        enemyMaterial.renderQueue = enemyBorder.renderQueue + 50;
        unownedMaterial = new Material(unownedBorder); ;
        unownedMaterial.color = new Color(unownedBorder.color.r / 2, unownedBorder.color.g / 2, unownedBorder.color.b / 2);
        unownedMaterial.renderQueue = unownedBorder.renderQueue + 50;
        // VVV DO NOT SWAP BORDER WITH MATERIAL, IT JUST BREAKS VVV
        playerArray = new Material[] { playerMaterial, playerBorder };
        //Material[] playerArray = { playerBorder, playerMaterial };
        unownedArray = new Material[] { unownedMaterial, unownedBorder };
        enemyArray = new Material[] { enemyMaterial, enemyBorder };
        //0 = unowned, 1 = player owned, 2 = enemy owned
        materialDictionary.Add(0, unownedArray);
        materialDictionary.Add(1, playerArray);
        materialDictionary.Add(2, enemyArray);
    }
    public Dictionary<int, Material[]> GetMaterialDictionary()
    {
        if (!hasRan)
        {
            init();
        }
        return materialDictionary;
    }
    public Color GetMainColour(int owner)
    {
        if (!hasRan)
        {
            init();
        }
        return materialDictionary[owner][1].color;
    }
    public Material[] GetPalette(int owner)
    {
        if (!hasRan)
        {
            init();
        }
        return materialDictionary[owner];
    }
}
