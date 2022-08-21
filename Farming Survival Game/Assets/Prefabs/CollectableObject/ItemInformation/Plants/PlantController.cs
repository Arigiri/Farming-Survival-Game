using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantController : MonoBehaviour
{
    [SerializeField] private PlantInformation m_PlantInformation;
    private TreeType m_Type;
    private List<Tile> m_Tile;
    public float TimeToGrow;
    private void SetUp() {
        m_Tile = new List<Tile>();
        foreach(var tile in m_PlantInformation.PlantTile)
        {
            m_Tile.Add(tile);
        }
        TimeToGrow = m_PlantInformation.DayToGrow;
    }
    public TreeType GetTreeType()
    {
        return m_Type;
    }
    public Tile GetAnimatedTile(int index, bool Setup)
    {
        if(Setup) SetUp();
        
        return m_Tile[index];
    }
    public int GetMaxSize()
    {
        return m_Tile.Count;
    }
    
}

public enum TreeType
{
    Tomato,
    None
};