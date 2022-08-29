using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableRecipe : ScriptableObject
{
    public List<CollectableObjectController> Material;
    public CollectableObjectController Result;
    public string Name;
}

public class Recipe
{
    public ScriptableRecipe m_Recipe;
    public List<CollectableObjectController> m_Material = new List<CollectableObjectController>();
    public Dictionary<CollectableObjectController, int> Amount = new Dictionary<CollectableObjectController, int>();
    public Recipe(ScriptableRecipe recipe)
    {
        m_Recipe = recipe;
        foreach(var material in m_Recipe.Material)
        {
            if(m_Material.Contains(material) == false)
            {
                m_Material.Add(material);
                Amount[material] = 1;
            }
            else Amount[material] += 1;
        }
    }
    public int GetAmount(int i)
    {
        return Amount[m_Material[i]];
    }
    public int GetIndex(CollectableObjectController m_Object)
    {
        for(int i = 0; i < m_Material.Count; i++)
        {   
            var item = m_Material[i];
            if(item.GetCollectableType() == m_Object.GetCollectableType())return i;
        }
        return -1;
    }
}
