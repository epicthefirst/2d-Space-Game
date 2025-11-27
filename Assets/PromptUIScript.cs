using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;

public class PromptUIScript : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button max;
    [SerializeField] UnityEngine.UI.Button min;
    [SerializeField] UnityEngine.UI.Button confirmButton;
    [SerializeField] TMP_InputField input;
    [SerializeField] TMP_Text message;
    [SerializeField] UIManager uIManager;

    private int maxCarriers;
    public bool isActive;
    public ShipController sc;



    private void Start()
    {
        isActive = false;
        max.gameObject.SetActive(isActive);
        min.gameObject.SetActive(isActive);
        input.gameObject.SetActive(isActive);
        confirmButton.gameObject.SetActive(isActive);
        message.gameObject.SetActive(isActive);
    }
    public void init(GameObject carrier)
    {
        isActive = true;
        sc = carrier.GetComponent<ShipController>();

        input.gameObject.SetActive(isActive);
        confirmButton.gameObject.SetActive(isActive);
        message.gameObject.SetActive(isActive);

        input.text = sc.ShipCount.ToString();
        message.text = "Enter the desired amount of ships";


    }
    public void confirmButtonPressed()
    {
        isActive = false;
        input.gameObject.SetActive(isActive);
        confirmButton.gameObject.SetActive(isActive);
        message.gameObject.SetActive(isActive);

        int parsed = int.Parse(input.text);
        Debug.Log(parsed);
        uIManager.WhenInputConfirmed(parsed, sc);


    }
    public void maxButtonPressed()
    {
        if (isActive)
        {
            input.text = (sc.dockedStar.GetComponent<StarScript>().GarrisonCount + sc.ShipCount).ToString();
        }
        else
        {
            Debug.LogError("This shouldn't happen :(");
        }
    }
    public void minButtonPressed()
    {
        if (isActive)
        {
            input.text = 0.ToString();
        }
        else
        {
            Debug.LogError("This shouldn't happen :(");
        }
    }
    public void postMessage(string text)
    {
        message.gameObject.SetActive(true);
        message.text = text;
    }
    public void clearMessage()
    {
        message.gameObject.SetActive(false);
    }
}
