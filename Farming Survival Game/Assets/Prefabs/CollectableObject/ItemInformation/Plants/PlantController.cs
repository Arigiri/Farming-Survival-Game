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
    public void SetUp() {
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
    public Tile GetAnimatedTile(int index)
    {   
        return m_Tile[index];
    }
    public int GetMaxSize()
    {
        return m_Tile.Count;
    }
    public void SelfDestroy(Vector3 Position)
    {
        var m_Player = FindObjectOfType<PlayerController>();
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(Position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in m_PlantInformation.ItemsDrops)
        {
            item.ResetAttribute(true);
            Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, SpawnPoint);
        }
    }
    
}

public enum TreeType
{
    Tomato,
    None
};