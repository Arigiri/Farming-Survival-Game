using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerActionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController m_Player;
    [SerializeField]private TileController m_TileController;
    [SerializeField] private ToolBarController m_ToolBar;
    private TileBase CurrTile;
    private Vector3 CropPosition;
    private Vector3 MousePosition;
    private TreeType m_TreeType;
    private OnMapBuilding m_Building;
    public bool CanCut = false;
    
    public void SetBuilding(OnMapBuilding building)
    {
        m_Building = building;
    }
    public OnMapBuilding GetBuilding()
    {
        return m_Building;
    }
    public TileController GetTileController()
    {
        return m_TileController;
    }

    public void SetTreeTile(TreeType Type)
    {
        m_TreeType = Type;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Player.GetCurrItem() != null && m_Player.CanAction)m_Player.SetAction(m_Player.GetCurrItem().m_Action);
        else m_Player.CanAction = true;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnTriggerStay2D(Collider2D other) {
        var NewObject = other.gameObject.GetComponent<OnMapObjectController>();
        if(NewObject != null)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var name = m_TileController.GetTileName(mousePosition, m_TileController.m_CropTileMap);
            if(name != null && name.IndexOf("Tree") >= 0 && other.name == "ObjectMap")
            {
                // if((MousePosition - m_Player.transform.position).magnitude <= 10)CanCut = true;
                CanCut = true;
                m_Player.Chop(true);
            }
            // print(m_TileController.GetTileName(m_Player.transform.position, m_TileController.m_CropTileMap));
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        m_Player.Chop(false);
        CanCut = false;
    }

    public void TriggerAction(Action m_Action)
    {
        if(m_Action == Action.None)return;
        switch(m_Action)
        {
            case Action.Cut : m_TileController.CutDownTree(CropPosition); break;
            case Action.Hoe : m_TileController.SetCropTile(m_Player, CropPosition); break;
            case Action.Water : m_TileController.SetWateredTile(CropPosition); break;
            case Action.Plant : m_TileController.SetPlantTile(CropPosition, m_Player.GetCurrItem().m_TreeType);break;
            case Action.GrowSapling : m_TileController.SetTreeTile(CropPosition); break;
            case Action.Havest : print("Havest"); m_TileController.Havest(CropPosition); break;
            case Action.Place : m_TileController.SetOnMapObjectTile(CropPosition, m_Building);break;
            default : print("Quen Setup Kia!!!"); break;            
        }
        if(m_Player.IsInteracting == false)m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability --;
        try
        {
            if(m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability <= 0 && (m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_CollectableObject.IsTool || !m_Player.IsInteracting) )
            {
                m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].RemoveItem();
            }
        }
        catch {}
        m_ToolBar.Setup();
        
        // Debug.Break();
    }

    public void SetCropPosition(Vector3 Position)
    {
        CropPosition = Position;
    }

    public bool CanCrop()
    {
        return m_TileController.CanCrop(m_Player, MousePosition);
    }

    public bool CanWater()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanWater(m_Player, MousePosition);
    }

    public bool CanPlant()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanPlant(m_Player, MousePosition) && m_Player.GetAction() != Action.Havest;
    }

    public bool CanGrowSapling()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanGrowSapling(m_Player, Position);
    }
    public bool CanInteractive()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanInteractive(m_Player, MousePosition);// &&(m_Player.GetCurrItem() == null || m_Player.GetCurrItem().m_Action != Action.Plant);
    }
    public bool CanPlace()
    {
        return m_TileController.CanCrop(m_Player, MousePosition);
    }
    public Action GetActionFromTile(Vector3 Position)
    {
        var name = m_TileController.GetTileName(Position, m_TileController.m_CropTileMap);
        if(name == "")return Action.None;
        int Level = Convert.ToInt32(name[name.Length - 1]) - Convert.ToInt32('0');
        switch(name.TrimEnd(name[name.Length - 1]))//tree
        {
            case "Tomato": if(Level == 3) return Action.Havest; break;
        }
        switch(name)
        {
            case "Chest": return Action.Interact;
            default: break;
        }
        return Action.None;
    }
}

public enum Action
{
    None = 0,
    Cut = 1,
    Hoe = 2,
    Dig = 3,
    Havest = 4,
    Water = 5,
    Plant = 6,
    GrowSapling = 7,
    Interact = 8,// Tuong tac voi cac do vi du nhu ruong
    Place = 9,
}
