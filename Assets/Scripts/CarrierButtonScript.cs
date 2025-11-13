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
    private Texture2D specialistImage;
    private UIManager uIManager;

    public string carrierSpecialist;

    public void init(string name, GameObject linkedCarrier, Color colour, UIManager uIManager, string carrierSpecialist)
    {
        this.carrierSpecialist = carrierSpecialist;
        this.uIManager = uIManager;
        lastLinkedCarrier = linkedCarrier;
        this.GetComponent<Button>().image.color = colour;
        carrierNameText.text = linkedCarrier.GetComponent<ShipController>().Name;
        carrierShipCountText.text = linkedCarrier.GetComponent<ShipController>().ShipCount.ToString();
        specialistImage = uIManager.getSpecImage(carrierSpecialist);
        if (specialistImage != null)
        {
            Debug.Log("Success");
        }
    }


    public void click()
    {
        uIManager.carrierButtonPressed(lastLinkedCarrier);
        Debug.Log("Click");
    }


}
