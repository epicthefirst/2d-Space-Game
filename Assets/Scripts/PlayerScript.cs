using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private string playerName;
    private Vector2 playerCapital;
    private int totalPlayerEconCount;
    private int totalPlayerIndustryCount;
    private int totalPlayerScienceCount;
    private int totalPlayerShipCount;
    private int playerWeaponLevel;
    private int playerMnufacturingLevel;
    private int playerScanningLevel;
    private int playerRangeLevel;
    private List<GameObject> playerCarrierList = new List<GameObject>();
    private List<GameObject> playerStarList = new List<GameObject>();


    public int cycleCount = 0;
    private List<GameObject> playerStars = new List<GameObject>();
    public int NewCycle(int cycleCount)
    {
        this.cycleCount = cycleCount;
        Debug.Log("Cycle:" + cycleCount);
        Debug.Log("Previous econCount" + totalPlayerEconCount);
        totalPlayerEconCount = 0;
        foreach (GameObject s in playerStars)
        {
            StarScript ss = s.GetComponent<StarScript>();
            totalPlayerEconCount += ss.EconCount;
            totalPlayerIndustryCount += ss.IndustryCount;
            totalPlayerScienceCount += ss.ScienceCount;
        }
        Debug.Log("New econCount" + totalPlayerEconCount);
        Debug.Log("New industryCount" + totalPlayerIndustryCount);
        Debug.Log("New scienceCount" + totalPlayerScienceCount);
        return totalPlayerEconCount;
    }

    public void newCarrier(GameObject newCarrier)
    {
        playerCarrierList.Add(newCarrier);
    }
    public void removeCarrier(GameObject carrierRemove)
    {
        playerCarrierList.Remove(carrierRemove);
    }
    public void addStar(GameObject newStar)
    {
        playerStarList.Add(newStar);
    }
    public void removeStar(GameObject starRemove)
    {
        playerStarList.Remove(starRemove);
    }
    public void test()
    {
        Debug.Log("test success");
    }
}
