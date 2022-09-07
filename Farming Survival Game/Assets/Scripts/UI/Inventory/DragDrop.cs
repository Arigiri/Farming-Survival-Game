using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Inventory_UI m_InventoryUI;
    [SerializeField] private TextMeshProUGUI m_CloneQuantity;
    [SerializeField] private ChestUI m_ChestUI;
    [SerializeField] private bool IsInventory;
    private Vector3 FirstSlotPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 StartPosition;
    public bool CheckDrop; // kiem tra xem co duoc keo tha vao dung o Slot_UI khong
    private int CurrSlotIndex; // vi tri ma Clone Icon dang o
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FirstSlotPosition = transform.position;
        // Debug.Log(FirstSlotPosition); // plus 135
        gameObject.SetActive(false);
    }

    private void Update() 
    {
        if(m_InventoryUI != null)
        {
            if(gameObject.activeSelf == true && m_InventoryUI.gameObject.activeSelf == false)
            {
                // Debug.Log("Panik!!!");
                SetActiveFalse();
            }
            if(m_CloneQuantity.text != m_InventoryUI.slots[CurrSlotIndex].GetQuantityText())
            {
                m_CloneQuantity.text = m_InventoryUI.slots[CurrSlotIndex].GetQuantityText();
            }

            if(m_InventoryUI.slots[CurrSlotIndex].thisImage.color != new Color(1, 1, 1, 0.75f))
            {
                SetActiveFalse();
            }
        }
        
        if(m_ChestUI != null)
        {
            if(gameObject.activeSelf == true && m_ChestUI.gameObject.activeSelf == false)
            {
                // Debug.Log("Panik!!!");
                SetActiveFalse();
            }
            if(m_CloneQuantity.text != m_ChestUI.ChestSlots[CurrSlotIndex].GetQuantityText())
            {
                m_CloneQuantity.text = m_ChestUI.ChestSlots[CurrSlotIndex].GetQuantityText();
            }

            if(m_ChestUI.ChestSlots[CurrSlotIndex].thisImage.color != new Color(1, 1, 1, 0.75f))
            {
                SetActiveFalse();
            }
        }
    }

    public void SetPosition(int idx)
    {
        gameObject.transform.position = FirstSlotPosition + new Vector3(135 * (idx % 9), -(135 * (idx / 9)), 0);
        CurrSlotIndex = idx;
    }

    public void SetPositionOnChestUI(int idx)
    {
        gameObject.transform.position = FirstSlotPosition + new Vector3(135 * (idx % 4), -(135 * (idx / 4)), 0);
        CurrSlotIndex = idx;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        StartPosition = gameObject.transform.position;
        CheckDrop = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if(CheckDrop == false)
        {
            gameObject.transform.position = StartPosition;
            if(m_InventoryUI != null)
            {
                if(m_CloneQuantity.text != m_InventoryUI.slots[CurrSlotIndex].GetQuantityText())
                {
                    gameObject.SetActive(false);
                }
            }
            else 
            {
                if(m_CloneQuantity.text != m_ChestUI.ChestSlots[CurrSlotIndex].GetQuantityText())
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
 
    public void OnPointerDown(PointerEventData data)
    {
        if(IsInventory)  return;
        print("OnPointerDown");
        clicked++;
        if (clicked == 1) clicktime = Time.time;
 
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            // Debug.Log("Double CLick");
            GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>().m_ChestController.MoveItemToOtherPanel(CurrSlotIndex);
 
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
 
    }

    public void SetActiveFalse() // reset Clone truoc khi xet active false
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(false);
    }
    public Inventory_UI GetInventory_UI()
    {
        return m_InventoryUI;
    }
    public int GetCurrSlotIndex()
    {
        return CurrSlotIndex;
    }
}