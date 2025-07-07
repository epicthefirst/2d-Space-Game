//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using Unity.VisualScripting;

//public class CapitalStar : MonoBehaviour
//{
//    //Numbers to mess with
//    private readonly float qualityMultiplier = 3;
//    readonly List<int> capitalList = new List<int> { 0, 0, 2, 1, 1, 1 };

//    //No touchy
//    [SerializeField] GameObject canvas;
//    private int steps;
//    private Vector2 positionSelf;
//    [SerializeField] GameObject parent;
//    private LineRenderer orbitMaker;
//    private List<int> planetList;
//    private MapGeneration mapGeneration;
//    private TextMeshPro ships;
//    private int owner = 1;
//    private int econ = 0;
//    private int econPrice;
//    private int industry = 0;
//    private int industryPrice;
//    private int science = 0;
//    private int sciencePrice;
//    private int gasCount = 3;
//    private int planetaryCount = 2;
//    private int habitableCount = 1;
//    private int shipCount = 100;

//    private GameObject insidePolygon;
//    private GameObject borderPolygon;
//    private GameObject starSpawn;
//    private Dictionary<int, Material[]> materialDictionary;
//    private Material[] materials;

//    // Start is called before the first frame update
//    private void Start()
//    {
//        steps = (int)qualityMultiplier * 20;
//        for (int i = 0; i < capitalList.Count; i++)
//        {
//            DrawOrbit(steps, i + 1, capitalList[i]);
            
//        }
//        // Create the Text GameObject.
//        GameObject go = new GameObject();
//        go.transform.parent = parent.transform;
//        go.transform.position = parent.transform.position;
//        TextMeshPro ships = go.AddComponent<TextMeshPro>();
//        ships.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 3);
//        ships.enableAutoSizing = true;
//        ships.transform.position += new Vector3(4, 0, 0);
//        ships.text = shipCount.ToString();
//    }

//    void DrawOrbit(int steps, float radius, int orbitType)
//    {
//        GameObject orbitObject = new GameObject("OrbitCaptial");
//        orbitObject.transform.SetParent(parent.transform);
//        LineRenderer orbitMaker = orbitObject.AddComponent<LineRenderer>();
//        orbitMaker.material = new Material(Shader.Find("Sprites/Default"));

//        //bool isPlanetary = orbitType == 1;

//        //orbitMaker.startColor = isPlanetary ? Color.red : Color.gray;
//        //orbitMaker.endColor = isPlanetary ? Color.red : Color.gray;
//        //orbitMaker.startWidth = isPlanetary ? 0.5f : 0.3f;
//        //orbitMaker.endWidth = isPlanetary ? 0.5f : 0.3f;


//        //if (orbitType == 1)
//        //{
//        //    orbitMaker.startColor = Color.red;
//        //    orbitMaker.endColor = Color.red;
//        //    orbitMaker.startWidth = 0.5f;
//        //    orbitMaker.endWidth = 0.5f;
//        //}
//        //else{
//        //{
//        //    orbitMaker.startColor = Color.gray;
//        //    orbitMaker.endColor = Color.gray;
//        //    orbitMaker.startWidth = 0.3f;
//        //    orbitMaker.endWidth = 0.3f;
//        //}

//        switch (orbitType)
//        {
//            case 0:
//                //Planetary
//                orbitMaker.startColor = Color.gray;
//                orbitMaker.endColor = Color.gray;
//                orbitMaker.startWidth = 0.3f;
//                orbitMaker.endWidth = 0.3f;
//                break;
//            case 1:
//                //Gas
//                orbitMaker.startColor = Color.red;
//                orbitMaker.endColor = Color.red;
//                orbitMaker.startWidth = 0.5f;
//                orbitMaker.endWidth = 0.5f;
//                break;
//            case 2:
//                //Habitable
//                orbitMaker.startColor = Color.green;
//                orbitMaker.endColor = Color.green;
//                orbitMaker.startWidth = 0.3f;
//                orbitMaker.endWidth = 0.3f;
//                break;

//        }



//        orbitMaker.positionCount = (steps * 2 / 3) + 1;

//        float cutStartAngle = Mathf.PI / 3;
//        for (int i = 0; i < (steps * 2 / 3) + 1; i++)
//        {
//            float circumferenceProgress = (float)i / steps;

//            float currentRadian = (circumferenceProgress * 2 * Mathf.PI) + cutStartAngle;

//            float xScaled = Mathf.Cos(currentRadian);
//            float yScaled = Mathf.Sin(currentRadian);

//            float x = xScaled * radius;
//            float y = yScaled * radius;

//            Vector2 position = new Vector2(x, y) + positionSelf;

//            orbitMaker.SetPosition(i, position);
//        }




//        //orbitMaker.SetPosition(steps, orbitMaker.GetPosition(0));
//        orbitMaker.material = new Material(Shader.Find("Sprites/Default"));
//    }
//    public void shipInbound(int shipShipCount, int shipOwner)
//    {
//        //0 = unowned, 1 = player owned, 2 = enemy owned
//        Debug.Log("I have ran");
//        if (owner == 0)
//        {
//            shipCount = shipShipCount;
//            refresh();
//        }
//    }
//    void refresh()
//    {

//        ships = gameObject.GetComponentInChildren<TextMeshPro>();
//        ships.text = shipCount.ToString();
//        materials = materialDictionary.GetValueOrDefault(owner);
//        insidePolygon.GetComponent<MeshRenderer>().material = materials[0];
//        borderPolygon.GetComponent<MeshRenderer>().material = materials[1];

//    }
//    public void SendPolygon(GameObject insidePolygon, GameObject borderPolygon, Dictionary<int, Material[]> materialDictionary)
//    {
//        this.insidePolygon = insidePolygon;
//        this.borderPolygon = borderPolygon;

//        this.materialDictionary = materialDictionary;
//    }


//    void OnMouseOver()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {

//            UIManager uIManager = canvas.GetComponent<UIManager>();

//            StarData star = new StarData(-1, positionSelf, capitalList, owner, econ, industry, science, 50, gameObject);

//            uIManager.InitUI(star);
//        }
//    }


//}