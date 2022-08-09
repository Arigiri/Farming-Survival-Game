using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    [SerializeField] private GameObject m_InventoryPanel;
    // [SerializeField] private Button m_Button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ShowInventory();
        }
    }

    private void ShowInventory()
    {
        if(m_InventoryPanel.activeSelf == false)    m_InventoryPanel.SetActive(true);
        else m_InventoryPanel.SetActive(false);
    }

}
