using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Image playPanel;
    [SerializeField] InputField seedNumberInput;
    [SerializeField] TMP_Dropdown mapSizeDropdown;
    [SerializeField] TMP_Dropdown gameModeDropdown;
    [SerializeField] Button startButton;


    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        playPanel.gameObject.SetActive(false);
    }

    public void PlayButtonPressed()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        playPanel.gameObject.SetActive(true);
    }

    public void StartButtonPressed()
    {
        if (seedNumberInput.text == null)
        {
            int seed = Random.Range(int.MinValue, int.MaxValue);
        }
        switch (mapSizeDropdown.value)
        {
            case 0:

                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
