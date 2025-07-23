using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CycleEvent : EventArgs
{
    public int CurrentCycle { get; set; }
    public int CurrentTick { get; set; }
    public int TickPerCycle { get; set; }
}

public class UIManager : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{


/*    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + name + " GameObject");
    }*/


    public MapGeneration mapGeneration;
    public OwnerColourScript ownerColourScript;

    [SerializeField] Camera mainCamera;

    [SerializeField] Image panel;
    [SerializeField] Image panel2;

    [SerializeField] TMP_Text seedText;
    [SerializeField] TMP_Text starNameText;
    [SerializeField] TMP_Text postitionText;
    [SerializeField] TMP_Text ownerText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text tickText;

    [SerializeField] TMP_Text planetaryText;
    [SerializeField] TMP_Text gasText;
    [SerializeField] TMP_Text habitableText;

    [SerializeField] TMP_Text econPriceText;
    [SerializeField] TMP_Text industryPriceText;
    [SerializeField] TMP_Text sciencePriceText;

    [SerializeField] Button nextTickButton;
    [SerializeField] Button createCarrierButton;
    [SerializeField] TMP_Text messagePrompt;

    [SerializeField] TMP_InputField shipInput;
    [SerializeField] Button shipInputButton;
/*    [SerializeField] Button slingshotToggleButton;*/

    //Buy buttons
    [SerializeField] Button buyEconButton;
    [SerializeField] Button buyIndustryButton;
    [SerializeField] Button buyScienceButton;
    [SerializeField] Button carrierMenuBlueButton;

    [SerializeField] PlayerScript playerScript;

    [SerializeField] TMP_Text carrierText;
    [SerializeField] ScrollRect carrierList;

    [SerializeField] TMP_Text econIndicator;
    [SerializeField] TMP_Text industryIndicator;
    [SerializeField] TMP_Text scienceIndicator;

    [SerializeField] GameObject carrierButtonParent;
    [SerializeField] UnityEngine.Object specImageFolder;
    [SerializeField] RoutePlanner routePlannerScript;

    public GameObject StarInfo;
    public GameObject CarrierInfo;


    private Vector2 pos;
    private List<int> orbitList = new List<int>();



    //Permanent, spec stuff
    private Dictionary<string, Texture2D> specImageDictionary;
    private Dictionary<string, int> specCostDictionary = new Dictionary<string, int>();



    private int owner;
    private int econPrice;
    private int industryPrice;
    private int sciencePrice;
    private int playerRange;

    public int tickCounter = 0;
    public int playerMoney = 500;
    public int cycleLength = 20; // Change this in the future
    public int carrierCost = 25; //This too

    public int baseIncomePerCycle = 250;
    
    public int cycleCount = 0;
    private int carrierCount = 0;



    private int gasCount;
    private int terrestrialCount;
    private int habitableCount;

    private bool starSelected = false;
    private GameObject currentCarrier;

    private GameObject currentStar;
    private GameObject tempStar;
    private GameObject lastClickedStar;
    private StarScript CStarScript;
    private StarScript TStarScript;
    [SerializeField] GameObject shipPrefab;

    private GameObject circleObject;
    private bool shipInputConfirm;
    private int inputedShipCount;
    [SerializeField] TMP_Text inputedShipCountTextField;

    public GameObject carrierButtonPrefab;
    public bool isRoutePlannerActive = false;


    //Events
    public event EventHandler<CycleEvent> NewTick;
    CycleEvent cycleEvent = new();


    //Change this to add new specialists
    private void setDictionaries()
    {
        specImageDictionary = new Dictionary<string, Texture2D>()
        {
            {"TestingCarrier", Resources.Load<Texture2D>("Spec_PFPs/Carrier_Spec/TestingCarrier")}
        };
    }


    void Start()
    {
        cycleEvent.TickPerCycle = cycleLength;
        setDictionaries();
        shipInputButton.onClick.AddListener(WhenInputConfirmed);
        nextTickButton.onClick.AddListener(OnTickButtonPress);
        createCarrierButton.onClick.AddListener(OnCreateCarrierPress);

        //Buy buttons
        buyEconButton.onClick.AddListener(BuyEconomy);
        buyIndustryButton.onClick.AddListener(BuyIndustry);
        buyScienceButton.onClick.AddListener(BuyScience);

        seedText.text = "Seed: "+mapGeneration.seed;
        moneyText.text = "Credits: " + playerMoney;
        tickText.text = "Tick: "+ tickCounter;
        ClearUI();
    }

    public Texture2D getSpecImage(String specialistName)
    {
        Texture2D specImage;
        Debug.Log("CHECK FILENAME TO MATCH:" + specialistName);
        specImageDictionary.TryGetValue(specialistName, out specImage);
        return specImage;
    }
    void RefreshUI()
    {
        
        if (currentStar != null)
        {
            CStarScript.ReCountPlanets();
            CStarScript.Refresh();
            switch (owner)
            {
                case 0:
                    //unowned
                    break;
                case 1:
                    //player

                    carrierList.gameObject.SetActive(true);
                    carrierText.text = "Carriers: " + CStarScript.CarrierCount;
                    break;
                case 2:
                    //enemy
                    carrierList.gameObject.SetActive(true);
                    carrierText.text = "Carriers: " + CStarScript.CarrierCount;
                    //add a lil something here later
                    break;
            }
            panel.color = CStarScript.materials[1].color;
            panel.gameObject.SetActive(true);
            panel2.gameObject.SetActive(true);

            econPrice = (50 * (CStarScript.EconCount + 1)) / (CStarScript.PlanetaryCount + 1 + CStarScript.HabitableCount * 2);
            industryPrice = (100 * (CStarScript.IndustryCount + 1)) / (CStarScript.GasCount + 1 + CStarScript.HabitableCount * 2);
            sciencePrice = (200 * (CStarScript.ScienceCount + 1)) / (1 + CStarScript.HabitableCount * 5);

            starNameText.text = "Star: " + CStarScript.Name;

            planetaryText.text = "Terrestrial: " + CStarScript.PlanetaryCount;
            habitableText.text = "Habitable: " + CStarScript.HabitableCount;
            gasText.text = "Gas: " + CStarScript.GasCount;

            econIndicator.text = CStarScript.EconCount.ToString();
            industryIndicator.text = CStarScript.IndustryCount.ToString();
            scienceIndicator.text = CStarScript.ScienceCount.ToString();


            postitionText.text = "Position: " + currentStar.transform.position;
        }

        seedText.text = "Seed: " + mapGeneration.seed;
        moneyText.text = "Credits: " + playerMoney;
        tickText.text = "Tick: " + tickCounter;
        
        

        econPriceText.text = "Economy: " + econPrice + "$";
        industryPriceText.text = "Industry: " + industryPrice + "$";
        sciencePriceText.text = "Science: " + sciencePrice + "$";
        carrierButtons();
        currentCarrier = null;
        
        return;
    }
    


    public void InitUI(GameObject star)
    {
        Debug.Log(star);

        if (isRoutePlannerActive)
        {
            Destroy(circleObject);
            circleObject = GenerateCircle(star.transform.position, star.GetComponent<StarScript>().Range);


            if (false)
            {
                //Some pathfinding script
            }
            else
            {
                Debug.LogWarning((currentCarrier != null));
                List<GameObject> starWaypoints = routePlannerScript.currentCarrier.GetComponent<ShipController>().starWaypoints;
                starWaypoints.Add(star);
                routePlannerScript.updateUI(starWaypoints);
            }
            return;
        }
        
        lastClickedStar = star;
        Debug.Log(lastClickedStar);
        ClearUI();
        switchPanels(0);
        starSelected = true;
        currentStar = star;
        CStarScript = star.GetComponent<StarScript>();
        this.orbitList = CStarScript.planetList;
        this.owner = CStarScript.Owner;
        Debug.Log(CStarScript.EconCount);
        switch (owner)
        {
            case 0:
                ownerText.text = "Unowned";
                break;
            case 1:
                ownerText.text = "Owner: You";
                createCarrierButton.gameObject.SetActive(true);
                buyEconButton.gameObject.SetActive(true);
                buyIndustryButton.gameObject.SetActive(true);
                buyScienceButton.gameObject.SetActive(true);
                
                break;
            case 2:
                ownerText.text = "Owner: Enemy";
                break;
        }
        RefreshUI();
        Destroy(circleObject);
        circleObject = GenerateCircle(star.transform.position, CStarScript.Range);
        carrierButtons();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            ClearUI();
            StopAllCoroutines();
            routePlannerScript.clear();
            starSelected = false;
        }
/*        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Yurr");
            RaycastHit2D ray = Physics2D.Raycast(new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            if (ray & !isOnUI)
            {
                Debug.LogWarning(ray.point.ToSafeString());
                InitUI(ray.transform.gameObject);
            }
        }*/
    }
    private void BuyEconomy()
    {
        if (playerMoney >= econPrice)
        {
            playerMoney -= econPrice;
            CStarScript.EconCount += 1;
            Debug.Log(CStarScript.EconCount);
        }
        else
        {
            messagePrompt.gameObject.SetActive(true);
            messagePrompt.text = "You're too broke to buy this";
            Debug.Log("Pooron");
        }
        RefreshUI();
    }
    private void BuyIndustry()
    {
        if (playerMoney >= industryPrice)
        {
            playerMoney -= industryPrice;
            CStarScript.IndustryCount += 1;
        }
        else
        {
            messagePrompt.gameObject.SetActive(true);
            messagePrompt.text = "You're too broke to buy this";
            Debug.Log("Pooron");
        }
        RefreshUI();
    }
    private void BuyScience()
    {
        if (playerMoney >= sciencePrice)
        {
            playerMoney -= sciencePrice;
            CStarScript.ScienceCount += 1;
        }
        else
        {
            messagePrompt.gameObject.SetActive(true);
            messagePrompt.text = "You're too broke to buy this";
            Debug.Log("Pooron");
        }
        RefreshUI();
    }
    void OnTickButtonPress()
    {

        tickCounter++;
        Debug.Log("New tick");
        if (tickCounter % 20 == 0)
        {
            cycleCount++;
            playerMoney += baseIncomePerCycle + (playerScript.NewCycle(cycleCount) * 10);

        };
        cycleEvent.CurrentCycle = cycleCount;
        cycleEvent.CurrentTick = tickCounter;
        NewTick.Invoke(this, cycleEvent);
        RefreshUI();
        ClearUI();
    }

    void carrierButtons()
    {
        foreach (Transform child in carrierButtonParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        int y = 0;
        foreach (GameObject i in CStarScript.CarrierList)
        {
            GameObject carrierButton = Instantiate(carrierButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity, carrierButtonParent.transform);
            carrierButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y);
            CarrierButtonScript cBScript = carrierButton.GetComponent<CarrierButtonScript>();
            cBScript.init(i, ownerColourScript.GetMainColour(owner), this, "TestingCarrier");
            carrierButton.transform.GetChild(3).gameObject.GetComponent<RawImage>().texture = getSpecImage(cBScript.carrierSpecialist);

            y += 50;
            if (carrierButtonParent.GetComponent<RectTransform>().sizeDelta.y < y)
            {
                carrierButtonParent.GetComponent<RectTransform>().sizeDelta = new Vector2(carrierButtonParent.GetComponent<RectTransform>().sizeDelta.x, y);
            }
        }
        Debug.Log("carrierButtons finished");
    }
    public void carrierButtonPressed(GameObject linkedCarrier)
    {
/*        StarInfo.SetActive(false);
        CarrierInfo.SetActive(true);*/
        switchPanels(1);
        currentCarrier = linkedCarrier;
    }
    public void carrierMenuBlueButtonPressed()
    {

/*        tempStar = currentStar;
        TStarScript = tempStar.GetComponent<StarScript>();*/
        starSelected = false;
        messagePrompt.text = "Select a star within range";
        messagePrompt.gameObject.SetActive(true);

        routePlannerScript.init(currentCarrier);
        ClearUI();
        Destroy(circleObject);
        circleObject = GenerateCircle(currentStar.transform.position, CStarScript.Range);
        /*        StartCoroutine(RoutePlanerCoroutine());*/
    }
/*    IEnumerator RoutePlanerCoroutine()
    {
        Debug.Log("Coroutine started");
        while (!isRouteStarSelected)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("RoutePlanerCoroutine stopped");
                yield break;
            }
            yield return null;  // Wait until the next frame
        }
        if (isRouteStarSelected)
        {
            if (Mathf.RoundToInt(Vector2.Distance(tempStar.transform.position, currentStar.transform.position)) > TStarScript.Range)
            {
                //Some pathfinding script
            }
            else
            {
                List<GameObject> starWaypoints = currentCarrier.GetComponent<ShipController>().starWaypoints;
                starWaypoints.Add(selectedStar);
                routePlannerScript.updateUI(starWaypoints);
            }
            isRouteStarSelected = false;
            yield return null;
        }

    }*/

    public void carrierInfoBackButtonPressed()
    {
        Debug.Log("Last clicked star: " + lastClickedStar);
        Debug.Log("Current star: " + currentStar);
        Debug.Log("Panel: " + (panel != null ? "Panel is not null" : "Panel is null"));
        /*CarrierInfo.SetActive(false);*/
        switchPanels(0);
        if (lastClickedStar != null)
        {
            InitUI(lastClickedStar);
        }
        else
        {
            ClearUI();
        }
        Debug.Log("Back button pressed");
    }
    void OnCreateCarrierPress()
    {   
        if (playerMoney >= carrierCost)
        {
            playerMoney -= carrierCost;
            //If the user selects the initial star
            GameObject ship = GameObject.Instantiate(shipPrefab, currentStar.transform.position, Quaternion.identity) as GameObject;
            ship.transform.parent = currentStar.transform;
            ShipController shipController = ship.GetComponent<ShipController>();
            carrierCount++;
            shipController.Init(nextTickButton, currentStar, inputedShipCount, carrierCount, playerScript);
            RefreshUI();
        }
        else
        {
            Debug.Log("It costs " + carrierCost + "$ to do that");
        }



    }
    IEnumerator SecondStarSelectCoroutine()
    {
        Debug.Log("Coroutine started");
        while (!starSelected)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("SecondStarSelect coroutine stopped");
                yield break;
            }
            yield return null;  // Wait until the next frame
        }
        if (starSelected)
        {
            Debug.Log("Selected");
            Debug.Log(playerRange);
            if (Mathf.RoundToInt(Vector2.Distance(tempStar.transform.position, currentStar.transform.position)) > TStarScript.Range)
            {
                messagePrompt.text = "That star is not within range!";
                messagePrompt.gameObject.SetActive(true);
            }
            else
            {
                shipInput.gameObject.SetActive(true);
                shipInputButton.gameObject.SetActive(true);
                messagePrompt.text = "Input the desired amount of ships to the carrier";
                messagePrompt.gameObject.SetActive(true);

/*                slingshotToggleButton.gameObject.SetActive(true);*/

                StartCoroutine(CheckForEnter());
            }
            yield break;
        }

    }
    IEnumerator CheckForEnter()
    {   
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        WhenInputConfirmed();
        yield break;
    }

    void WhenInputConfirmed()
    {
        messagePrompt.gameObject.SetActive(false);
        Debug.Log(inputedShipCountTextField.text);
        if(int.TryParse(shipInput.text, out int inputedShipCount))
        {
            Debug.Log(inputedShipCount);
        }
        else
        {
            Debug.Log("Damn, something broke");
        }


        if (inputedShipCount > TStarScript.GarrisonCount)
        {
            messagePrompt.gameObject.SetActive(true);
            messagePrompt.text = "The number you inputed is too large";
            shipInput.gameObject.SetActive(false);
            shipInputButton.gameObject.SetActive(false);
        }
        else if (tempStar.transform.position == currentStar.transform.position)
        {
            //If the user selects the initial star
            GameObject ship = GameObject.Instantiate(shipPrefab, tempStar.transform.position, Quaternion.identity) as GameObject;
            ship.transform.parent = tempStar.transform;
            ShipController shipController = ship.GetComponent<ShipController>();
            carrierCount++;
            shipController.Init(nextTickButton, tempStar, inputedShipCount, carrierCount, playerScript);
            ClearUI();
        }
        else
        {
            //If the user sends it to another star
            GameObject ship = GameObject.Instantiate(shipPrefab, tempStar.transform.position, Quaternion.identity) as GameObject;
            ship.transform.parent = tempStar.transform;
            ShipController shipController = ship.GetComponent<ShipController>();
            carrierCount++;
            shipController.Init(nextTickButton, tempStar, inputedShipCount, carrierCount, playerScript);
            shipController.SendToStar(currentStar);
            ClearUI();
        }
    }

    public void ClearUI()
    {
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel is null when trying to deactivate it!");
        }

        buyEconButton.gameObject.SetActive(false);
        buyIndustryButton.gameObject.SetActive(false);
        buyScienceButton.gameObject.SetActive(false);

        econIndicator.text = string.Empty;
        industryIndicator.text = string.Empty;
        scienceIndicator.text = string.Empty;

        postitionText.text = string.Empty;
        ownerText.text = string.Empty;
        starNameText.text = string.Empty;

        planetaryText.text = string.Empty;
        gasText.text = string.Empty;
        habitableText.text = string.Empty;

        econPriceText.text = string.Empty;
        industryPriceText.text = string.Empty;
        sciencePriceText.text = string.Empty;
        createCarrierButton.gameObject.SetActive(false);
        messagePrompt.gameObject.SetActive(false);
        shipInput.gameObject.SetActive(false);
        shipInputButton.gameObject.SetActive(false);

        carrierText.text = string.Empty;
        carrierList.gameObject.SetActive(false);

        CarrierInfo.SetActive(false);
        StarInfo.SetActive(false);
        if (IsInvoking("CheckForEnter"))
        {
            StopCoroutine(CheckForEnter());
        }
/*        if (IsInvoking("RoutePlanerCoroutine()"))
        {
            StopCoroutine(RoutePlanerCoroutine());
        }*/
    
        Destroy(circleObject);
    }

    public void switchPanels(int panelNumber)
    {
        switch (panelNumber)
        {
            case 0:
                CarrierInfo.SetActive(false);
                StarInfo.SetActive(true);
                return;
            case 1:
                CarrierInfo.SetActive(true);
                StarInfo.SetActive(false);
                return;
        }
    }


    static GameObject GenerateCircle(Vector2 pos, int radius)
    {

        GameObject circleObject = new GameObject("circleObject");
        LineRenderer circleMaker = circleObject.AddComponent<LineRenderer>();
        circleMaker.material = new Material(Shader.Find("Sprites/Default"));

        circleMaker.startColor = Color.blue;
        circleMaker.endColor = Color.blue;
        circleMaker.startWidth = 0.5f;
        circleMaker.endWidth = 0.5f;

        int steps = (int)MathF.Round(2 * MathF.PI * radius);
        circleMaker.positionCount = (steps) + 2;

        for (int i = 0; i < (steps) + 2; i++)
        {
            float circumferenceProgress = (float)i / steps;

            float currentRadian = (circumferenceProgress * 2 * Mathf.PI);

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector2 position = new Vector2(x, y) + pos;

            circleMaker.SetPosition(i, position);
        }
        circleMaker.material = new Material(Shader.Find("Sprites/Default"));
        return circleObject;
    }


}
