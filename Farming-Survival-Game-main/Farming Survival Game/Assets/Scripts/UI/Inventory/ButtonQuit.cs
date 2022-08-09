using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuit : MonoBehaviour
{
    [SerializeField] GameObject m_InventoryPanel;
    private void ClickButtonQuit()
    {
        if(m_InventoryPanel.activeSelf == false)    m_InventoryPanel.SetActive(true);
    }
}
