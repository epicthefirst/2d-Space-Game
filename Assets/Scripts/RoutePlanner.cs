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

    private List<GameObject> carrierList;
    private List<GameObject> preFabList = new List<GameObject>();
    private Vector2 originalSizeDelta;
    // Start is called before the first frame update
    void Start()
    {
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
    }
    public void init(List<GameObject> carrierList)
    {
        Vector2 pos = originalSizeDelta;
        this.carrierList = carrierList;
        for (int i = 0; i < carrierList.Count; i++)
        {
            //, new Vector3(0, -80 - (i * 40)), Quaternion.identity
            GameObject obj = Instantiate(preFab, gameObject.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -90 - (i * 40));
            pos += new Vector2(0, 40);
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

    }
}
