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

    public void Swap(int idx1, int idx2)
    {
        if(idx1 == -1 || idx2 == -1)    return;
        Slot tmp = ChestSlots[idx1];
        ChestSlots[idx1] = ChestSlots[idx2];
        ChestSlots[idx2] = tmp;
        m_ChestUI.Setup();
    }
    public void Init() // Ham nay chay dau tien 
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_ChestUI = GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>();
        NumSlots = 12;
        for(int i = 0; i < NumSlots; i++)
        {
            Slot slot = new Slot();
            ChestSlots.Add(slot);
        }
        m_ChestUI.m_ChestController = this;
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
    public void OpenChest() // Mo ruong
    {
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
}
