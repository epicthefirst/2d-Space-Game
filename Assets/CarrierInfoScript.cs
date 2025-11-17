using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CarrierInfoScript : MonoBehaviour
{
    [SerializeField] TMP_Text carrierNameText;
    [SerializeField] TMP_Text shipCountText;
    [SerializeField] TMP_Text carrierDestinationText;
    [SerializeField] TMP_Text specialistsText;
    [SerializeField] TMP_Text weaponStrengthText;
    [SerializeField] TMP_Text ownerText;


    public void init(string carrierName, int shipCount, GameObject destination, string specialist, int weaponStrength, string owner)
    {
        carrierNameText.text = carrierName;
        shipCountText.text = "Ships: " + shipCount.ToString();
        carrierDestinationText.text = "Destination: " + (destination == null ? "None" : destination.name);
        specialistsText.text = "Specialist: "+ specialist;
        weaponStrengthText.text = "Weapons: " + weaponStrength.ToString();
        ownerText.text = "Owner: " + owner;
    }

    public void editShipCount()
    {

    }


}
