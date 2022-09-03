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
    [SerializeField] private Inventory_UI m_InventoryUI;
    [SerializeField] private Image m_OtherCloneSlot;

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
        if(m_ChestUI.GetCurrDurability(thisSlotIdx) <= 0)
        {
            m_Durability.gameObject.SetActive(false);
            if(m_ChestUI.m_ChestController.ChestSlots[thisSlotIdx].Count > 0)    m_QuantityText.text = m_ChestUI.m_ChestController.ChestSlots[thisSlotIdx].Count.ToString();
            else m_QuantityText.text = "";
        }
        else
        {
            m_Durability.gameObject.SetActive(true);
            try{
                m_Durability.value = (float)m_ChestUI.m_ChestController.ChestSlots[thisSlotIdx].m_Durability / (float)m_ChestUI.m_ChestController.ChestSlots[thisSlotIdx].m_MaxDurability * 100f;
            }
            catch
            {
                // print(m_Durability);
                // print(m_Slot);
            }
            m_QuantityText.text = "";
        }
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
        if(eventData.pointerDrag != null)
        {
            // Debug.Log(eventData.pointerDrag);
            // int idx = m_ObjectInformationPanel.m_SlotIdx;
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = m_FirstSlotPosition.position + new Vector3(135 * (idx % 9), -(135 * (idx / 9)), 0);
            // m_ItemIcon.color = new Color(0.4f, 0.65f, 0.23f, 1);
            // Debug.Log(SlotIdx);
            var thisCloneSlot = GameObject.FindGameObjectWithTag("DragDrop");
            thisCloneSlot.GetComponent<DragDrop>().CheckDrop = true;
            // print(m_InventoryUI.slots[m_ObjectInformationPanel.m_SlotIdx].GetCurrDurability());
            if(thisCloneSlot.GetComponent<DragDrop>().GetInventory_UI() != null) // cloneSlot duoc keo tu Inventory panel
            {
                if(m_InventoryUI.slots[thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()].GetCurrDurability() <= 0) // Vi tri cu cua cai cloneslot
                {
                    m_CloneDurability.gameObject.SetActive(false);
                    if(m_InventoryUI.GetInventorySlotCount(thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()) > 0)    
                        m_CloneQuantityText.text = m_InventoryUI.GetInventorySlotCount(thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()).ToString();
                }
                else
                {
                    m_CloneDurability.gameObject.SetActive(true);
                    m_CloneQuantityText.text = "";
                }
                m_ChestUI.m_ChestController.MoveFromInventoryToChest(thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex(), thisSlotIdx);
                m_ChestUI.SetupChest();
                m_ChestUI.Setup();
            }
            else // Duoc keo noi bo trong ChestPanel
            {
                if(m_ChestUI.GetCurrDurability(thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()) <= 0) // Vi tri cu cua cai cloneslot
                {
                    m_CloneDurability.gameObject.SetActive(false);
                    if(m_ChestUI.m_ChestController.ChestSlots[thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()].Count > 0)    
                        m_CloneQuantityText.text = m_ChestUI.m_ChestController.ChestSlots[thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()].Count.ToString();
                }
                else
                {
                    m_CloneDurability.gameObject.SetActive(true);
                    m_CloneQuantityText.text = "";
                }
                m_ChestUI.m_ChestController.Swap(thisSlotIdx, thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex()); //chuyen tu vi tri idx2 sang vi tri idx1
                m_ChestUI.SetupChest();
            }
        }

        // DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
        // obj.SetPosition(SlotIdx);
        // obj.CheckDrop = true;
        // m_CloneSlot.gameObject.SetActive(false);
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
        m_OtherCloneSlot.gameObject.SetActive(false);
        m_InventoryUI.Setup(false);
    }
    public void OnClick1()
    {
        if(m_ItemIcon.sprite != null)
        {
            m_CloneSlot.gameObject.SetActive(true);
            DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
            obj.SetPositionOnChestUI(thisSlotIdx);
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
        if(m_CloneSlot != null && m_CloneSlot.gameObject.activeSelf == true && m_ChestUI.m_ChestController.ChestSlots[thisSlotIdx].Count == 0)
        {
            m_CloneSlot.GetComponent<DragDrop>().SetActiveFalse();
        }
    }
    public string GetQuantityText()
    {
        return m_QuantityText.text;
    }
}
