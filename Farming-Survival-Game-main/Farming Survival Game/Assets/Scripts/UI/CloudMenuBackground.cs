using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMenuBackground : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject m_SpawnPoint;
    [SerializeField] private float LifeTime;
    [SerializeField] private float m_CloudMoveSpeed;
    [SerializeField] private float WakeTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(1, 0) * Time.deltaTime * m_CloudMoveSpeed);
    }

    public void CallInit()
    {
        Invoke("Init", WakeTime);
    }

    public void Init()
    {
        transform.position = m_SpawnPoint.transform.position;
        Invoke("Init", LifeTime);
    }
}
