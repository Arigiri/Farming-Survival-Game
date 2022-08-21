using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerActionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController m_Player;
    [SerializeField] private Tilemap m_Tilemap;
    private List<Vector2> ObjectOnQueue = new List<Vector2>();
    [SerializeField]private TileController m_TileController;
    [SerializeField] private ToolBarController m_ToolBar;
    private TileBase CurrTile;
    private Vector3 CropPosition;
    private Vector3 MousePosition;
    private TreeType m_TreeType;
    public bool CanCut = false;
    
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

    private bool MapObjectFind(Vector2 Object)
    {
        foreach(var obj in ObjectOnQueue)
        {
            if(obj == Object)return true;
        }
        return false;
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
            case Action.Plant : m_TileController.SetPlantTile(m_Player, CropPosition, m_Player.GetCurrItem().m_TreeType);break;
            // case Action.GrowSapling : m_TileController.set
            default : print("Quen Setup Kia!!!"); break;            
        }
        m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability --;
        if( m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability <= 0)
        {
             m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].RemoveItem();
        }
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
        return m_TileController.CanPlant(m_Player, MousePosition);
    }

    public bool CanGrowSapling()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanGrowSapling(m_Player, Position);
    }
    public bool CanHavest()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.GetHavestTree(Position) != TreeType.None;
    }
    public bool CanInteractive()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanInteractive(m_Player, MousePosition);
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
}
