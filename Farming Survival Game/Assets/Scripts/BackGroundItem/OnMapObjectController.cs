using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapObjectController : MonoBehaviour
{
    public CollectableObjectController[] ItemsDropper;
    [SerializeField]private Action m_Action;
    private TileController m_TileController;
    private Vector3Int CurrTile;
    // Start is called before the first frame update
    void Start()
    {
        m_TileController = FindObjectOfType<TileController>();
        CurrTile = m_TileController.GetTile(transform.position, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfDestroy()
    {
        DropItem();
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    private void DropItem()
    {
        PlayerController m_Player = FindObjectOfType<PlayerController>();
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(transform.position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in ItemsDropper)
        {
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
}
