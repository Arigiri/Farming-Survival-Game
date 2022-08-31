using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : MonoBehaviour
{
    [SerializeField] private Action m_Action;
    [SerializeField] private float m_MaxLengthInteractive;
    private TileController m_TileController;
    private PlayerController m_Player;
    private ChestUI m_ChestUI;
    public List<Slot> ChestSlots = new List<Slot>();
    public int NumSlots;
    private int x;

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
    public bool Add(CollectableObjectController Object) // kiem tra co add do vao Chest thanh cong khong
    {
        bool Flag = false;
        foreach(Slot slot in ChestSlots)
        {
            if(slot.Type == Object.GetCollectableType())
            {
                if(slot.Count < slot.MaxCount)
                {
                    slot.Count ++;
                    Flag = true;
                    m_ChestUI.Setup();
                    return true;
                    // break;
                }
            }
        }
        if(!Flag)   
        {
            foreach(Slot slot in ChestSlots)
            {
                if(slot.Type == CollectableType.NONE)
                {
                    slot.Add(Object);
                    m_ChestUI.Setup();
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
        ChestSlots[idx].RemoveItem();
        // if(ChestSlots[idx].Count == 0)
        // {
        //     m_ObjectInformationPanel.ResetInformationPanel();
        // }
    }

    public void Swap(int idx1, int idx2) // chuyen tu idx2 sang idx1
    {
        if(idx1 == -1 || idx2 == -1)    return;
        if(GetCollectableType(idx1) != GetCollectableType(idx2))
        {
            Slot tmp = ChestSlots[idx1];
            ChestSlots[idx1] = ChestSlots[idx2];
            ChestSlots[idx2] = tmp;
            m_ChestUI.Setup();
        }
        else 
        {
            int lackCount = ChestSlots[idx1].MaxCount - ChestSlots[idx1].Count;
            int ItemMoveCount = Mathf.Min(lackCount, ChestSlots[idx2].Count);
            ChestSlots[idx1].Count += ItemMoveCount;
            ChestSlots[idx2].Count -= ItemMoveCount;
            if(ChestSlots[idx2].Count == 0)    ChestSlots[idx2].ClearItem();
            m_ChestUI.Setup();
            m_ChestUI.SetupChest();
        }
    }
    public void Init() // Ham nay chay dau tien luc dat ruong xuong dat
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_ChestUI = GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>();
        NumSlots = 12;
        for(int i = 0; i < NumSlots; i++)
        {
            Slot slot = new Slot();
            ChestSlots.Add(slot);
        }
        x = Random.Range(1, 10000);
        print(x);
    }
    public int GetChestNumSlot()
    {
        return ChestSlots.Count;
    }
    public CollectableType GetCollectableType(int idx)
    {
        return ChestSlots[idx].Type;
    }
    public Slot GetSlot(int idx)
    {
        return ChestSlots[idx];
    }
    public void OpenChest() // Mo ruong, ham nay chay moi lan mo ruong
    {
        print(x);
        m_ChestUI.m_ChestController = this;
        m_ChestUI.TurnOnChestUI();
        // m_ChestUI.MakeItemContainer(ItemsContainer);
    }

    public void SelfDestroy() // Pha ruong va Lay ruong vao Inventory
    {
        m_Player.AddItemToInventory(gameObject.GetComponent<CollectableObjectController>());
        // DropItem();
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    // private void DropItem()
    // {
    //     Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(transform.position, 0.35f, 0.5f);
    //     foreach(CollectableObjectController item in ItemsContainer)
    //     {
    //         item.ResetAttribute(true);
    //         Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
    //         m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, SpawnPoint);
    //     }
    // }
    public Slot ConvertFromInventorySlotToChestSlot(InventoryController.Slot slot)
    {
        Slot tmp = new Slot();
        tmp.Count = slot.Count;
        tmp.MaxCount = slot.MaxCount;
        tmp.Type = slot.Type;
        tmp.m_Icon = slot.m_Icon;
        tmp.m_Action = slot.m_Action;
        tmp.m_Durability = slot.m_Durability;
        tmp.m_MaxDurability = slot.m_MaxDurability;
        tmp.m_TreeType = slot.m_TreeType;
        tmp.m_CollectableObject = slot.m_CollectableObject;
        return tmp;
    }
    public InventoryController.Slot ConvertFromChestSlotToInventorySlot(Slot slot)
    {
        InventoryController.Slot tmp = new InventoryController.Slot();
        tmp.Count = slot.Count;
        tmp.MaxCount = slot.MaxCount;
        tmp.Type = slot.Type;
        tmp.m_Icon = slot.m_Icon;
        tmp.m_Action = slot.m_Action;
        tmp.m_Durability = slot.m_Durability;
        tmp.m_MaxDurability = slot.m_MaxDurability;
        tmp.m_TreeType = slot.m_TreeType;
        tmp.m_CollectableObject = slot.m_CollectableObject;
        return tmp;
    }
    public void MoveFromInventoryToChest(int InventoryIdx, int ChestIdx)
    {
        if(InventoryIdx == -1 || ChestIdx == -1)    return;
        if(m_Player.GetCollectableType(InventoryIdx) != GetCollectableType(ChestIdx))
        {
            Slot tmp = ConvertFromInventorySlotToChestSlot(m_Player.GetInventoryController().Slots[InventoryIdx]);
            m_Player.GetInventoryController().Slots[InventoryIdx] = ConvertFromChestSlotToInventorySlot(ChestSlots[ChestIdx]);
            ChestSlots[ChestIdx] = tmp;
            m_ChestUI.Setup();
        }
        else // cung mot do, uu tien chest
        {
            int lackCount = ChestSlots[ChestIdx].MaxCount - ChestSlots[ChestIdx].Count;
            int ItemMoveCount = Mathf.Min(lackCount, m_Player.GetInventoryController().Slots[InventoryIdx].Count);
            ChestSlots[ChestIdx].Count += ItemMoveCount;
            m_Player.GetInventoryController().Slots[InventoryIdx].Count -= ItemMoveCount;
            if(m_Player.GetInventoryController().Slots[InventoryIdx].Count == 0)    m_Player.GetInventoryController().Slots[InventoryIdx].ClearItem();
            m_ChestUI.Setup();
            m_ChestUI.SetupChest();
        }
        
    }
}
