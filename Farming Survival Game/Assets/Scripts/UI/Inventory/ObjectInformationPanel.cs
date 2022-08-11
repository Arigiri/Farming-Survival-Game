using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInformationPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject m_InformationPanel;
    [SerializeField] private TextMeshProUGUI m_Description;
    [SerializeField] private TextMeshProUGUI m_ItemName;
    [SerializeField] private Image m_Icon;
    [SerializeField] private InventoryController m_Inventory;
    [SerializeField] private TextMeshProUGUI m_FoodText;
    [SerializeField] private TextMeshProUGUI m_StaminaText;
    [SerializeField] private GameObject m_Food;
    [SerializeField] private GameObject m_Stamina;

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
        m_Description.text = m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.Description;
        m_ItemName.text = m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.ItemName;
        if(m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.ItemFood == -1)   
        {
            m_Food.SetActive(false);
        }
        else 
        {
            m_Food.SetActive(true);
            m_FoodText.text = " : " + m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.ItemFood.ToString();
        }
        if(m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.ItemStamina == -1) 
        {
            m_Stamina.SetActive(false);
        }
        else 
        {
            m_Stamina.SetActive(true);
            m_StaminaText.text = " : " + m_Inventory.Slots[m_SlotIdx].m_CollectableObject.m_Information.ItemStamina.ToString();
        }
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
        m_Description.text = "";
        m_ItemName.text = "";
        m_Food.SetActive(false);
        m_Stamina.SetActive(false);
    }
}
