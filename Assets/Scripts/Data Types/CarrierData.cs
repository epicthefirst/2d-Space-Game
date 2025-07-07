

using UnityEngine;

public class CarrierData
{
    public int shipCount;
    public string name;
    public bool hasSpecialist;
    public string specialist = null;
    public GameObject carrier;
 
    public CarrierData(int shipCount, string carrierName, bool hasSpecialist, string carrierSpec, GameObject carrier)
    {
        this.shipCount = shipCount;
        this.name = carrierName;
        this.hasSpecialist = hasSpecialist;
        this.carrier = carrier;
        if (hasSpecialist) {this.specialist = carrierSpec;}
    }
}
