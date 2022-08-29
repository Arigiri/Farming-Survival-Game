using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRecipe : MonoBehaviour
{
    [SerializeField] private ScrollUnitManager m_Recipe;
    private GameObject ScrollList;
    private void Start() {
        ScrollList = GameObject.FindGameObjectWithTag("ScrollList");
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
        var newunit = Instantiate(m_Recipe);
        newunit.transform.SetParent(ScrollList.transform);
        
    }
}
