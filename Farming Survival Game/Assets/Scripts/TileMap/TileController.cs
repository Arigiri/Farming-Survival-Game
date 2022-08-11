using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
   public Tile TileSelect;
   public Tilemap m_TileMap;
   public RuleTile m_UnWateredCropTile;
   public RuleTile m_WateredCropTile;
   public PlantController[] m_Crops;
   public Tilemap m_UnWateredCropTileMap;
   public Tilemap m_WateredCropTileMap;
   public Tilemap m_CropTileMap;
   [SerializeField] private float m_MaxLengthPlace;
   Vector3Int Location = Vector3Int.zero;
   private List<Vector3Int> OnMapObjectsList = new List<Vector3Int>();

   private void Start() {
      // m_UnWateredCropTile.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0);
   }
   private void Update() 
   {
      m_TileMap.SetTile(Location,null);
      Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Location = m_TileMap.WorldToCell(MousePosition);
      m_TileMap.SetTile(Location,TileSelect);
   }

   

   public bool CanCrop(PlayerController m_Player, Vector3 Position)
   {
      Vector3Int PlayerLocation = m_TileMap.WorldToCell(m_Player.transform.position);
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      return ((PlayerLocation - NewLocation).magnitude <= m_MaxLengthPlace && m_UnWateredCropTileMap.GetTile(NewLocation) == null && OnMapObjectsList.Contains(NewLocation) == false);
   }

   public bool CanWater(PlayerController m_Player,Vector3 Position)
   {
      Vector3Int PlayerLocation = m_TileMap.WorldToCell(m_Player.transform.position);
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      return (PlayerLocation - NewLocation).magnitude <= m_MaxLengthPlace && m_UnWateredCropTileMap.GetTile(NewLocation) != null && m_WateredCropTileMap.GetTile(NewLocation) == null;
   }

   public bool CanPlant(PlayerController m_Player, Vector3 Position)
   {
      Vector3Int PlayerLocation = m_TileMap.WorldToCell(m_Player.transform.position);
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      return (PlayerLocation - NewLocation).magnitude <= m_MaxLengthPlace && m_WateredCropTileMap.GetTile(NewLocation) != null && m_CropTileMap.GetTile(NewLocation) == null;
   }
   public void SetWateredTile(Vector3 Position)
   {
      // print("WaterCrop");
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_WateredCropTileMap.SetTile(NewLocation, m_WateredCropTile);
   }
   public void SetCropTile(PlayerController m_Player, Vector3 Position)
   {
      // print("StartCrop");
      
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_UnWateredCropTileMap.SetTile(NewLocation, m_UnWateredCropTile);
   }
   public void SetPlantTile(PlayerController m_Player, Vector3 Position, TreeType type)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      foreach(PlantController plant in m_Crops)
      {
         if(plant.GetTreeType() == type)
         {
            m_CropTileMap.SetTile(NewLocation, plant.GetAnimatedTile());
            print(plant.GetAnimatedTile());
            return;
         }
      }
      
   }
   public Vector3Int GetTile(Vector3 Position, bool IsObjectOnMap)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      if(IsObjectOnMap)OnMapObjectsList.Add(NewLocation);
      return m_TileMap.WorldToCell(Position);
   }
   public void RemoveOnMapObject(Vector2 Position)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      if(OnMapObjectsList.Contains(NewLocation))
         OnMapObjectsList.Remove(NewLocation);
   }
}
