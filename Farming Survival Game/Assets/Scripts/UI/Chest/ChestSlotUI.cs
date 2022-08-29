using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChestSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image m_ItemIcon;
    [SerializeField] private TextMeshProUGUI m_QuantityText;
    [SerializeField] private Slider m_Durability;
    [SerializeField] private Image m_CloneSlot;
    [SerializeField] private Color m_Color;
    [SerializeField] private Transform m_FirstSlotPosition;
    [SerializeField] private Slider m_CloneDurability;
    [SerializeField] private TextMeshProUGUI m_CloneQuantityText;
    [SerializeField] private ChestUI m_ChestUI;

    public Image thisImage; //Day la anh cua Slot
    public int thisSlotIdx;
    // Start is called before the first frame update
    void Awake()
    {
        m_Durability.gameObject.SetActive(false);
        thisImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(m_CloneSlot != null && m_CloneSlot.gameObject.activeSelf == true && m_ObjectInformationPanel.m_SlotIdx != -1 && m_Player.GetCollectableCount(m_ObjectInformationPanel.m_SlotIdx) == 0)
        // {
        //     m_CloneSlot.GetComponent<DragDrop>().SetActiveFalse();
        // }
        // if(m_CurrDurability <= 0)
        // {
        //     m_Durability.gameObject.SetActive(false);
        //     if(m_Inventory.Slots[SlotIdx].Count > 0)    m_QuantityText.text = m_Inventory.Slots[SlotIdx].Count.ToString();
        //     else m_QuantityText.text = "";
        // }
        // else
        // {
        //     m_Durability.gameObject.SetActive(true);
        //     try{
        //         m_Durability.value = (float)m_Slot.m_Durability / (float)m_Slot.m_MaxDurability * 100f;
        //     }
        //     catch
        //     {
        //         // print(m_Durability);
        //         // print(m_Slot);
        //     }
        //     m_QuantityText.text = "";
        // }
    }

    public void SetItem(ChestController.Slot slot)
    {
        if(slot != null)
        {
            m_ItemIcon.sprite = slot.m_Icon;
            m_ItemIcon.color = new Color(1, 1, 1, 1);
            if(slot.Count > 0)  m_QuantityText.text = slot.Count.ToString();
            else m_QuantityText.text = "";
            // m_Slot = slot;
            // if(slot.m_Durability != -1)print(slot.m_Durability);
        }
        // else SetEmpty();
    }

    public void SetEmpty()
    {
        m_ItemIcon.sprite = null;
        m_ItemIcon.color = new Color(1, 1, 1, 0);
        m_QuantityText.text = "";
        // m_Slot = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop");
        // if(eventData.pointerDrag != null)
        // {
        //     // Debug.Log(eventData.pointerDrag);
        //     // int idx = m_ObjectInformationPanel.m_SlotIdx;
        //     // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = m_FirstSlotPosition.position + new Vector3(135 * (idx % 9), -(135 * (idx / 9)), 0);
        //     // m_ItemIcon.color = new Color(0.4f, 0.65f, 0.23f, 1);
        //     // Debug.Log(SlotIdx);
            
        //     m_CloneSlot.GetComponent<DragDrop>().CheckDrop = true;
        //     // print(m_InventoryUI.slots[m_ObjectInformationPanel.m_SlotIdx].GetCurrDurability());
        //     if(m_InventoryUI.slots[m_ObjectInformationPanel.m_SlotIdx].GetCurrDurability() <= 0)
        //     {
        //         m_CloneDurability.gameObject.SetActive(false);
        //         if(m_Inventory.Slots[m_ObjectInformationPanel.m_SlotIdx].Count > 0)    m_CloneQuantityText.text = m_Inventory.Slots[m_ObjectInformationPanel.m_SlotIdx].Count.ToString();
        //     }
        //     else
        //     {
        //         m_CloneDurability.gameObject.SetActive(true);
        //         m_CloneQuantityText.text = "";
        //     }
        //     m_Player.InventorySwap(SlotIdx, m_ObjectInformationPanel.m_SlotIdx);
        //     m_InventoryUI.Setup(false);
        // }

        // // DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
        // // obj.SetPosition(SlotIdx);
        // // obj.CheckDrop = true;
        // // m_CloneSlot.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        foreach(ChestSlotUI slot in m_ChestUI.ChestSlots)
        {
            Image img = slot.gameObject.GetComponent<Image>();
            img.color = m_Color;
        }
        Image image = gameObject.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0.75f); // neu sua cai nay thi sua ca cai trong dragdrop
    }
    public void OnClick1()
    {
        if(m_ItemIcon.sprite != null)
        {
            m_CloneSlot.gameObject.SetActive(true);
            DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
            obj.SetPosition(m_ChestUI.CurrSlotIdx);
            m_CloneSlot.sprite = m_ItemIcon.sprite;
            m_CloneSlot.color = m_ItemIcon.color;
            m_CloneDurability.gameObject.SetActive(true);
            m_CloneDurability.value = m_Durability.value;
            if(m_ChestUI.GetCurrDurability(thisSlotIdx) <= 0)   
            {
                m_CloneDurability.gameObject.SetActive(false);
                
            }
            m_CloneQuantityText.text = m_QuantityText.text;
        }
    }
}
