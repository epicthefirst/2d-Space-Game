using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orbits : MonoBehaviour {
    //Numbers to mess with
    private static float distanceIncrease = 1.5f;
    private static float qualityMultiplier = 3;

    //No touchy
    public System.Random random;
    public LineRenderer orbitMaker;
    List<int> planetList;
    

    public void OrbitStart(List<int> planetList)
    {
        this.planetList = planetList;
        init();
    }
    private void init()
    {
        int quality = (int)(qualityMultiplier * 20);

        for (int i = 0; i < planetList.Count; i++)
        {

            drawOrbit(quality, distanceIncrease * (i + 1), planetList[i]);

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
}