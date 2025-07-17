using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public MapGeneration mapGeneration;
    public OwnerColourScript ownerColourScript;

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
    [SerializeField] Button createShipButton;
    [SerializeField] TMP_Text messagePrompt;

    [SerializeField] TMP_InputField shipInput;
    [SerializeField] Button shipInputButton;

    //Buy buttons
    [SerializeField] Button buyEconButton;
    [SerializeField] Button buyIndustryButton;
    [SerializeField] Button buyScienceButton;

    [SerializeField] PlayerScript playerScript;

    [SerializeField] TMP_Text carrierText;
    [SerializeField] ScrollRect carrierList;

    [SerializeField] TMP_Text econIndicator;
    [SerializeField] TMP_Text industryIndicator;
    [SerializeField] TMP_Text scienceIndicator;

    [SerializeField] GameObject carrierButtonParent;
    [SerializeField] UnityEngine.Object specImageFolder;

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

    public int baseIncomePerCycle = 250;
    
    public int cycleCount = 0;
    private int carrierCount = 0;



    private int gasCount;
    private int terrestrialCount;
    private int habitableCount;

    private bool starSelected = false;

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


    //Events
    public event EventHandler NewTick;


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
        setDictionaries();
        shipInputButton.onClick.AddListener(WhenInputConfirmed);
        nextTickButton.onClick.AddListener(OnTickButtonPress);
        createShipButton.onClick.AddListener(OnCreateShipPress);

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

        }

        seedText.text = "Seed: " + mapGeneration.seed;
        moneyText.text = "Credits: " + playerMoney;
        tickText.text = "Tick: " + tickCounter;
        
        postitionText.text = "Position: " + currentStar.transform.position;

        econPriceText.text = "Economy: " + econPrice + "$";
        industryPriceText.text = "Industry: " + industryPrice + "$";
        sciencePriceText.text = "Science: " + sciencePrice + "$";

        
        return;
    }
    


    public void InitUI(GameObject star)
    {
        Debug.Log(star);
        lastClickedStar = star;
        Debug.Log(lastClickedStar);
        ClearUI();
        StarInfo.SetActive(true);
        starSelected = true;
        currentStar = star;
        CStarScript = star.GetComponent<StarScript>();
        this.orbitList = CStarScript.OrbitList;
        this.owner = CStarScript.Owner;
        Debug.Log(CStarScript.EconCount);
        switch (owner)
        {
            case 0:
                ownerText.text = "Unowned";
                break;
            case 1:
                ownerText.text = "Owner: You";
                createShipButton.gameObject.SetActive(true);
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            ClearUI();
            StopAllCoroutines();
            starSelected = false;
        }
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
        NewTick.Invoke(this, EventArgs.Empty);
        tickCounter++;
        Debug.Log("New tick");
        if (tickCounter % 20 == 0)
        {
            cycleCount++;
            playerMoney += baseIncomePerCycle + (playerScript.NewCycle(cycleCount) * 10);

        };
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
        StarInfo.SetActive(false);
        CarrierInfo.SetActive(true);
        currentStar = null;

    }
    public void carrierInfoBackButtonPressed()
    {
        Debug.Log("Last clicked star: " + lastClickedStar);
        Debug.Log("Current star: " + currentStar);
        Debug.Log("Panel: " + (panel != null ? "Panel is not null" : "Panel is null"));
        CarrierInfo.SetActive(false);
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
    void OnCreateShipPress()
    {   
        tempStar = currentStar;
        TStarScript = tempStar.GetComponent<StarScript>();
        starSelected = false;
        messagePrompt.text = "Select a star within range";
        messagePrompt.gameObject.SetActive(true);
        StartCoroutine("SecondStarSelectCoroutine");
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
            Debug.Log("Running");
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
                CheckForEnter();
            }
            yield break;
        }

    }
    IEnumerator CheckForEnter()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            WhenInputConfirmed();
            yield break;
        }
        yield return null;
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
        createShipButton.gameObject.SetActive(false);
        messagePrompt.gameObject.SetActive(false);
        shipInput.gameObject.SetActive(false);
        shipInputButton.gameObject.SetActive(false);

        carrierText.text = string.Empty;
        carrierList.gameObject.SetActive(false);
        if (IsInvoking("CheckForEnter"))
        {
            StopCoroutine(CheckForEnter());
        }
        Destroy(circleObject);
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
