using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public sealed class OwnerColourScript : MonoBehaviour
{
    [SerializeField] Material playerBorder;
    [SerializeField] Material enemyBorder;
    [SerializeField] Material unownedBorder;
    private Material playerMaterial;
    private Material enemyMaterial;
    private Material unownedMaterial;

    public Dictionary<int, Material[]> materialDictionary = new Dictionary<int, Material[]>();

    public Material[] MainColourArray;
    public Material[] InsideColourArray;
    private bool[] isPaletteTaken;

    public Material[] playerArray;
    public Material[] unownedArray;
    public Material[] enemyArray;
    private bool hasRan = false;

    private static OwnerColourScript instance;

    


    public static OwnerColourScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OwnerColourScript>();
                if (instance == null)
                {
                    Debug.Log("Big ass error here");
                }
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        if (!hasRan)
        {
            Init();
        }
    }

    //private void Init()
    //{
    //    Debug.LogError("Has ran");
    //    hasRan = true;
    //    if (playerBorder == null)
    //    {
    //        Debug.LogError("Bad");
    //    }
    //    playerMaterial = new Material(playerBorder);
    //    playerMaterial.color = new Color(playerBorder.color.r / 2, playerBorder.color.g / 2, playerBorder.color.b / 2);
    //    playerMaterial.renderQueue = playerBorder.renderQueue + 50;
    //    enemyMaterial = new Material(enemyBorder); ;
    //    enemyMaterial.color = new Color(enemyBorder.color.r / 2, enemyBorder.color.g / 2, enemyBorder.color.b / 2);
    //    enemyMaterial.renderQueue = enemyBorder.renderQueue + 50;
    //    unownedMaterial = new Material(unownedBorder); ;
    //    unownedMaterial.color = new Color(unownedBorder.color.r / 2, unownedBorder.color.g / 2, unownedBorder.color.b / 2);
    //    unownedMaterial.renderQueue = unownedBorder.renderQueue + 50;
    //    // VVV DO NOT SWAP BORDER WITH MATERIAL, IT JUST BREAKS VVV
    //    playerArray = new Material[] { playerMaterial, playerBorder };
    //    //Material[] playerArray = { playerBorder, playerMaterial };
    //    unownedArray = new Material[] { unownedMaterial, unownedBorder };
    //    enemyArray = new Material[] { enemyMaterial, enemyBorder };
    //    //0 = unowned, 1 = player owned, 2 = enemy owned
    //    materialDictionary.Add(0, unownedArray);
    //    materialDictionary.Add(1, playerArray);
    //    materialDictionary.Add(2, enemyArray);
    //}
    private void Init()
    {
        Debug.LogError("Has ran");
        hasRan = true;
        if (playerBorder == null)
        {
            Debug.LogError("Bad");
        }

        CreateColourArrays();
    }

    public void CreateColourArrays()
    {
        //16 colours
        MainColourArray = new Material[16];
        InsideColourArray = new Material[16];
        Material baseMaterial = new Material(Shader.Find("Sprites/Default"));
        baseMaterial.renderQueue = baseMaterial.renderQueue - 100;

        Material temp = new Material(baseMaterial);
        Material tempBorder = new Material(baseMaterial);

        Color[] colours = new Color[] 
        { 
            new Color32(64, 64, 64, 255), //0: Unowned
            new Color32(0, 0, 255, 255), //1: Blue #0000ff
            new Color32(255, 0, 0, 255), //2: Red #ff0000
            new Color32(0, 255, 0, 255), //3: Green #00ff00
            new Color32(255, 0, 255, 255), //4: Purple #ff00ff
            new Color32(255, 255, 0, 255), //5: Yellow #ffff00

        };

        isPaletteTaken = new bool[colours.Length];
        isPaletteTaken[0] = true; //Unowned colour shouldn't be a player colour

        for(int i = 0; i < colours.Length; i++)
        {
            temp = new Material(baseMaterial);
            temp.color = colours[i];
            MainColourArray[i] = temp;
            tempBorder = new Material(temp);
            tempBorder.renderQueue = baseMaterial.renderQueue + 1;
            tempBorder.color = new Color(temp.color.r / 2, temp.color.g / 2, temp.color.b / 2);
            InsideColourArray[i] = tempBorder;
               
        }
    }

    public Material[] GetMainMaterialArray()
    {
        return MainColourArray;
    }
    public Material[] GetSecondaryMaterialArray()
    {
        return InsideColourArray;
    }
    public Material GetMainMaterial(int index)
    {
        //Debug.LogError("I have been called");
        return MainColourArray[index];
    }
    public Material GetSecondaryMaterial(int index)
    {
        //Debug.LogError("I have been called");
        return InsideColourArray[index];
    }

    public int GetNextFreePalette()
    {
        int i = Array.FindIndex(isPaletteTaken, e => !e);
        isPaletteTaken[i] = true;
        return i;
    }
    public void TakePalette(int index)
    {
        if (isPaletteTaken[index])
        {
            Debug.LogError("Tried to claim a palette that's already taken");
        }
        isPaletteTaken[index] = true;
    }
    //public Dictionary<int, Material[]> GetMaterialDictionary()
    //{
    //    return materialDictionary;
    //}
    //public Color GetMainColour(int owner)
    //{
    //    return materialDictionary[owner][1].color;
    //}
    //public Material[] GetPalette(int owner)
    //{
    //    return materialDictionary[owner];
    //}
}
