using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orbits : MonoBehaviour {
    //Numbers to mess with
    private static float distanceIncrease = 1.5f;
    private static int qualityMultiplier = 3;

    //No touchy
    public System.Random random;
    public LineRenderer orbitMaker;
    public List<int> planetList;
    public List<Tuple<int, int>> planetTimings;
    

    public void init(List<int> planetList, List<Tuple<int, int>> planetTimings, int tick)
    {

        this.planetList = planetList;
        this.planetTimings = planetTimings;
        if (planetList.Count == planetTimings.Count)
        {
            for (int i = 0; i < planetList.Count; i++)
            {
                 
                drawOrbit(qualityMultiplier * 20, distanceIncrease * (i + 1), planetList[i]);
                drawPlanets(qualityMultiplier * 5, i, planetList[i], tick);
            }
        }
        else
        {
            Debug.LogError("YO THERE'S AN ERROR HERE");
        }
    }




    void drawOrbit(int steps, float radius, int orbitType)
    {
        GameObject orbitObject = new GameObject("Orbit");
        orbitObject.transform.SetParent(gameObject.transform);
        LineRenderer orbitMaker = orbitObject.AddComponent<LineRenderer>();
        orbitMaker.material = new Material(Shader.Find("Sprites/Default"));

        //bool isPlanetary = orbitType == 1;

        //orbitMaker.startColor = isPlanetary ? Color.red : Color.gray;
        //orbitMaker.endColor = isPlanetary ? Color.red : Color.gray;
        //orbitMaker.startWidth = isPlanetary ? 0.5f : 0.3f;
        //orbitMaker.endWidth = isPlanetary ? 0.5f : 0.3f;


        switch (orbitType)
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

        float cutStartAngle = 5 * Mathf.PI/10;
        for (int i = 0; i < (steps * 6 / 10)+1; i++)
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
    private void drawPlanets(int steps, int i, int planetType, int tick)
    {
        float orbitalRadius = distanceIncrease * (i + 1);
        GameObject planetObject = new GameObject("Planet");
        planetObject.transform.SetParent(gameObject.transform);
        LineRenderer planetMaker = planetObject.AddComponent<LineRenderer>();
        planetMaker.material = new Material(Shader.Find("Sprites/Default"));
        float radius = 99;
        //List<int> planetList, List< int > planetTimings

        switch (planetType)
        {
            case 0:
                //Planetary
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
        }

        planetMaker.positionCount = steps + 3;
        float orbitRadians = (1 / planetTimings[i].Item2) * ((tick + planetTimings[i].Item1) % planetTimings[i].Item2) * 2 * Mathf.PI;

        float xTimingAdjust = Mathf.Cos(orbitRadians);
        float yTimingAdjust = Mathf.Sin(orbitRadians);
        for (int j = 0; j < steps + 3; j++)
        {
            float circumferenceProgress = (float)j / steps;

            float currentRadian = (circumferenceProgress * 2 * Mathf.PI);

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector2 position = new Vector2(x, y) + gameObject.transform.position.ConvertTo<Vector2>() + new Vector2(xTimingAdjust, yTimingAdjust);

            planetMaker.SetPosition(j, position);
        }

            

    }
}