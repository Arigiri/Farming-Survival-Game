using System.Diagnostics.SymbolStore;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject m_ProcessingBar;
    [SerializeField] private PlayerActionController m_PlayerAction;
    private PlayerController m_Player;
    private Slider m_Slider;
    private float m_ProcessingTime = 0f;
    private float m_CurrProcessingTime = -1;
    private Action m_Action;
    void Start()
    {
        m_Slider = m_ProcessingBar.GetComponentInChildren<Slider>();
        m_Player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_CurrProcessingTime >= 0)
        {
            m_CurrProcessingTime += Time.deltaTime;
            m_Slider.value = Mathf.Round(m_CurrProcessingTime / m_ProcessingTime * 100f);
        }
        if(m_CurrProcessingTime >= m_ProcessingTime)
        {
            m_PlayerAction.TriggerAction(m_Action);
            TurnOffProgressBar();
            m_Player.IsWorking = false;
        }
    }

    // [ContextMenu("MakeProgress")]
    public void MakeProgressBar(float ProcessTime)
    {
       m_ProcessingTime = ProcessTime;
       m_CurrProcessingTime = 0;
       m_ProcessingBar.SetActive(true);
    }

    public void TurnOffProgressBar()
    {
        m_ProcessingBar.SetActive(false);
        m_CurrProcessingTime = -1;
    }

    public void SetAction(Action action)
    {
        m_Action = action;
    }
}
