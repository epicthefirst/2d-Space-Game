using System;
using System.Collections.Generic;
using System.Linq;
using csDelaunay;
using Newtonsoft.Json.Linq;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapGeneration : MonoBehaviour
{  //Stuff to mess with (yay!)
    public int qualityMultiplier = 5;
    public int numberOfCircles = 5;
    public double offset = 20d;
    [SerializeField] int minRandOffset = 1;
    [SerializeField] int maxRandOffset = 5;
    public int seed = -2;
    private int range = 75;
    private int[] enemyCapital = { 42 };
    private List<int> capitalList = new List<int> { 0, 0, 2, 1, 1, 1 };
    public string[] starNames = new string[] { "Absolutno", "Acamar", "Achernar", "Achird", "Acrab", "Acrux", "Acubens", "Adhafera", "Adhara", "Adhil", "Ain", "Ainalrami",
        "Aladfar", "Alasia", "Albaldah", "Albali", "Albireo", "Alchiba", "Alcor", "Alcyone", "Aldebaran", "Alderamin", "Aldhanab", "Aldhibah", "Aldulfin", "Alfirk", "Algedi", "Algenib",
        "Algieba", "Algol", "Algorab", "Alhena", "Alioth", "Aljanah", "Alkaid", "Alkalurops", "Alkaphrah", "Alkarab", "Alkes", "Almaaz", "Almach", "Alnair", "Alnasl", "Alnilam", "Alnitak", 
        "Alniyat", "Alphard", "Alphecca", "Alpheratz", "Alpherg", "Alrakis", "Alrescha", "Alruba", "Alsafi", "Alsciaukat", "Alsephina", "Alshain", "Alshat", "Altair", "Altais", 
        "Alterf", "Aludra", "Alula Australis", "Alula Borealis", "Alya", "Alzirr", "Amadioha", "Amansinaya", "Anadolu", "Ancha", "Angetenar", "Aniara", "Ankaa", "Anser", "Antares", 
        "Arcalis", "Arcturus", "Arkab Posterior", "Arkab Prior", "Arneb", "Ascella", "Asellus Australis", "Asellus Borealis", "Ashlesha", "Aspidiske", "Asterope", "Atakoraka", "Athebyne", 
        "Atik", "Atlas", "Atria", "Avior", "Axolotl", "Ayeyarwady", "Azelfafage", "Azha", "Azmidi", "Baekdu", "Barnard's Star", "Baten Kaitos", "Beemim", "Beid", "Belel", "Belenos", 
        "Bellatrix", "Berehynia", "Betelgeuse", "Bharani", "Bibha", "Biham", "Bosona", "Botein", "Brachium", "Bubup", "Buna", "Bunda", "Canopus", "Capella", "Caph", "Castor", "Castula", 
        "Cebalrai", "Ceibo", "Celaeno", "Cervantes", "Chalawan", "Chamukuy", "Chaophraya", "Chara", "Chason", "Chechia", "Chertan", "Citadelle", "Citala", "Cocibolca", "Copernicus", 
        "Cor Caroli", "Cujam", "Cursa", "Dabih", "Dalim", "Deneb", "Deneb Algedi", "Denebola", "Diadem", "Dingolay", "Diphda", "Diwo", "Diya", "Dofida", "Dombay", "Dschubba", "Dubhe", 
        "Dziban", "Ebla", "Edasich", "Electra", "Elgafar", "Elkurud", "Elnath", "Eltanin", "Emiw", "Enif", "Errai", "Fafnir", "Fang", "Fawaris", "Felis", "Felixvarela", "Flegetonte", 
        "Fomalhaut", "Formosa", "Franz", "Fulu", "Fumalsamakah", "Funi", "Furud", "Fuyue", "Gacrux", "Gakyid", "Geminga", "Giausar", "Gienah", "Ginan", "Gloas", "Gomeisa", "Goku", //Goku
        "Grumium", "Gudja", "Gumala", "Guniibuu", "Hadar", "Haedus", "Hamal", "Hassaleh", "Hatysa", "Helvetios", "Heze", "Hoggar", "Homam", "Horna", "Hunahpu", "Hunor", "Iklil", "Illyrian", "Imai", 
        "Inquill", "Intan", "Intercrus", "Irena", "Itonda", "Izar", "Jabbah", "Jishui", "Kaffaljidhma", "Kalausi", "Kamuy", "Kang", "Karaka", "Kaus Australis", "Kaus Borealis", 
        "Kaus Media", "Kaveh", "Keid", "Khambalia", "Kitalpha", "Kochab", "Koeia", "Koit", "Kornephoros", "Kraz", "Kurhah", "La Superba", "Larawag", "Lerna", "Lesath", "Libertas", 
        "Lich", "Liesma", "Lilii Borea", "Lionrock", "Lucilinburhuc", "Lusitania", "Maasym", "Macondo", "Mago", "Mahasim", "Mahsati", "Maia", "Malmok", "Marfik", "Markab", "Markeb", 
        "Marohu", "Marsic", "Matar", "Mazaalai", "Mebsuta", "Megrez", "Meissa", "Mekbuda", "Meleph", "Menkalinan", "Menkar", "Menkent", "Menkib", "Merak", "Merga", "Meridiana", "Merope", 
        "Mesarthim", "Miaplacidus", "Mimosa", "Minchir", "Minelauva", "Mintaka", "Mira", "Mirach", "Miram", "Mirfak", "Mirzam", "Misam", "Mizar", "Moldoveanu", "Monch", "Montuno", 
        "Morava", "Moriah", "Mothallah", "Mouhoun", "Mpingo", "Muliphein", "Muphrid", "Muscida", "Musica", "Muspelheim", "Nahn", "Naledi", "Naos", "Nashira", "Nasti", "Natasha", "Nekkar", 
        "Nembus", "Nenque", "Nervia", "Nganurganity", "Nihal", "Nikawiy", "Nosaxa", "Nunki", "Nusakan", "Nushagak", "Nyamien", "Ogma", "Okab", "Paikauhale", "Parumleo", "Peacock", 
        "Petra", "Phact", "Phecda", "Pherkad", "Phoenicia", "Piautos", "Pincoya", "Pipirima", "Pipoltr", "Pleione", "Poerava", "Polaris", "Polaris Australis", "Polis", "Pollux", "Porrima", 
        "Praecipua", "Prima Hyadum", "Procyon", "Propus", "Proxima Centauri", "Ran", "Rana", "Rapeto", "Rasalas", "Rasalgethi", "Rasalhague", "Rastaban", "Regulus", "Revati", "Rigel", 
        "Rigil Kentaurus", "Rosaliadecastro", "Rotanev", "Ruchbah", "Rukbat", "Sabik", "Saclateni", "Sadachbia", "Sadalbari", "Sadalmelik", "Sadalsuud", "Sadr", "Sagarmatha", "Saiph", 
        "Salm", "Samaya", "Sansuna", "Sargas", "Sarin", "Sceptrum", "Scheat", "Schedar", "Secunda Hyadum", "Segin", "Seginus", "Sham", "Shama", "Sharjah", "Shaula", "Sheliak", "Sheratan", 
        "Sika", "Sirius", "Situla", "Skat", "Solaris", "Spica", "Sterrennacht", "Stribor", "Sualocin", "Subra", "Suhail", "Sulafat", "Syrma", "Tabit", "Taika", "Taiyangshou", "Taiyi", 
        "Talitha", "Tangra", "Tania Australis", "Tania Borealis", "Tapecue", "Tarazed", "Tarf", "Taygeta", "Tegmine", "Tejat", "Terebellum", "Tevel", "Theemin", "Thuban", "Tiaki", 
        "Tianguan", "Tianyi", "Timir", "Tislit", "Titawin", "Tojil", "Toliman", "Tonatiuh", "Torcular", "Tuiren", "Tupa", "Tupi", "Tureis", "Ukdah", "Uklun", "Unukalhai", "Uruk", "Vega", 
        "Veritate", "Vindemiatrix", "Wasat", "Wazn", "Wezen", "Wurren", "Xamidimura", "Xihe", "Xuange", "Yed Posterior", "Yed Prior", "Yildun", "Zaniah", "Zaurak", "Zavijava", "Zhang", 
        "Zibal", "Zosma", "Zubenelgenubi", "Zubenelhakrabi", "Zubeneschamali" };

    public Dictionary<int, int> orbitalPeriodsDictionary = new Dictionary<int, int>
    {
        {0, 5}, {1, 8}, {2, 13}, {3, 21}, {4, 34}, {5, 55}, {6, 89}
        // 1,      1,      2,       3,       5,      ,8       ,13
        // 36,348,312/N, 22,717,695/N, 27,960,240/N, 25,963,080/N, 21,381,360/N, 26,435,136/N, 26,546,520/N 
        //Where N is = to 181741560 (The LCM of 5,8,13,21,34,55,89)
    }; 


    //Stuff to not mess with
    IDictionary<Tuple<int, int>, RingData> ringDictionary = new Dictionary<Tuple<int, int>, RingData>();
    List<RingData> badStars = new List<RingData>();
    //List<Tuple<RingData, RingData>> badStarPairs = new List<Tuple<RingData, RingData>>();
    int totalStarCount = 0;
    private System.Random random;
    Vector2 lastStarPos;
    Vector2 vectorAdjust;
    private GameObject starSpawn;
    public GameObject star;

    //0 = unowned, 1 = player owned, 2 = enemy owned
    private Dictionary<GameObject, int> dictionary = new Dictionary<GameObject, int> { };
    [SerializeField] GameObject canvasObject;
    public GameObject capital;
    private List<int> positiveOrNegative = new List<int>() { -1, 1 };
    public UIManager uiManager;
    public PlayerScript playerScript;
    public RingData[][] arrayOfRings;
    private RingData[] tempArray = new RingData[3];

    //Terrestrial, gas, habitable; add more if need be
    public GameObject[] planetArray = new GameObject[3];



    // Start is called before the first frame update
    void Start()
    {
/*      
        switch (planetList[i])
        {
            case 0:
                //Terrestrial
                planetMaker.startColor = Color.gray;
                planetMaker.endColor = Color.gray;
                planetMaker.startWidth = 0.6f;
                planetMaker.endWidth = 0.6f;
                radius = 0.2f;
                break;
            case 1:
                //Gas
                planetMaker.startColor = Color.red;
                planetMaker.endColor = Color.red;
                planetMaker.startWidth = 0.8f;
                planetMaker.endWidth = 0.8f;
                radius = 0.3f;
                break;
            case 2:
                //Habitable
                planetMaker.startColor = Color.green;
                planetMaker.endColor = Color.green;
                planetMaker.startWidth = 0.6f;
                planetMaker.endWidth = 0.6f;
                radius = 0.2f;
                break;
        }*/


/*        GameObject terrestrial = new GameObject("Terrestrial");
*//*        MeshRenderer terrestrialMR = terrestrial.AddComponent<MeshRenderer>();
        terrestrialMR.material.color = Color.gray;*/
/*        MeshFilter terrestrialMF = terrestrial.AddComponent<MeshFilter>();
        terrestrialMF.mesh = polyMesh(0.3f, qualityMultiplier * 8);*//*
        planetArray[0] = terrestrial;*/

/*        GameObject gas = new GameObject("Gas_giant");
        MeshRenderer gasMR = gas.AddComponent<MeshRenderer>();
        gasMR.material.color = Color.red;
        MeshFilter gasMF = gas.AddComponent<MeshFilter>();
        gasMF.mesh = polyMesh(0.5f, qualityMultiplier * 10);
        planetArray[1] = gas;

        GameObject habitable = new GameObject("Habitable");
        MeshRenderer habitableMR = habitable.AddComponent<MeshRenderer>();
        habitableMR.material.color = Color.green;
        MeshFilter habitableMF = habitable.AddComponent<MeshFilter>();
        habitableMF.mesh = polyMesh(0.3f, qualityMultiplier * 12);
        planetArray[2] = habitable;*/








        arrayOfRings = new RingData[numberOfCircles - 1][];

        
        random = new System.Random(seed);
        starNames = starNames.OrderBy(x => random.Next()).ToArray();
        capitalStar();
        for (int i = 0; i < numberOfCircles; i++)
        {
            
            fillDistanceFromCenter(offset * i, i - 1);
        }
        foreach (RingData[] ringDataRing in arrayOfRings)
        {
            //First star check
            tempArray[0] = ringDataRing[ringDataRing.Length - 1];
            tempArray[1] = ringDataRing[0];
            tempArray[2] = ringDataRing[1];
            DistanceCheck(tempArray);
            //Star 2 to ^2 check
            for (int j = 1; j < ringDataRing.Length - 2; j++)
            {
                tempArray = ringDataRing.Skip(j).Take(3).ToArray();
                DistanceCheck(tempArray);
            }
            //Last star check
            tempArray[0] = ringDataRing[ringDataRing.Length - 2];
            tempArray[1] = ringDataRing[ringDataRing.Length - 1];
            tempArray[2] = ringDataRing[0];
            DistanceCheck(tempArray);

            //foreach (RingData ringData in ringDataRing)
            //{
            //    Vector2.Distance(ringData.position, ringDataRing.[])
            //}
        }
        Debug.Log(badStars.Count); //Redo all this shit
        if (badStars.Count % 2 == 0)
        {
            for (int i = 0; i < badStars.Count; i += 2)
            {   //i <=> i + 1
                float moveX = badStars[i].position.x - badStars[i+1].position.x;
                float moveY = badStars[i].position.y - badStars[i + 1].position.y;
                Vector2 vector = new Vector2(moveX, moveY);
                float distance = MathF.Sqrt(badStars[i].distance);
                badStars[i].star.transform.position += ((Vector3)vector / distance) * 5; 
                badStars[i + 1].star.transform.position -= ((Vector3)vector / distance) * 5;
            }
        }
        //foreach (RingData bs in badStars)
        //{
        //    //var tempDict = dictionary.FirstOrDefault(t => t.Key == bs.star);
        //    //if (tempDict.Key != null)
        //    //{
        //    //    Debug.Log("bs.star is in the dictionary");
        //    //}
        //    //dictionary.Add(bs.star, dictionary[bs.originalStar]);
        //    //dictionary.Remove(bs.originalStar);
        //    Instantiate(GenerateCircle(bs.position, 10), bs.position, Quaternion.identity);
        //}

        VoronoiDiagram voronoiDiagram = this.GetComponent<VoronoiDiagram>();
        voronoiDiagram.Init(dictionary);




/*        int Vertices = 6;
        Vector3 initialPos = new Vector3(25, 25);

        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = Vertices;
        lr.startWidth = 0.25f;
        lr.endWidth = 0.25f;
        //Quaternion rotate=Quaternion.Euler(0,0,360/Vertices);
        Vector3[] positions = new Vector3[Vertices];
        positions[0] = initialPos;
        for (int i = 1; i < Vertices; i++)
        {
            positions[i] = Quaternion.Euler(0, 0, 360 * i / Vertices) * initialPos;
        }
        lr.SetPositions(positions);
        lr.loop = true;
        Debug.LogWarning("Done");*/

    }

    private List<Tuple<int, int>> slingshotPeriodCalculator(int planetCount)
    {
        /*        foreach (GameObject star in dictionary.Keys)
                {
                    List<int> orbitList = star.GetComponent<StarScript>().OrbitList;
        *//*            orbitList.Count;*//*

                };*/
        List<Tuple<int, int>> timingsList = new List<Tuple<int, int>>();
        //Gives the timings for each planet, in a tick delay
        for (int i = 0; i < planetCount; i++)
        {
            //Timing, then total
            timingsList.Add(new Tuple<int, int>(random.Next(0, orbitalPeriodsDictionary[i]), orbitalPeriodsDictionary[i]));
        }

        //Add stuff here for later

        return timingsList;

    }

    private void DistanceCheck(RingData[] array)
    {
        float array1x = array[1].position.x;
        float array1y = array[1].position.y;

        float xMultiply12 = (array1x - array[2].position.x);
        float yMultiply12 = (array1y - array[2].position.y);

        float xMultiply10 = (array1x - array[0].position.x);
        float yMultiply10 = (array1y - array[0].position.y);

        float distancePreRoot1to2 = (xMultiply12 * xMultiply12) + (yMultiply12 * yMultiply12);
        float distancePreRoot1to0 = (xMultiply10 * xMultiply10) + (yMultiply10 * yMultiply10);

        if (distancePreRoot1to2 < 400) {
            array[1].distance = distancePreRoot1to2;
            badStars.Add(array[1]);
            //badStarPairs.Add(new Tuple<RingData, RingData>(array[1], array[2]));


        }
        else if (distancePreRoot1to0 < 400)
        {
            array[1].distance = distancePreRoot1to2;
            badStars.Add(array[1]);
            //badStarPairs.Add(new Tuple<RingData, RingData>(array[1], array[0]));
        }
    }
    void capitalStar()
    {
        //0 = unowned, 1 = player owned, 2 = enemy owned
        dictionary.Add(capital, 1);
        
        StarScript capitalScript = capital.AddComponent<StarScript>();
        capitalScript.Initialize(-1, "Capital", capitalList, slingshotPeriodCalculator(capitalList.Count), range, 1, canvasObject, 100, planetArray);
        capitalScript.EconCount = 9;
        capitalScript.IndustryCount = 5;
        capitalScript.ScienceCount = 2;
        //playerScript.addStar(new StarData(ref capital));
        ringDictionary.Add((new Tuple<int,int>(-1,1)), new RingData(capital, capital.transform.position ,-1, 1));
        capitalScript.isAwake = true;
    }

    //Makes the star pattern
    void fillDistanceFromCenter(double distance, int k)
    {


        int starsPerCircle = (int)Math.Floor((distance * Math.PI) / offset * 3);
        if (k >= 0)
        {
            arrayOfRings[k] = new RingData[starsPerCircle];
        }
        for (int i = 0; i < starsPerCircle; i++)
        {

            
            //Debug.Log(starsPerCircle);
            addPoint(starsPerCircle, distance, i, k);
            totalStarCount = totalStarCount + 1;
            

        }

    }
    public Vector2 getPos(double starsPerCircle, double distance, int i)
    {



        //Calculate the angle for each star
        double angle = 360.0 / starsPerCircle * i + (360.0 / starsPerCircle / 3);
        double radians = angle * Mathf.Deg2Rad;

        //Calculate x and y positions based on the distance and angle
        var x = Math.Cos(radians) * (distance);
        var y = Math.Sin(radians) * (distance);
        vectorAdjust = new Vector2((positiveOrNegative[random.Next(0, positiveOrNegative.Count)]) * (random.Next(0, maxRandOffset - minRandOffset) + minRandOffset), (positiveOrNegative[random.Next(0, positiveOrNegative.Count)]) * (random.Next(0, maxRandOffset - minRandOffset) + minRandOffset));
        return new Vector2((float)x, (float)y) + vectorAdjust;
    }


    //Makes the stars and the orbits
    public GameObject addPoint(double starsPerCircle, double distance, int i, int k)
    {

        Vector2 pos;

        pos = getPos(starsPerCircle, distance, i);


        lastStarPos = pos;

        List<int> planetList = new List<int>();
        int planetAmount = (int)MathF.Min(random.Next(1, 16), MathF.Min(random.Next(2, 5), random.Next(2, 5)));

        //List of planets for each star, gets sent to Orbits and StarScript
        for (int j = 0; j < planetAmount; j++)
        {
            int choice = random.Next(0, 2);
            planetList.Add(choice);
        }
        planetList.Sort();


        starSpawn = GameObject.Instantiate(star, pos, Quaternion.identity) as GameObject; //Spawn stars
        starSpawn.name = totalStarCount.ToString();

        Orbits orbits = starSpawn.AddComponent<Orbits>();
        StarScript starScript = starSpawn.AddComponent<StarScript>();




        if (enemyCapital.Contains(totalStarCount))
        {
            starScript.Initialize(totalStarCount, starNameMethod(totalStarCount), capitalList, slingshotPeriodCalculator(capitalList.Count), range, 2, canvasObject, 100, planetArray);
            starScript.isAwake = true;
            dictionary.Add(starSpawn, 2);
        }
        else
        {
            starScript.Initialize(totalStarCount, starNameMethod(totalStarCount), planetList, slingshotPeriodCalculator(planetAmount), range, 0, canvasObject, 0, planetArray);
            dictionary.Add(starSpawn, 0);
        }
        //Debug.Log(k + "/" + i);
        arrayOfRings[k][i] = new RingData(starSpawn, pos, k, i);
        return starSpawn;
    }
    private string starNameMethod(int starCount)
    {
        if (starCount <  starNames.Length)
        {
            return starNames[starCount];
        }
        else
        {
            return (starCount-starNames.Length).ToString();
        }
    }




    //
    //
    //Testing methods
    //
    //

    static GameObject GenerateCircle(Vector2 pos, int radius)
    {

        GameObject circleObject = new GameObject("circleObject");
        LineRenderer circleMaker = circleObject.AddComponent<LineRenderer>();
        circleMaker.material = new Material(Shader.Find("Sprites/Default"));

        circleMaker.startColor = Color.red;
        circleMaker.endColor = Color.red;
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


    public Mesh polyMesh(float radius, int n)
    {
        Mesh mesh = new Mesh();

        //verticies
        List<Vector3> verticiesList = new List<Vector3> { };
        float x;
        float y;
        for (int i = 0; i < n; i++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / n);
            y = radius * Mathf.Cos((2 * Mathf.PI * i) / n);
            verticiesList.Add(new Vector3(x, y, 0f));
        }
        Vector3[] verticies = verticiesList.ToArray();

        //triangles
        List<int> trianglesList = new List<int> { };
        for (int i = 0; i < (n - 2); i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        //normals
        List<Vector3> normalsList = new List<Vector3> { };
        for (int i = 0; i < verticies.Length; i++)
        {
            normalsList.Add(-Vector3.forward);
        }
        Vector3[] normals = normalsList.ToArray();

        //initialise
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.normals = normals;

        return mesh;

        //Dont need
/*        //polyCollider
        polyCollider.pathCount = 1;

        List<Vector2> pathList = new List<Vector2> { };
        for (int i = 0; i < n; i++)
        {
            pathList.Add(new Vector2(verticies[i].x, verticies[i].y));
        }
        Vector2[] path = pathList.ToArray();

        polyCollider.SetPath(0, path);*/
    }
}