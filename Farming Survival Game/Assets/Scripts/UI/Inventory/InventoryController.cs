using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // [SerializeField] private int m_MaxStack;
    [SerializeField] private int m_MaxStack;
    [SerializeField] private ObjectInformationPanel m_ObjectInformationPanel;
    [SerializeField] private Inventory_UI m_InventoryUI;
    [SerializeField] private TransparentObject m_TransparentObject;
    public List<Slot> Slots = new List<Slot>();
    public int NumSlots;

    private void Update()
    {
        if(m_InventoryUI.gameObject.activeSelf == true)
        {
            if(m_TransparentObject.gameObject.activeSelf == false)
            {
                m_TransparentObject.gameObject.SetActive(true);
            }
        }

        if(m_InventoryUI.gameObject.activeSelf == false)
        {
            if(m_TransparentObject.gameObject.activeSelf == true)
            {
                m_TransparentObject.gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < NumSlots; i++)
        {
            var slot = Slots[i];
            if(slot.m_CollectableObject != null)
                slot.m_Durability = slot.m_CollectableObject.GetCurrDurability();
        }
    }

    public class Slot
    {
        public int Count;
        public int MaxCount;
        public CollectableType Type;
        public Sprite m_Icon;
        public Action m_Action;
        public int m_Durability;
        public int m_MaxDurability;
        public TreeType m_TreeType;
        public CollectableObjectController m_CollectableObject;
        public Slot()
        {
            Count = 0;
            MaxCount = 0;
            Type = CollectableType.NONE;
            m_Action = Action.None;
            m_Durability = -1;
            m_MaxDurability = -1;
        }

        public void Add(CollectableObjectController Object)
        {
            Type = Object.GetCollectableType();
            if(Type == CollectableType.SeedBag)
            {
                m_TreeType = Object.GetTreeType();
            }
            m_Icon = Object.Icon;
            MaxCount = Object.GetStack();
            m_Action = Object.GetAction();
            if(Object.GetCurrDurability() > 0) m_Durability = Object.GetCurrDurability();
            else m_Durability = Object.m_Information.StartDurability;
            m_MaxDurability = Object.m_Information.ToolDurability;
            m_CollectableObject = Object;
            Count++;
        }
        
        public void RemoveItem()
        {
            if(Count > 0)
            {
                Count--;
                if(Count == 0)ClearItem();
            }
        }

        public void ClearItem()
        {
            m_Icon = null;
            Type = CollectableType.NONE;
            m_Durability = -1;
            m_MaxDurability = -1;
            m_CollectableObject = null;
        }
    }

  
    
    void Awake()
    {
        // print(NumSlots);
        for(int i = 0; i < NumSlots; i++)
        {
            Slot slot = new Slot();
            Slots.Add(slot);
        }
    }

    public bool Add(CollectableObjectController Object)
    {
        bool Flag = false;
        foreach(Slot slot in Slots)
        {
            if(slot.Type == Object.GetCollectableType())
            {
                if(slot.Count < slot.MaxCount)
                {
                    slot.Count ++;
                    Flag = true;
                    m_InventoryUI.Setup(false);
                    return true;
                    // break;
                }
            }
        }
        if(!Flag)   
        {
            foreach(Slot slot in Slots)
            {
                if(slot.Type == CollectableType.NONE)
                {
                    slot.Add(Object);
                    m_InventoryUI.Setup(false);
                    return true;
                    // break;
                }
            }
        }
        // m_InventoryUI.Setup(false);
        return false;
    }

    public void Remove(int idx)
    {
        Slots[idx].RemoveItem();
        if(Slots[idx].Count == 0)
        {
            m_ObjectInformationPanel.ResetInformationPanel();
        }
    }

    public void Swap(int idx1, int idx2)
    {
        if(idx1 == -1 || idx2 == -1)    return;
        Slot tmp = Slots[idx1];
        Slots[idx1] = Slots[idx2];
        Slots[idx2] = tmp;
        m_InventoryUI.Setup(false);
    }
}
