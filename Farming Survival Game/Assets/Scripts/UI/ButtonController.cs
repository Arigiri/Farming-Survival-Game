using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
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
