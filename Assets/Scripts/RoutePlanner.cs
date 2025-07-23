using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoutePlanner : MonoBehaviour
{
    [SerializeField] Button undoButton;
    [SerializeField] Button undoAllButton;
    [SerializeField] Toggle loopToggle;
    [SerializeField] Button saveButton;
    [SerializeField] Button saveAndEditButton;
    [SerializeField] TMP_Text carrierNameText;
    [SerializeField] TMP_Text shipCountText;
    [SerializeField] TMP_Text ETAText;
    [SerializeField] GameObject preFab;
    [SerializeField] UIManager uIManager;

    public bool isActive;

    public GameObject currentCarrier;
    private List<GameObject> starList;
    private List<GameObject> preFabList = new List<GameObject>();
    private Vector2 originalSizeDelta;
    // Start is called before the first frame update
    void Start()
    {
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.SetActive(false);
    }
    public void init(GameObject carrier)
    {
        gameObject.SetActive(true);
        isActive = true;
        currentCarrier = carrier;

        updateUI(carrier.GetComponent<ShipController>().starWaypoints);
        uIManager.isRoutePlannerActive = true;
    }

    public void updateUI(List<GameObject> starList)
    {
        Vector2 pos = originalSizeDelta;
        this.starList = starList;
        for (int i = 0; i < starList.Count; i++)
        {
            //, new Vector3(0, -80 - (i * 40)), Quaternion.identity
            GameObject obj = Instantiate(preFab, gameObject.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -90 - (i * 40));
            pos += new Vector2(0, 40);
            obj.GetComponent<TextObjectResizer>().write(starList[i].GetComponent<StarScript>().Name);
            preFabList.Add(obj);


        }
        gameObject.GetComponent<RectTransform>().sizeDelta = pos;
    }
    public void clear()
    {
        foreach(GameObject p in preFabList)
        {
            Destroy(p);
        }
        uIManager.isRoutePlannerActive = false;
        gameObject.SetActive(false);
        

    }
}
