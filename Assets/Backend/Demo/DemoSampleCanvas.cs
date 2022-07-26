using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoSampleCanvas : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    public void SetText(string txt)
    {
        displayText.text = txt;
    }
}
