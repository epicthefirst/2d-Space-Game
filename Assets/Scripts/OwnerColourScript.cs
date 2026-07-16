using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private void Awake()
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
        Material baseMaterial = new Material(Shader.Find("Standard")); //Work on me here
        baseMaterial.renderQueue = baseMaterial.renderQueue + 50;

        Material temp = new Material(baseMaterial);
        Material tempBorder = new Material(baseMaterial);

        //0
        //Unowned
        temp = new Material(baseMaterial);
        temp.color = new Color(64, 64, 64);
        MainColourArray[1] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[1] = tempBorder;

        //1
        //Blue: #0000ff
        temp = new Material(baseMaterial);
        temp.color = new Color(0, 0, 255);
        MainColourArray[1] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[1] = tempBorder;

        //2
        //Red: #ff0000
        temp = new Material(baseMaterial);
        temp.color = new Color(255, 0, 0);
        MainColourArray[2] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[2] = tempBorder;

        //3
        //Green: #00ff00
        temp = new Material(baseMaterial);
        temp.color = new Color(0, 255, 0);
        MainColourArray[3] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[3] = tempBorder;

        //4
        //Purple: #ff00ff
        temp = new Material(baseMaterial);
        temp.color = new Color(255, 0, 255);
        MainColourArray[4] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[4] = tempBorder;

        //5
        //Yellow: #ffff00
        temp = new Material(baseMaterial);
        temp.color = new Color(255, 255, 0);
        MainColourArray[5] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[5] = tempBorder;

        //6
        //Cyan: #00ffff
        temp = new Material(baseMaterial);
        temp.color = new Color(255, 255, 0);
        MainColourArray[6] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[6] = tempBorder;

        //7
        //Orange: #ff8000
        temp = new Material(baseMaterial);
        temp.color = new Color(255, 255, 0);
        MainColourArray[7] = temp;
        tempBorder = new Material(temp);
        tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        InsideColourArray[7] = tempBorder;

        ////7
        ////Black: #000000
        //temp = new Material(baseMaterial);
        //temp.color = new Color(0, 0, 0);
        //MainColourArray[7] = temp;
        //tempBorder = new Material(temp);
        //tempBorder.color = new Color(tempBorder.color.r / 2, tempBorder.color.g / 2, tempBorder.color.b / 2);
        //BorderColourArray[7] = tempBorder;



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
        return MainColourArray[index];
    }
    public Material GetSecondaryMaterial(int index)
    {
        return InsideColourArray[index];
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
