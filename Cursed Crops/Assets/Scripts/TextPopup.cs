using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Color textColor;

    private const float DisappearMax = 1f;
    private float disappearTimer;
    private float disappearSpeed = 3f;
    private float scaleUpAmmount = 0.75f;
    private float scaleDownAmmount = 0.5f;
    private float moveYSpeed = 4f;

    private void Awake()
    {
        textMesh = this.GetComponent<TextMeshPro>();
    }

    public void Setup(string text, Color color)
    {
        textMesh.SetText(text);
        textMesh.color = textColor = color;
        disappearTimer = DisappearMax;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if(disappearTimer > DisappearMax * 0.75f)
        {
            transform.localScale += Vector3.one * scaleUpAmmount * Time.deltaTime;
        } else
        {
            transform.localScale -= Vector3.one * scaleDownAmmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
