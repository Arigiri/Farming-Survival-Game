using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUnitController : MonoBehaviour
{
    [SerializeField] private ScriptableRecipe ScriptableRecipe;
    [SerializeField]private Image m_Image;
    private CraftingInformationPanelUI m_InformationPanel;
    private Recipe m_Recipe;
    
    void Awake()
    {
        m_Recipe = new Recipe(ScriptableRecipe);
        m_InformationPanel = GameObject.FindGameObjectsWithTag("CraftingInformationPanelUI")[0].GetComponent<CraftingInformationPanelUI>();
        m_Image.sprite = ScriptableRecipe.Result.m_Information.Icon;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        m_InformationPanel.m_Recipe = m_Recipe;
        m_InformationPanel.InformationShow = true;
        m_InformationPanel.SetUp();
    }
}
