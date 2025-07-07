using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StarScript : MonoBehaviour
{
    public int selfId;
    public string Name;
    public List<int> OrbitList;
    public GameObject canvas;
    private TextMeshPro ships;
    private GameObject go;
    public int Owner = 0;
    private int shipCountDisplay = 0;
    public int GarrisonCount = 0;
    public int CarrierShipTally;
    public int EconCount = 0;
    public int EconPrice;
    public int IndustryCount = 0;
    public int IndustryPrice;
    public int ScienceCount = 0;
    public int SciencePrice;
    public int GasCount = 0;
    public int PlanetaryCount = 0;
    public int HabitableCount = 0;
    public int Range = 2;
    public int CarrierCount;
    public bool isAwake = false;
    public StarData star;

    public LineRenderer orbitMaker;
    public GameObject insidePolygon;
    public GameObject borderPolygon;
    private GameObject starSpawn;
    private Dictionary<int, Material[]> materialDictionary;
    public Material[] materials;
    public List<GameObject> CarrierList = new List<GameObject>();

    private TextMeshPro shipText;
    private TextMeshPro scienceText;
    private TextMeshPro industryText;
    private TextMeshPro econText;

    private UIManager uIManager;

    public void Initialize(int Id, string Name, List<int> OrbitList, int Range, int Owner, GameObject canvas, int GarrisonCount)
    {

        this.Owner = Owner;
        this.selfId = Id;
        this.Name = Name;
        this.OrbitList = OrbitList;
        this.Range = Range;
        this.canvas = canvas;
        Debug.Log(Owner);
        this.GarrisonCount = GarrisonCount;
        
    }

    //Add to this if need be
    public void UpdateStarData(StarData starData)
    {
        Debug.Log("Received" + starData.EconCount);
        Owner = starData.Owner;
        EconCount = starData.EconCount;
        IndustryCount = starData.IndustryCount;
        ScienceCount = starData.ScienceCount;
        //if (GarrisonCount != starData.ShipCount) { Debug.Log("The GarrisonCount != to new shipCount, this shouldn't happen"); }
        Refresh();
    }
    private void WakeUp()
    {
        if (!isAwake){
            uIManager.NewTick += thisNewTick;
        }
        isAwake = true;
    }



    private void Start()
    {

        uIManager = canvas.GetComponent<UIManager>();
        if (isAwake)
        {
            uIManager.NewTick += thisNewTick;
        }
        gameObject.GetComponent<Orbits>().OrbitStart(OrbitList);
        //Create the 3 infrastructure indicators

        GameObject econInfrastructure = new GameObject("econInfrastructure");
        econInfrastructure.transform.SetParent(gameObject.transform);
        econInfrastructure.transform.position = gameObject.transform.position;
        econText = econInfrastructure.AddComponent<TextMeshPro>();
        econText.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        econText.transform.position += new Vector3(2, 3);
        econText.verticalAlignment = VerticalAlignmentOptions.Middle;
        econText.enableWordWrapping = false;
        econText.fontSize = 12;
        econText.color = Color.green;

        GameObject industryInfrastructure = new GameObject("industryInfrastructure");
        industryInfrastructure.transform.SetParent(gameObject.transform);
        industryInfrastructure.transform.position = gameObject.transform.position;
        industryText = industryInfrastructure.AddComponent<TextMeshPro>();
        industryText.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        industryText.transform.position += new Vector3(5, 3);
        industryText.verticalAlignment = VerticalAlignmentOptions.Middle;
        industryText.enableWordWrapping = false;
        industryText.fontSize = 12;
        industryText.color = Color.red;

        GameObject scienceInfrastructure = new GameObject("scienceInfrastructure");
        scienceInfrastructure.transform.SetParent(gameObject.transform);
        scienceInfrastructure.transform.position = gameObject.transform.position;
        scienceText = scienceInfrastructure.AddComponent<TextMeshPro>();
        scienceText.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        scienceText.transform.position += new Vector3(8, 3);
        scienceText.verticalAlignment = VerticalAlignmentOptions.Middle;
        scienceText.enableWordWrapping = false;
        scienceText.fontSize = 12;
        scienceText.color = Color.blue;


        // Create the Text GameObject.
        GameObject go = new GameObject("shipCountDisplay");
        go.transform.parent = gameObject.transform;
        go.transform.position = gameObject.transform.position;
        shipText = go.AddComponent<TextMeshPro>();
        shipText.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 3);
        shipText.transform.position += new Vector3(26, 1, 0);
        shipText.verticalAlignment = VerticalAlignmentOptions.Middle;
        shipText.enableWordWrapping = false;
        shipText.text = shipCountDisplay.ToString();
        shipText.fontSize = 24;
        shipText.color = Color.white;
        

        // Make the name gameobject
        GameObject go2 = new GameObject("starNameDisplay");
        go2.transform.parent = gameObject.transform;
        go2.transform.position = gameObject.transform.position;
        TextMeshPro starNameDisplay = go2.AddComponent<TextMeshPro>();
        starNameDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(6, 0.5f);
        starNameDisplay.transform.position += new Vector3(5, -1, 0);
        starNameDisplay.text = Name;
        starNameDisplay.enableWordWrapping = false;
        starNameDisplay.fontSize = 12;
        Refresh();
    }
    private void thisNewTick(object sender, EventArgs e)
    {
        if (IndustryCount != 0)
        {
            GarrisonCount += IndustryCount;
            Refresh();
        }
        
        Debug.Log("NewTick responded");
    }



    public void ShipInbound(int shipShipCount, int shipOwner, GameObject carrier)
    {
        WakeUp();
        //0 = unowned, 1 = player owned, 2 = enemy owned
        Debug.Log("I have ran");
        if (shipOwner == Owner)
        {

            AttachCarrier(1, carrier);
            Refresh();
        }
        else if (Owner == 0)
        {
            Owner = shipOwner;
            //canvas.GetComponent<UIManager>().playerStars.Add(gameObject);
            //Come back to this
            AttachCarrier(1, carrier);
            Refresh();
        }
        else
        {
            if (shipShipCount > GarrisonCount)
            {
                Owner = shipOwner;
                carrier.GetComponent<ShipController>().ShipCount -= GarrisonCount;
                GarrisonCount = 0;
                EconCount = 0;
                IndustryCount = 0;
                ScienceCount = 0;
                AttachCarrier(1, carrier);
                //canvas.GetComponent<UIManager>().playerStars.Add(gameObject);
                // Do some stuff here
                Refresh();
            }
            else
            {
                GarrisonCount -= shipShipCount;
                Destroy(carrier);
                Refresh();
            }
        }
      


    }
    public void AttachCarrier(int increase, GameObject carrier)
    {
        CarrierList.Add(carrier);
        CarrierCount += increase;
        PolygonRefresh();
        Refresh();
    }
    public void DetachCarrier(int decrease, GameObject carrier)
    {
        CarrierList.Remove(carrier);
        CarrierCount -= decrease;
        PolygonRefresh();
        Refresh();
    }
    public void ReduceShipCount(int shipCountReduction)
    {
        GarrisonCount -= shipCountReduction;
        PolygonRefresh();
        Refresh();
    }

    public void Refresh()
    {
        Debug.Log("Refresh");
        CarrierShipTally = 0;
        foreach (GameObject carrier in CarrierList) 
        {
            CarrierShipTally += carrier.GetComponent<ShipController>().ShipCount;
        }
        shipCountDisplay = GarrisonCount + CarrierShipTally;
        if (shipText != null)
        {
            shipText.text = shipCountDisplay.ToString() + (CarrierCount == 0 ? null : "/" + CarrierCount);
        }
        if (Owner != 0)
        {
            econText.text = EconCount.ToString();
            industryText.text = IndustryCount.ToString();
            scienceText.text = ScienceCount.ToString();
        }
    }
    public void ReCountPlanets()
    {
        PlanetaryCount = 0;
        GasCount = 0;
        HabitableCount = 0;
        for (int i = 0; i < OrbitList.Count; i++)
        {
            switch (OrbitList[i])
            {
                case 0:
                    //Planetary
                    PlanetaryCount++;
                    break;
                case 1:
                    //Gas
                    GasCount++;
                    break;
                case 2:
                    //Habitable
                    HabitableCount++;
                    break;

            }
        }
    }
    void PolygonRefresh()
    {
        materials = materialDictionary[Owner];
        insidePolygon.GetComponent<MeshRenderer>().material = materials[0];
        borderPolygon.GetComponent<MeshRenderer>().material = materials[1];
    }
    public void SendPolygon(GameObject insidePolygon, GameObject borderPolygon, Dictionary<int, Material[]> materialDictionary)
    {
        this.insidePolygon = insidePolygon;
        this.borderPolygon = borderPolygon;
        this.materialDictionary = materialDictionary;
        Debug.Log(materialDictionary.Count);
        PolygonRefresh();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            
            
            uIManager.InitUI(gameObject);
        }
    }
    

}
