using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameManager.GameState ScenceChangeTo;
    [SerializeField] private GameManager m_GameManager;
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

    public void Trigger()
    {
        print(ScenceChangeTo);
        m_GameManager.SetState(ScenceChangeTo);
    }
}
