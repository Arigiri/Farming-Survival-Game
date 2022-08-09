using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public Color DefaultColor;
    void Start()
    {
        DefaultColor = gameObject.GetComponentInChildren<TextMeshProUGUI>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextColor(Color color)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;
    }
}
