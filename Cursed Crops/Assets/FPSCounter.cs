using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI FPSText;

    void Update()
    {
        var num = (int)(1f / Time.deltaTime);
        FPSText.text = num.ToString();
    }
}
