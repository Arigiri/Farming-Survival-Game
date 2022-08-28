using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private PlayerController m_Player;
    [SerializeField] GameObject ChestPanel, InventoryPanel;
    [SerializeField] private TransparentObject m_TransparentObject;
    [SerializeField] private Color m_Color;
    public List<Slot_UI> InventorySlots = new List<Slot_UI>();
    public List<ChestSlotUI> ChestSlots = new List<ChestSlotUI>();
    void Awake() 
    {
        for(int i = 0; i < InventorySlots.Count; i++)
        {
            InventorySlots[i].SlotIdx = i;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Setup();
    }
    void Update()
    {
        if(m_Player.GetMoveOrNot() != Vector2.zero && GetChestUIOnOrOff() == true)
        {
            TurnOffChestUI();
        }
        if(InventoryPanel.activeSelf == true)
        {
            if(m_TransparentObject.gameObject.activeSelf == false)
            {
                m_TransparentObject.gameObject.SetActive(true);
            }
        }

        if(InventoryPanel.activeSelf == false)
        {
            if(m_TransparentObject.gameObject.activeSelf == true)
            {
                m_TransparentObject.gameObject.SetActive(false);
            }
        }
    }
    public void Setup()
    {
        if(InventorySlots.Count == m_Player.GetInventoryNumSlot())
        {
            for(int i = 0; i < InventorySlots.Count; i++)
            {
                if(m_Player.GetCollectableType(i) != CollectableType.NONE)
                {
                    InventorySlots[i].SetItem(m_Player.GetSlot(i));
                    // InventorySlots[i].Setup(m_Player.GetInventoryController().InventorySlots[i]);
                }
                else 
                {
                    InventorySlots[i].SetEmpty();
                }
                InventorySlots[i].thisImage.color = m_Color;
            }
        }
    }

    public void TurnOnChestUI()
    {
        ChestPanel.SetActive(true);
        InventoryPanel.SetActive(true);
        Setup();
    }

    public void TurnOffChestUI()
    {
        ChestPanel.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    public bool GetChestUIOnOrOff() // true -> on, false -> off
    {
        if(InventoryPanel.activeSelf == true)   return true;
        else return false;
    }
}
