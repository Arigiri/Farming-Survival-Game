using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingInformationPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CraftingUnitName, Description, QuanityText;
    [SerializeField] private GameObject[] m_Materials;
    [SerializeField] private Slider m_QuanitySlider;
    [SerializeField] private InventoryController m_Inventory;
    [SerializeField] private Button CraftButton;
    private List<Tuple<int, int>> MaterialIndex;
    public Recipe m_Recipe;
    public bool InformationShow = false;

    private void OnEnable() {
        ClearText();
    }
    public void SetUp()
    {
        for(int i = 0; i < m_Materials.Length; i++)
        {
            var ImageSprite = m_Materials[i].GetComponent<Image>();
            var NameText = m_Materials[i].GetComponentInChildren<TextMeshProUGUI>();
            var QuanityText = m_Materials[i].transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>();
            m_Materials[i].gameObject.SetActive(true);
            try
            {
                ImageSprite.sprite = m_Recipe.m_Material[i].Icon;
                NameText.text = m_Recipe.m_Material[i].name;
                QuanityText.text = m_Recipe.GetAmount(i).ToString();
            }
            catch
            {
                m_Materials[i].SetActive(false);
            }  
        }
        ResetText();
    }
    public void ClearText()
    {
        InformationShow = false;
        CraftingUnitName.text = "";
        Description.text = "";
        QuanityText.text = "0";
        CraftButton.interactable = false;
        m_QuanitySlider.maxValue = 0;
        foreach(var unit in m_Materials)
        {
            unit.SetActive(false);
        }

    }
    public void ResetText()
    {
        if(InformationShow == false)return;
        CraftingUnitName.text = m_Recipe.m_Recipe.Name;
        Description.text = m_Recipe.m_Recipe.Result.m_Information.Description;
        int MaxCount = GetMaxCountCanCraft();
        // QuanityText.text = MaxCount.ToString();
        m_QuanitySlider.maxValue = MaxCount;
        if(MaxCount == 0)CraftButton.interactable = false;
        else CraftButton.interactable = true;
    }
    public void OnSliderValueChange()
    {
        float Value = Mathf.Floor(m_QuanitySlider.value);
        // m_QuanitySlider.value = Mathf.Round(m_QuanitySlider.value);
        QuanityText.text = Value.ToString();
    }
    public void Craft()
    {
        int CraftCount = Int32.Parse(QuanityText.text);
        m_Recipe.m_Recipe.Result.ResetAttribute(true);
        for(int i = 0; i < CraftCount; i++)
        {
            m_Inventory.Add(m_Recipe.m_Recipe.Result);
            foreach(var item in MaterialIndex)
            {
                var index = item.Item1;
                print(item.Item2);
                for(int j = 0; j < item.Item2; j++)m_Inventory.Remove(index);
            }
        }
        ResetText();
    }

    public int GetMaxCountCanCraft()
    {
        MaterialIndex = new List<Tuple<int, int>>();
        Dictionary<CollectableType, int> CheckCount = new Dictionary<CollectableType, int>();
        for(int i = 0; i < m_Inventory.Slots.Count; i++)
        {
            var slot = m_Inventory.Slots[i];
            if(slot.Type == CollectableType.NONE)continue;
            int index = m_Recipe.GetIndex(slot.m_CollectableObject);
            if(index < 0)continue;
            CheckCount[slot.Type] = slot.Count;
            MaterialIndex.Add(new Tuple<int, int>(i, m_Recipe.GetAmount(index)));
        }
        int result = 10000;
        for(int i = 0; i < m_Recipe.m_Material.Count; i++)
        {
            float count = m_Recipe.GetAmount(i);
            var item = m_Recipe.m_Material[i].GetCollectableType();
            float ItemCount = 0f;
            try
            {
                ItemCount = CheckCount[item];
            }
            catch
            {
                continue;
            }
            // print(result);
            result = Mathf.Min(Mathf.FloorToInt(ItemCount/count), result);
        }
        return result == 10000 ? 0 : result;
    }
}
