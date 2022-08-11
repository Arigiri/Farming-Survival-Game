using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantController : MonoBehaviour
{
    [SerializeField] private PlantInformation m_PlantInformation;
    private TreeType m_Type;
    private AnimatedTile m_Tile;
    private void Awake() {
        
        print(m_PlantInformation.PlantTile);
    }
    public TreeType GetTreeType()
    {
        m_Tile = m_PlantInformation.PlantTile;
        return m_Type;
    }
    public AnimatedTile GetAnimatedTile()
    {
        return m_Tile;
    }
}

public enum TreeType
{
    Tomato
};