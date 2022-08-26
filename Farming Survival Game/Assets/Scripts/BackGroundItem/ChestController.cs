using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : MonoBehaviour
{
    public CollectableObjectController[] ItemsContainer;
    [SerializeField]private Action m_Action;
    
    [SerializeField] private float m_MaxLengthInteractive;
    private TileController m_TileController;
    private PlayerController m_Player;
    // Start is called before the first frame update
    public void Init()
    {
        m_Player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CanOpenChest() // Mo ruong
    {
        return false;
    }

    public void SelfDestroy() // Pha ruong va Lay ruong vao Inventory
    {
        m_Player.AddItemToInventory(gameObject.GetComponent<CollectableObjectController>());
        DropItem();
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    private void DropItem()
    {
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(transform.position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in ItemsContainer)
        {
            item.ResetAttribute(true);
            Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, SpawnPoint);
        }
    }
}
