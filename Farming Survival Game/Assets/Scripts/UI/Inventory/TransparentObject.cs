using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransparentObject : MonoBehaviour, IDropHandler
{
    [SerializeField] private Inventory_UI m_InventoryUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDropInTransparentObject");
        if(eventData.pointerDrag != null)
        {
            m_InventoryUI.RemoveAll();
        }
    }
}
