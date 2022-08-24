using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ToolBarController : MonoBehaviour
{
    private InventoryController m_Inventory;
    [SerializeField] private Inventory_UI m_InventoryUI;
    [SerializeField] private PlayerController m_Player;
    private AttributeUIController m_AttributeUIController;
    private List<InventoryController.Slot> m_Slots = new List<InventoryController.Slot>();
    public List<Slot_UI> m_SlotUI;
    public int m_MaxToolSlot;
    public int ActiveSlot;
    private int OldActiveSlot;
    private KeyCode[] NumberArr = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};

    void Start()
    {
        m_AttributeUIController = FindObjectOfType<AttributeUIController>();
        m_Inventory = m_Player.GetInventoryController();
        for(int i = 0; i < m_MaxToolSlot; i++)
        {
            m_Slots.Add(m_Inventory.Slots[i]);
            m_SlotUI[i].SlotIdx = i;
        }
        m_SlotUI[0].IsActive = true;
       
        
    }
    public int GetActiveSlot()
    {
        return OldActiveSlot;
    }

    public void Setup()
    {
        for(int i = 0; i < m_MaxToolSlot; i++)
        {
            m_Slots[i] = m_Inventory.Slots[i];
            m_Inventory.Slots[i].m_Durability = m_Slots[i].m_Durability;
            if(m_Player.GetInventoryController().Slots.Count > 0 && m_Player.GetInventoryController().Slots[i].Type == CollectableType.NONE)
                m_Slots[i].ClearItem();
            m_SlotUI[i].SetItem(m_Slots[i]);
            if(m_Slots[i].Type == CollectableType.NONE) m_SlotUI[i].GetImage().color = new Color(1, 1, 1, 0);
            if(m_Slots[i].Count == 0 || m_Slots[i].m_Durability > 0)m_SlotUI[i].GetText().text = "";
            m_SlotUI[i].Setup(m_Slots[i]);
        }
    }
    void Update()
    {
        Setup();
        if(ChangeToolBar())
            m_AttributeUIController.TurnOffProgressBar();
            
        
    }
    private bool ChangeToolBar()
    {
        bool result = false;
        Vector2 Scroll = Input.mouseScrollDelta;
        if(Scroll != Vector2.zero)
        {
            for(int i = 0; i < m_MaxToolSlot; i++)
            {
                if(m_SlotUI[i].IsActive)
                {
                    ActiveSlot = Math.Min(-(int)Scroll.y + i, m_MaxToolSlot - 1);
                    ActiveSlot = Math.Max(ActiveSlot, 0);
                    m_SlotUI[ActiveSlot].ShowTarget();
                    result = true;
                    break;
                }
            }
        }

        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Q))
        {
            for(int i = 0; i < NumberArr.Length; i++)
                if(Input.GetKeyDown(NumberArr[i]))
                {
                    ActiveSlot = i;
                    m_SlotUI[ActiveSlot].ShowTarget();
                    result = true;
                    break;
                }
        }

        for(int i = 0; i < m_MaxToolSlot; i++)
        {
            if(m_SlotUI[i].IsActive)
            {   
                ActiveSlot = Math.Min(-(int)Scroll.y + i, m_MaxToolSlot - 1);
                ActiveSlot = Math.Max(ActiveSlot, 0);
                m_Player.SetItemOnHand(m_Slots[i]);
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CollectableObjectController itemToDrop = ItemGameManager.instance.itemManager.GetItemByType(m_Slots[ActiveSlot].Type);
            if(itemToDrop != null)
            {
                itemToDrop.SetDurability(m_Slots[ActiveSlot].m_Durability);
                m_Player.DropItemAhead(itemToDrop);
                m_Player.GetInventoryController().Remove(ActiveSlot);
                m_InventoryUI.Setup(true);
            }
        }
        // print(ActiveSlot);
        OldActiveSlot = ActiveSlot;
        return result;
    }
    public void SetActiveSlot()
    {

    }
    public void TriggerOff()
    {
        foreach(Slot_UI slot in m_SlotUI)
        {
            slot.IsActive = false;
            slot.transform.GetChild(3).gameObject.SetActive(false);
        }
    }
}
