using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantController : MonoBehaviour
{
    [SerializeField] private PlantInformation m_PlantInformation;
    private TreeType m_Type;
    private Tile[] m_Tile;
    public TreeType GetTreeType()
    {
        m_Tile = m_PlantInformation.PlantTile;
        return m_Type;
    }
    public Tile GetAnimatedTile(int index)
    {
        return m_Tile[index];
    }
}

public enum TreeType
{
    Tomato,
    NormalTree
};