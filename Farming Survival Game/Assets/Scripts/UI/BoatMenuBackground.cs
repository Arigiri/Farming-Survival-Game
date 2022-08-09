using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMenuBackground : MonoBehaviour
{
    [SerializeField] private Vector2 m_direction;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_WakeTime;
    [SerializeField] private Transform m_SpawnPoint;
    private bool flag = false;
    private float CurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = m_WakeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(flag == false && CurrentTime <= 0)    
        {
            Init();
            flag = true;
        }
        CurrentTime -= Time.deltaTime;
        if(flag)    transform.Translate(m_direction * Time.deltaTime * m_MoveSpeed);
    }

    private void Init()
    {
        transform.position = m_SpawnPoint.position;
        Invoke("Init", 120);
    }
}
