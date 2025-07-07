using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarData
{
    //Stuff you can ask for
    public int Id;
    public string Name;
    public Vector2 Position;
    public List<int> OrbitList;
    public int Owner;
    public int EconPrice;
    public int IndustryPrice;
    public int SciencePrice;
    public int PlanetaryCount;
    public int GasCount;
    public int HabitableCount;
    public int EconCount;
    public int IndustryCount;
    public int ScienceCount;
    public int Range;
    public GameObject Star;
    public int ShipCount;
    public StarScript StarScript;

    public StarData(ref int id, ref string name, ref List<int> orbitList, ref int owner, ref int shipCount, ref int econCount, ref int industryCount, ref int scienceCount, ref int range, GameObject star)
    {
        this.Id = id;
        this.Name = name;
        this.Position = star.transform.position;
        this.OrbitList = orbitList;
        this.Star = star;
        this.StarScript = star.GetComponent<StarScript>();
        //Above this won't change
        this.Owner = owner;
        this.Range = range;
        this.ShipCount = shipCount;
        this.EconCount = econCount;
        this.IndustryCount = industryCount;
        this.ScienceCount = scienceCount;

        Debug.Log(Position);
        


        for (int i = 0; i < orbitList.Count; i++)
        {
            switch (orbitList[i])
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
}