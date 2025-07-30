using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextObjectResizer : MonoBehaviour
{
    public GameObject TextObject;
    [SerializeField] int extraMarginRight;
    public void write(string word)
    {
        TMP_Text textComponent = TextObject.GetComponent<TMP_Text>();
        // Get the size of the text for the given string.
        Vector2 textSize = TextObject.GetComponent<TMP_Text>().GetPreferredValues(word);
/*        Debug.LogWarning(textSize.x + " : " + textSize.y);*/
        // Set the text
        textComponent.text = word;
        RectTransform textObjTransform = TextObject.GetComponent<RectTransform>();
        textObjTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textSize.x + extraMarginRight);

    }
}
