using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class StarScript : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick (PointerEventData eventData)
    {
        uIManager.InitUI(gameObject);
        string text = "Start, " + planetTimings.Count + ":";
        foreach (Tuple<int, int> tu in planetTimings)
        {
            text = text + tu.Item1 + ", " + tu.Item2 + ";";
        }
        Debug.Log(text);
    }





    //ORBITS + PLANETS QUALITY
    //Numbers to mess with
    public static float distanceIncrease = 1.5f;
    public int qualityMultiplier;




    public int selfId;
    public string Name;
    public List<int> planetList;
    public List<Tuple<int, int>> planetTimings;
    public GameObject canvas;
    private TextMeshPro ships;
    private GameObject go;
    public GameInformation.PlayerClass owner;
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
    private int tick;
    private int cycle;
    [SerializeField] GameInformation gameInformation;

    public LineRenderer orbitMaker;
    public GameObject insidePolygon;
    public GameObject borderPolygon;
    private GameObject starSpawn;
    private Dictionary<int, Material[]> materialDictionary;
    public Material[] materials;
    public List<GameObject> CarrierList = new List<GameObject>();
    public List<GameObject> maneuverCarrierList = new List<GameObject>();

    private TextMeshPro shipText;
    private TextMeshPro scienceText;
    private TextMeshPro industryText;
    private TextMeshPro econText;

    private UIManager uIManager;

    private List<GameObject> planetObjectList = new List<GameObject>();
    private List<GameObject> orbitObjectList;
    public GameObject[] planetArray;

    public GameObject planet;
    Dictionary<int, int> slingshotWindowDurations;




    public Vector3[] array = new Vector3[10];


    public void Initialize(int Id, string Name, List<int> planetList, List<Tuple<int,int>> PlanetTimings, int Range, GameInformation.PlayerClass owner, GameObject canvas, int GarrisonCount, GameObject[] planetArray, int qualityMultiplier, Dictionary<int, int> slingshotWindowDurations)
    {

        this.owner = owner;
        this.selfId = Id;
        this.planetTimings = PlanetTimings;
        this.Name = Name;
        this.planetList = planetList;
        this.Range = Range;
        this.canvas = canvas;
        this.GarrisonCount = GarrisonCount;
        this.planetArray = planetArray;
        this.qualityMultiplier = qualityMultiplier;
        this.slingshotWindowDurations = slingshotWindowDurations;


        if(owner ==  null)
        {
            materials = new Material[] { ownerColo, owner.secondaryMaterial };
        }
        materials = new Material[] {owner.primaryMaterial, owner.secondaryMaterial};

        
    }


    private void WakeUp()
    {
        if (!isAwake){
            CycleEventManager.OnTick += thisNewTick;
        }
        isAwake = true;
    }



    private void Start()
    {

        uIManager = canvas.GetComponent<UIManager>();

        CycleEventManager.OnTick += thisNewTick;

        /*Debug.Log(gameObject.transform.position);*/

        if (planetList.Count == planetTimings.Count)
        {
            drawOrbit(qualityMultiplier);
            drawPlanets(qualityMultiplier);
        }
        else
        {
            Debug.LogError("YO THERE'S AN ERROR HERE");
        }
        /*gameObject.GetComponent<Orbits>().init(planetList, planetTimings, tick);*/
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
    private void thisNewTick(object sender, NewTickEvent e)
    {
        if (IndustryCount != 0)
        {
            GarrisonCount += IndustryCount;
            Refresh();
        }
        tick = e.CurrentTick;
        updatePlanets();
    }



    public void ShipInbound(int shipShipCount, GameInformation.PlayerClass shipOwner, GameObject carrier)
    {
        WakeUp();
        //0 = unowned, 1 = player owned, 2 = enemy owned
        if (shipOwner == owner)
        {

            AttachCarrier(carrier);
            Refresh();
        }
        else if (owner == null)
        {
            owner = shipOwner;
            //canvas.GetComponent<UIManager>().playerStars.Add(gameObject);
            //Come back to this
            owner.playerScript.AddStar(gameObject);
            AttachCarrier(carrier);
            Refresh();
        }
        else
        {
            if (shipShipCount > GarrisonCount + CarrierShipTally)
            {
                //WORK ON THIS LATER
                owner = shipOwner;
                carrier.GetComponent<ShipController>().ShipCount -= GarrisonCount;
                GarrisonCount = 0;
                EconCount = 0;
                IndustryCount = 0;
                ScienceCount = 0;
                AttachCarrier(carrier);
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
    public void AttachCarrier(GameObject carrier)
    {
        CarrierList.Add(carrier);
        CarrierCount += 1;
        PolygonRefresh();
        Refresh();
    }
    public void DetachCarrier(GameObject carrier)
    {
        if (CarrierList.Contains(carrier))
        {
            CarrierList.Remove(carrier);
            CarrierCount -= 1;

            //Debug.LogError("Dettached carrier");
        }

        PolygonRefresh();
        Refresh();
    }
    public void ReduceShipCount(int shipCountReduction)
    {
        GarrisonCount -= shipCountReduction;
        PolygonRefresh();
        Refresh();
    }

    public bool isGoingToSlingshot(int timeLeft)
    {
        for (int i = 0; i < planetList.Count; i++)
        {
/*            float orbitProgress = (1 / (float)planetTimings[i].Item2) * ((tick + planetTimings[i].Item1) % planetTimings[i].Item2);

            int orbitProgressInt = ((tick + planetTimings[i].Item1) % planetTimings[i].Item2);*/

            if ((timeLeft + tick + planetTimings[i].Item1 + slingshotWindowDurations[i] - 1) % planetTimings[i].Item2 <= (slingshotWindowDurations[i]-1))
            {
                Debug.Log("Slingshot-able, " + i + " : " + (timeLeft + tick + 1 + planetTimings[i].Item1) % planetTimings[i].Item2);
                Debug.Log(planetTimings[i].Item1 + " : " + planetTimings[i].Item2);
                return true;
            }
        }
        Debug.Log("Not slingshot-able");
        return false;
    }
    public void startSlingshot(GameObject carrier)
    {
        maneuverCarrierList.Add(carrier);
        PolygonRefresh();
        Refresh();
    }

    public void Refresh()
    {
        CarrierShipTally = 0;
        foreach (GameObject carrier in CarrierList) 
        {
            CarrierShipTally += carrier.GetComponent<ShipController>().ShipCount;
        }
        shipCountDisplay = GarrisonCount + CarrierShipTally;
        if (shipText != null)
        {
            shipText.text = shipCountDisplay.ToString() + (CarrierCount == 0 ? null : "/" + CarrierCount)+ (maneuverCarrierList.Count == 0 ? null : "!" + maneuverCarrierList.Count);
        }
        if (owner != null)
        {
            econText.text = EconCount.ToString();
            industryText.text = IndustryCount.ToString();
            scienceText.text = ScienceCount.ToString();
        }

        materials = new Material[] { owner.primaryMaterial, owner.secondaryMaterial };
    }
    public void ReCountPlanets()
    {
        PlanetaryCount = 0;
        GasCount = 0;
        HabitableCount = 0;
        for (int i = 0; i < planetList.Count; i++)
        {
            switch (planetList[i])
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
        if (owner == null)
        {
            insidePolygon.GetComponent<MeshRenderer>().material = materialDictionary[0][0];
            borderPolygon.GetComponent<MeshRenderer>().material = materialDictionary[0][1];
        }
        else
        {
            insidePolygon.GetComponent<MeshRenderer>().material = owner.primaryMaterial;
            borderPolygon.GetComponent<MeshRenderer>().material = owner.secondaryMaterial;
        }
    }
    public void SendPolygon(GameObject insidePolygon, GameObject borderPolygon, Dictionary<int, Material[]> materialDictionary)
    {
        this.insidePolygon = insidePolygon;
        this.borderPolygon = borderPolygon;
        this.materialDictionary = materialDictionary;
        PolygonRefresh();
    }






    //
    //
    //ORBITS AND STUFF
    //
    //


    void drawOrbit(int steps)
    {
        steps = steps * 15;
        for (int j = 0; j < planetList.Count; j++)
        {
            float radius = distanceIncrease * (j + 1);
            GameObject orbitObject = new GameObject("Orbit");
            orbitObject.transform.SetParent(gameObject.transform);
            LineRenderer orbitMaker = orbitObject.AddComponent<LineRenderer>();
            orbitMaker.material = new Material(Shader.Find("Sprites/Default"));

            //bool isPlanetary = orbitType == 1;

            //orbitMaker.startColor = isPlanetary ? Color.red : Color.gray;
            //orbitMaker.endColor = isPlanetary ? Color.red : Color.gray;
            //orbitMaker.startWidth = isPlanetary ? 0.5f : 0.3f;
            //orbitMaker.endWidth = isPlanetary ? 0.5f : 0.3f;


            switch (planetList[j])
            {
                case 0:
                    //Planetary
                    orbitMaker.startColor = Color.gray;
                    orbitMaker.endColor = Color.gray;
                    orbitMaker.startWidth = 0.3f;
                    orbitMaker.endWidth = 0.3f;
                    break;
                case 1:
                    //Gas
                    orbitMaker.startColor = Color.red;
                    orbitMaker.endColor = Color.red;
                    orbitMaker.startWidth = 0.5f;
                    orbitMaker.endWidth = 0.5f;
                    break;
                case 2:
                    //Habitable
                    orbitMaker.startColor = Color.green;
                    orbitMaker.endColor = Color.green;
                    orbitMaker.startWidth = 0.3f;
                    orbitMaker.endWidth = 0.3f;
                    break;
            }


            
            orbitMaker.positionCount = (steps * 6 / 10) + 1;

            float cutStartAngle = 5 * Mathf.PI / 10;
            for (int i = 0; i < (steps * 6 / 10) + 1; i++)
            {
                float circumferenceProgress = (float)i / steps;

                float currentRadian = (circumferenceProgress * 2 * Mathf.PI) + cutStartAngle;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector2 position = new Vector2(x, y) + gameObject.transform.position.ConvertTo<Vector2>();

                orbitMaker.SetPosition(i, position);
            }


            //// Useless now
            //orbitMaker.SetPosition(steps, orbitMaker.GetPosition(0));
            orbitMaker.material = new Material(Shader.Find("Sprites/Default"));
        }
    }
    private void drawPlanets(int steps)
    {
        foreach (GameObject pl in planetObjectList)
        {
            Destroy(pl);
        }
        for (int i = 0; i < planetList.Count; i++)
        {
            
            float orbitalRadius = distanceIncrease * (i + 1);
            /*            GameObject planetObject = new GameObject("Planet");
                        planetObject.transform.SetParent(gameObject.transform);
                        LineRenderer planetMaker = planetObject.AddComponent<LineRenderer>();
                        planetMaker.material = new Material(Shader.Find("Sprites/Default"));
                        planetObjectList.Add(planetObject);*/

            //List<int> planetList, List< int > planetTimings

            //orbitProgress ranges from 0-1, dont forget to offset and reverse direction!!!!!!!
            float orbitProgress = (1 / (float)planetTimings[i].Item2) * ((tick + planetTimings[i].Item1) % planetTimings[i].Item2);
            /*        float orbitProgress = 0.25f;*/
            float orbitRadians = (0.25f - orbitProgress) * 2 * Mathf.PI;
            float xTimingAdjust = Mathf.Cos(orbitRadians) * orbitalRadius;
            float yTimingAdjust = Mathf.Sin(orbitRadians) * orbitalRadius;



            switch (planetList[i])
            {
                case 0:
                    //Terrestrial
                    Debug.Log(planetArray[0]);
                    planet = Instantiate(planetArray[0], gameObject.transform, false);
                    break;
                case 1:
                    //Gas
                    planet = Instantiate(planetArray[1], gameObject.transform, false);
                    break;
                case 2:
                    //Habitable
                    planet = Instantiate(planetArray[2], gameObject.transform, false);
                    break;
            }

            planet.transform.localPosition = new Vector3(xTimingAdjust, yTimingAdjust);
            planet.SetActive(true);
            planetObjectList.Add(planet);
            //0.25f is to offset it
            planetTransparency(0.25f - orbitProgress, planet.GetComponent<MeshRenderer>());

            /*            planetMaker.positionCount = steps+4;

                        for (int j = 0; j < steps+4; j++)
                        {
                            float circumferenceProgress = (float)j / steps;

                            float currentRadian = (circumferenceProgress * 2 * Mathf.PI);

                            float xScaled = Mathf.Cos(currentRadian);
                            float yScaled = Mathf.Sin(currentRadian);

                            float x = xScaled * radius;
                            float y = yScaled * radius;

                            Vector2 position = new Vector2(x, y) + gameObject.transform.position.ConvertTo<Vector2>() + new Vector2(xTimingAdjust, yTimingAdjust);

                            planetMaker.SetPosition(j, position);

                        }*/
            /*            planetMaker.SetPosition(steps + 1, planetMaker.GetPosition(1));*/
            /*            planetMaker.SetPosition(steps-1, planetMaker.GetPosition(0));
                        planetMaker.SetPosition(steps, planetMaker.GetPosition(1));
                        planetMaker.SetPosition(steps+1, planetMaker.GetPosition(2));
                        planetMaker.SetPosition(steps+2, planetMaker.GetPosition(3));*/
        }
    }

    public void planetTransparency(float orbitProgress, MeshRenderer MR)
    {
        var color = MR.material.color;
        if (orbitProgress < 0.25f & orbitProgress > -0.15f)
        {
            color.a = 0.4f;
            MR.material.color = color;
        }
        else
        {
            color.a = 1f;
            MR.material.color = color;
        }
    }

    public void updatePlanets()
    {
        for (int i = 0; i < planetObjectList.Count; i++)
        {
            float orbitalRadius = distanceIncrease * (i + 1);
            float orbitProgress = (1 / (float)planetTimings[i].Item2) * ((tick + planetTimings[i].Item1) % planetTimings[i].Item2);

            planetTransparency(0.25f - orbitProgress, planetObjectList[i].GetComponent<MeshRenderer>());

            /*        float orbitProgress = 0.25f;*/
            float orbitRadians = (0.25f - orbitProgress) * 2 * Mathf.PI;
            float xTimingAdjust = Mathf.Cos(orbitRadians) * orbitalRadius;
            float yTimingAdjust = Mathf.Sin(orbitRadians) * orbitalRadius;

            planetObjectList[i].transform.localPosition = new Vector3(xTimingAdjust, yTimingAdjust);
        }
    }



}
