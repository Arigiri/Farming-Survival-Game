using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
   public Tile TileSelect;
   public Tile[] TreeTile; 
   public RuleTile m_UnWateredCropTile;
   public RuleTile m_WateredCropTile;
   public PlantController[] m_Crops;
   public Tilemap m_TileMap;
   public Tilemap m_UnWateredCropTileMap;
   public Tilemap m_WateredCropTileMap;
   public Tilemap m_CropTileMap;
   public float HourToGrowTree; //InGameTime
   public List<Tile> m_OnMapBuildingTiles;
   [SerializeField] private TimeController m_TimeController;
   [SerializeField] private float m_MaxLengthPlace;
   Vector3Int Location = Vector3Int.zero;
   private List<Vector3Int> OnMapObjectsList = new List<Vector3Int>();
   private Dictionary<Vector3Int, PlantController> PlantOnMap = new Dictionary<Vector3Int, PlantController>();

   private void Start() {
      // m_UnWateredCropTile.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0);
   }
   private void Update() 
   {
      m_TileMap.SetTile(Location,null);
      Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Location = m_TileMap.WorldToCell(MousePosition);
      m_TileMap.SetTile(Location,TileSelect);

      if(Input.GetMouseButtonDown(0))
      {
         print(OnMapBuilding.Chest.ToString());
         print(m_OnMapBuildingTiles[0].name);
         print(OnMapBuilding.Chest.ToString() == m_OnMapBuildingTiles[0].name);
      }
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
      return (PlayerLocation - NewLocation).magnitude <= m_MaxLengthPlace &&  m_CropTileMap.GetTile(NewLocation) == null && m_UnWateredCropTileMap.GetTile(NewLocation) != null;
   }
   public bool CanGrowSapling(PlayerController m_Player, Vector3 Position)
   {
      return CanCrop(m_Player, Position);
   }

   public bool CanInteractive(PlayerController m_Player, Vector3 Position)
   {
      Vector3Int PlayerLocation = m_TileMap.WorldToCell(m_Player.transform.position);
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      return m_CropTileMap.GetTile(NewLocation) != null; // Them && NewLocation co cai ruong hoac giuong
   }
   public void SetWateredTile(Vector3 Position)
   {
      // print("WaterCrop");
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_WateredCropTileMap.SetTile(NewLocation, m_WateredCropTile);
      try
      {
         var Plant = PlantOnMap[NewLocation];
         StartCoroutine(GrowUpPlant(NewLocation, 0));
      }
      catch{}
   }
   public void SetCropTile(PlayerController m_Player, Vector3 Position)
   {
      // print("StartCrop");
      
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_UnWateredCropTileMap.SetTile(NewLocation, m_UnWateredCropTile);
   }
   public void SetPlantTile(Vector3 Position, TreeType type)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      PlantController Plant = new PlantController();
      foreach(PlantController plant in m_Crops)
      {
         if(plant.GetTreeType() == type)
         {  
            // print( plant.GetAnimatedTile(1, false));
            Plant = plant;
            Plant.SetUp();
            PlantOnMap[NewLocation] = Plant;
            m_CropTileMap.SetTile(NewLocation, Plant.GetAnimatedTile(0));
            break;
         }
      }

      if(m_WateredCropTileMap.GetTile(NewLocation) != null)StartCoroutine(GrowUpPlant(NewLocation, 0));      
   }
    IEnumerator GrowUpPlant(Vector3Int NewLocation, int GrowState)
   {
      var Plant = PlantOnMap[NewLocation];
      if(GrowState < Plant.GetMaxSize() - 1)
      {
         yield return new WaitForSeconds(Plant.TimeToGrow);
         m_CropTileMap.SetTile(NewLocation, Plant.GetAnimatedTile(GrowState));
         yield return GrowUpPlant(NewLocation, GrowState + 1);
      }
   }
   
   public void SetTreeTile(Vector3 Position)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_CropTileMap.SetTile(NewLocation, TreeTile[0]);
      StartCoroutine(GrowUp());

      IEnumerator GrowUp()
      {
         yield return new WaitForSeconds(m_TimeController.ToSystemTime(HourToGrowTree));
         m_CropTileMap.SetTile(NewLocation, TreeTile[1]);
      }
   }
   public void Havest(Vector3 Position)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      var Plant = PlantOnMap[NewLocation];
      Plant.SelfDestroy(Position);
      m_CropTileMap.SetTile(NewLocation, Plant.GetAnimatedTile(4));

      StartCoroutine(GrowUpPlant(NewLocation, 2));
   }
   public void SetOnMapObjectTile(Vector3 Position, OnMapBuilding building)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      foreach(var Building in m_OnMapBuildingTiles)
      {
         if(Building.name == building.ToString())
         {
            m_CropTileMap.SetTile(NewLocation, Building);
            break;
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
   public string GetTileName(Vector3 Position, Tilemap Tilemap)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      var Tile = Tilemap.GetTile(NewLocation);
      return  Tile == null ? "" : Tile.name;  
   }
   public TreeType GetHavestTree(Vector3 Position)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      var NewTile = m_CropTileMap.GetTile(NewLocation);
      if(NewTile != null)
      {
         if(NewTile.name.IndexOf("Tomato", 0) >= 0)
         {
            return TreeType.Tomato;
         }
         else return TreeType.None;
      }
      else return TreeType.None;
   }
   public void CutDownTree(Vector3 Position)
   {
      Vector3Int NewLocation = m_TileMap.WorldToCell(Position);
      m_CropTileMap.SetTile(NewLocation, null);
      m_CropTileMap.GetComponent<OnMapObjectController>().CutDownTree(Position);
   }
   public bool CanPutObject(Vector3Int Position)
   {
      return m_UnWateredCropTileMap.GetTile(Position) == null && OnMapObjectsList.Contains(Position) == false && m_WateredCropTileMap.GetTile(Position) == null;
   }
}
