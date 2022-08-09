using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator m_Animator;
    [SerializeField] private InputAction m_InputAction;
    [SerializeField] private float m_movespeed;
    private Vector2 m_MoveDirection = Vector2.zero;
    private Rigidbody2D rb;

    private void OnEnable() 
    {
        m_InputAction.Enable();
    }

    private void OnDisable() 
    {
        m_InputAction.Disable();
    }


    private int IdleHash, RunHash, DyingHash;
    void Start()
    {
        IdleHash = Animator.StringToHash("Idle");
        DyingHash = Animator.StringToHash("Dying");
        RunHash = Animator.StringToHash("Run");

        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        m_MoveDirection = m_InputAction.ReadValue<Vector2>();
        if(m_MoveDirection != Vector2.zero)
            Run();
        else
            Idle();
        if(m_MoveDirection.x > 0)
            transform.localScale = Vector3.one;
        else if (m_MoveDirection.x < 0)
            transform.localScale= new Vector3(-1, 1, 1);
    }

    private void FixedUpdate() 
    {
        rb.velocity = m_MoveDirection * m_movespeed;
    }

    [ContextMenu("Run")]
    private void Run()
    {
        m_Animator.SetBool("Run", true);
        m_Animator.SetBool("Idle", false);
    }
    [ContextMenu("Idle")]
    private void Idle()
    {
        m_Animator.SetBool("Idle", true);
        m_Animator.SetBool("Run", false);
    }
    [ContextMenu("Die")]
    private void Die()
    {
        m_Animator.SetBool(DyingHash, true);
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Run", false);
    }
}
