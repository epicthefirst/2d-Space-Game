using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarrierButtonScript : MonoBehaviour
{
    [SerializeField] GameObject lastLinkedCarrier;
    [SerializeField] TMP_Text carrierNameText;
    [SerializeField] TMP_Text carrierShipCountText;
    [SerializeField] RawImage specialistImage;
    private UIManager uIManager;

    public void init(GameObject linkedCarrier, Color colour, UIManager uIManager)
    {
        this.uIManager = uIManager;
        lastLinkedCarrier = linkedCarrier;
        this.GetComponent<Button>().image.color = colour;
        carrierNameText.text = linkedCarrier.name;
        carrierShipCountText.text = linkedCarrier.GetComponent<ShipController>().ShipCount.ToString();


        Debug.Log("Success");
    }
    public void click()
    {
        uIManager.carrierButtonPressed(lastLinkedCarrier);
        Debug.Log("Click");
    }


}
