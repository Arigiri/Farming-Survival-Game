using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInformationPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject m_InformationPanel;
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Image m_Icon;
    [SerializeField] private InventoryController m_Inventory;

    public int m_SlotIdx; // Chi so cua o hien tai cua ObjectInformationPanel

    public void RefreshInformationPanel(Image Icon)
    {
        if(Icon.sprite == null)
        {
            m_Icon.color = new Color(1, 1, 1, 0);
            return;
        }
        m_Icon.sprite = Icon.sprite;
        m_Icon.color = new Color(1, 1, 1, 1);
        m_text.text = m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.Description;
        // Debug.Log(Icon.sprite);
    }

    public void GetSlotIdx(int SlotIdx)
    {
        m_SlotIdx = SlotIdx;
    }

    public void ResetInformationPanel()
    {
        m_Icon.sprite = null;
        m_Icon.color = new Color(1, 1, 1, 0);
    }
}
