using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Image playPanel;
    [SerializeField] TMP_InputField seedNumberInput;
    [SerializeField] TMP_Dropdown mapSizeDropdown;
    [SerializeField] TMP_Dropdown gameModeDropdown;
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField cycleLengthInput;

    [SerializeField] GameObject shipPrefab;

    private int seedInput = 42;


    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        playPanel.gameObject.SetActive(false);

        //cycleLengthInput.onValidateInput += delegate (string input, int charIndex, char addedChar) { return PosIntValidate(addedChar); };
    }
    //public char PosIntValidate(char charToValidate)
    //{

    //    if (charToValidate != '1'
    //    && charToValidate != '2'
    //    && charToValidate != '3'
    //    && charToValidate != '4'
    //    && charToValidate != '5'
    //    && charToValidate != '6'
    //    && charToValidate != '7'
    //    && charToValidate != '8'
    //    && charToValidate != '9'
    //    && charToValidate != '0')
    //    {
    //        //return a null character
    //        return '\0';
    //    }
    //    return charToValidate;
    //}

    public void PlayButtonPressed()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        playPanel.gameObject.SetActive(true);
    }

    public void StartButtonPressed()
    {
        if (!int.TryParse(seedNumberInput.text, out seedInput))
        {
            seedInput = Random.Range(int.MinValue, int.MaxValue);
        }
            int numberOfCircles = 0;
        switch (mapSizeDropdown.value)
        {
            case 0:
                //Tiny
                numberOfCircles = 4;
                break;
            case 1:
                //Small
                numberOfCircles = 8;
                break;
            case 2:
                //Medium
                numberOfCircles = 12;
                break;
            case 3:
                //Large
                numberOfCircles = 16;
                break;
        }
        int cycleLength = 12;
        if (!int.TryParse(cycleLengthInput.text, out cycleLength))
        {
            Debug.LogError("Tried parsing, and failed");
        }
        switch (gameModeDropdown.value)
        {
            case 0:
                //Normal


                Debug.LogWarning("Loading Normal Game");
                GameInformation.init(500, cycleLength, numberOfCircles, shipPrefab, "Normal", 1, seedInput);
                SceneManager.LoadScene("MainScene");
                break;
            case 1:
                //Simulation

                Debug.LogWarning("Loading Simulation");
                GameInformation.init(500, cycleLength, numberOfCircles, shipPrefab, "Simulation", 2, seedInput);
                SceneManager.LoadScene("MainScene");
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
