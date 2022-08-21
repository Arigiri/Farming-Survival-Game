using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapObjectController : MonoBehaviour
{
    [SerializeField]private PlantInformation m_TreeInformation;
    private CollectableObjectController[] ItemsDropperInTree;
    [SerializeField]private Action m_Action;
    private TileController m_TileController;
    private Vector3Int CurrTile;
    // Start is called before the first frame update
    private Vector2 TempPosition;
   private void Awake() {
        TempPosition = transform.position;
   }
    void Start()
    {
        m_TileController = FindObjectOfType<TileController>();
        CurrTile = m_TileController.GetTile(transform.position, true);

        ItemsDropperInTree = m_TreeInformation.ItemsDrops;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = TempPosition;
    }

    public void SelfDestroy()
    {
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    private void DropItem(CollectableObjectController[] ItemsDropper, Vector3 Position)
    {
        if(ItemsDropper.Length == 0)return;
        PlayerController m_Player = FindObjectOfType<PlayerController>();
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(Position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in ItemsDropper)
        {
            print(item);
            item.ResetAttribute(true);
            Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.1f;
            m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, gameObject);
        }
    }
    public Action GetAction()
    {
        return m_Action;
    }

    public Vector3Int GetTile()
    {
        return CurrTile;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        print(other.gameObject.name);
    }
    public void CutDownTree(Vector3 Position)
    {
        DropItem(ItemsDropperInTree, Position);
    }
}
