using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot_UI : MonoBehaviour, IDropHandler
{
    [SerializeField] ToolBarController m_ToolBar;
    [SerializeField] private int Num;
    public bool IsActive;
    [SerializeField] private Image m_ItemIcon;
    [SerializeField] private TextMeshProUGUI m_QuantityText;
    [SerializeField] private Inventory_UI m_InventoryUI;
    [SerializeField] private Color m_Color;
    [SerializeField] private Image m_CloneSlot;
    [SerializeField] private ObjectInformationPanel m_ObjectInformationPanel;
    [SerializeField] private Transform m_FirstSlotPosition;
    [SerializeField] private PlayerController m_Player;
    [SerializeField] private Slider m_Durability;
    [SerializeField] private Slider m_CloneDurability;
    [SerializeField] private TextMeshProUGUI m_CloneQuantityText;
    [SerializeField] private InventoryController m_Inventory;
    [SerializeField] private Image m_OtherCloneSlot;
    // [SerializeField] private ChestUI m_ChestUI;
    // [SerializeField] private 
    private InventoryController.Slot m_Slot;

    public Image thisImage; //Day la anh cua Slot
    public int SlotIdx;
    private int m_CurrDurability = -1;
    private void Awake() 
    {
        thisImage = gameObject.GetComponent<Image>();
    }
    public string GetQuantityText()
    {
        return m_QuantityText.text;
    }
    public void SetDurability(int Durability)
    {
        m_CurrDurability = Durability;
    }

    public int GetCurrDurability()
    {
        return m_CurrDurability;
    }
    public void Setup(InventoryController.Slot slot)
    {
        m_Slot = slot;
        SetDurability(slot.m_Durability);
        if(slot.m_CollectableObject != null)slot.m_CollectableObject.SetDurability(slot.m_Durability);
        if(m_CurrDurability <= 0)
        {
            m_Durability.gameObject.SetActive(false);
            if(m_Inventory.Slots[SlotIdx].Count > 0)    m_QuantityText.text = m_Inventory.Slots[SlotIdx].Count.ToString();
            else m_QuantityText.text = "";
        }
        else
        {
            m_Durability.gameObject.SetActive(true);
            m_Durability.value = (float)slot.m_Durability / (float)slot.m_MaxDurability * 100f;
            m_QuantityText.text = "";
        }
    }

    private void Start()
    {
        m_Durability.gameObject.SetActive(false);
        thisImage = gameObject.GetComponent<Image>();
    }
    private void Update()
    {
        // return;
        if(m_CloneSlot != null && m_CloneSlot.gameObject.activeSelf == true && m_ObjectInformationPanel.m_SlotIdx != -1 && m_Player.GetCollectableCount(m_ObjectInformationPanel.m_SlotIdx) == 0)
        {
            m_CloneSlot.GetComponent<DragDrop>().SetActiveFalse();
        }
        if(m_CurrDurability <= 0)
        {
            m_Durability.gameObject.SetActive(false);
            if(m_Inventory.Slots[SlotIdx].Count > 0)    m_QuantityText.text = m_Inventory.Slots[SlotIdx].Count.ToString();
            else m_QuantityText.text = "";
        }
        else
        {
            m_Durability.gameObject.SetActive(true);
            try{
                m_Durability.value = (float)m_Slot.m_Durability / (float)m_Slot.m_MaxDurability * 100f;
            }
            catch
            {
                // print(m_Durability);
                // print(m_Slot);
            }
            m_QuantityText.text = "";
        }
    }
    public void SetItem(InventoryController.Slot slot)
    {
        if(slot != null)
        {
            m_ItemIcon.sprite = slot.m_Icon;
            m_ItemIcon.color = new Color(1, 1, 1, 1);
            if(slot.Count > 0)  m_QuantityText.text = slot.Count.ToString();
            else m_QuantityText.text = "";
            m_Slot = slot;
            // if(slot.m_Durability != -1)print(slot.m_Durability);
        }
        // else SetEmpty();
    }

    public void SetEmpty()
    {
        m_ItemIcon.sprite = null;
        m_ItemIcon.color = new Color(1, 1, 1, 0);
        m_QuantityText.text = "";
        m_Slot = null;
    }

    public void OnClick()
    {
        if(m_InventoryUI == null)
        {
            ShowTarget();
            return;
        }
        foreach(Slot_UI slot in m_InventoryUI.slots)
        {
            Image img = slot.gameObject.GetComponent<Image>();
            img.color = m_Color;
        }
        Image image = gameObject.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0.75f); // neu sua cai nay thi sua ca cai trong dragdrop
        if(m_OtherCloneSlot != null)    
        {
            m_OtherCloneSlot.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>().SetupChest();
        }

    }
    public Color GetDefaultColor()
    {
        return m_Color;
    }
    public Image GetImage()
    {
        return m_ItemIcon;
    }

    public TextMeshProUGUI GetText()
    {
        return m_QuantityText;
    }

    public void ShowTarget()
    {
        m_ToolBar.TriggerOff();
        transform.GetChild(3).gameObject.SetActive(true);
        IsActive = true;
    }

    public void OnClick1()
    {
        if(m_ObjectInformationPanel.m_SlotIdx == -1)    return;
        if(m_ItemIcon.sprite != null)
        {
            m_CloneSlot.gameObject.SetActive(true);
            DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
            obj.SetPosition(m_ObjectInformationPanel.m_SlotIdx);
            m_CloneSlot.sprite = m_ItemIcon.sprite;
            m_CloneSlot.color = m_ItemIcon.color;
            m_CloneDurability.gameObject.SetActive(true);
            m_CloneDurability.value = m_Durability.value;
            if(m_CurrDurability <= 0)   
            {
                m_CloneDurability.gameObject.SetActive(false);
                
            }
            m_CloneQuantityText.text = m_QuantityText.text;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(m_ObjectInformationPanel == null)    return;
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
            if(thisCloneSlot.GetComponent<DragDrop>().GetInventory_UI() != null) // keo noi bo trong inventory panel
            {
                if(m_ObjectInformationPanel.m_SlotIdx == -1)    return;
                if(m_InventoryUI.slots[m_ObjectInformationPanel.m_SlotIdx].GetCurrDurability() <= 0)
                {
                    m_CloneDurability.gameObject.SetActive(false);
                    if(m_Inventory.Slots[m_ObjectInformationPanel.m_SlotIdx].Count > 0)    m_CloneQuantityText.text = m_Inventory.Slots[m_ObjectInformationPanel.m_SlotIdx].Count.ToString();
                }
                else
                {
                    m_CloneDurability.gameObject.SetActive(true);
                    m_CloneQuantityText.text = "";
                }
                m_Player.InventorySwap(SlotIdx, m_ObjectInformationPanel.m_SlotIdx);
                m_InventoryUI.Setup(false);
            } 
            else // keo tu Chest panel sang inventory panel
            {
                ChestUI m_ChestUI = GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>();
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
                m_Inventory.MoveFromChestToInventory(thisCloneSlot.GetComponent<DragDrop>().GetCurrSlotIndex(), SlotIdx);
                m_ChestUI.SetupChest();
                m_ChestUI.Setup();
            }
            
        }
        // DragDrop obj = m_CloneSlot.GetComponent<DragDrop>();
        // obj.SetPosition(SlotIdx);
        // obj.CheckDrop = true;
        // m_CloneSlot.gameObject.SetActive(false);
    }
}
