using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectsPool : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<CollectableObjectController> Objects;
 
    public Dictionary<CollectableType, CollectableObjectPool> m_Pool = new Dictionary<CollectableType, CollectableObjectPool>() // CollectableType la enum trong CollectableObjectController
    {
        {CollectableType.Carrot, new CollectableObjectPool(CollectableType.Carrot)},
        {CollectableType.Chili, new CollectableObjectPool(CollectableType.Chili)},
        {CollectableType.Axe, new CollectableObjectPool(CollectableType.Axe)},
        {CollectableType.Hoe, new CollectableObjectPool(CollectableType.Hoe)},
        {CollectableType.WateringCan, new CollectableObjectPool(CollectableType.WateringCan)},
        {CollectableType.SeedBag, new CollectableObjectPool(CollectableType.SeedBag)},
    };
    void Awake()
    {
        foreach(var Pool in m_Pool)
        {
            foreach(var obj in Objects)
            {
                if(obj.GetCollectableType() == Pool.Key)
                    Pool.Value.m_Object = obj;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
