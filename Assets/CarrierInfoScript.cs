using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CarrierInfoScript : MonoBehaviour
{
    [SerializeField] PromptUIScript promptUIScript;
    [SerializeField] TMP_Text carrierNameText;
    [SerializeField] TMP_Text shipCountText;
    [SerializeField] TMP_Text carrierDestinationText;
    [SerializeField] TMP_Text specialistsText;
    [SerializeField] TMP_Text weaponStrengthText;
    [SerializeField] TMP_Text ownerText;

    private ShipController linkedController;

    public void init(ShipController shipController)
    {
        linkedController = shipController;

        carrierNameText.text = linkedController.Name;
        shipCountText.text = "Ships: " + linkedController.ShipCount.ToString();
        GameObject destination = linkedController.starWaypoints.Count == 0 ? null : linkedController.starWaypoints[linkedController.starWaypoints.Count - 1];
        carrierDestinationText.text = "Destination: " + (destination == null ? "None" : destination.name);
        specialistsText.text = "Specialist: "+ linkedController.Specialist;
        weaponStrengthText.text = "Weapons: " + 67.ToString();
        ownerText.text = "Owner: " + linkedController.Owner.ToString();
    }
    public void refresh()
    {
        carrierNameText.text = linkedController.Name;
        shipCountText.text = "Ships: " + linkedController.ShipCount.ToString();
        GameObject destination = linkedController.starWaypoints.Count == 0 ? null : linkedController.starWaypoints[linkedController.starWaypoints.Count - 1];
        carrierDestinationText.text = "Destination: " + (destination == null ? "None" : destination.name);
        specialistsText.text = "Specialist: " + linkedController.Specialist;
        weaponStrengthText.text = "Weapons: " + 67.ToString();
        ownerText.text = "Owner: " + linkedController.Owner.ToString();
    }


    public void editShipCount()
    {
        promptUIScript.init(linkedController.gameObject);
    }


}
