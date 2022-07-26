using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Play,
        Pause,
        Death
    }
    [SerializeField] GameObject DeadScreen, PausePanel, MenuPanel, PlayScreen;
    [SerializeField] PlayerController m_Player;
    private GameState CurrState;
    void Start()
    {
        SetState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if(!PausePanel.GetComponent<PausePanel>().OnTrigger() && CurrState != GameState.Menu)
            {
                SetState(GameState.Pause);
            }
            else 
            {
                SetState(CurrState);
            }

    }

    public void SetState(GameState State)
    {
        if(State == GameState.Pause)
        {
            m_Player.Active = false;
            PausePanel.SetActive(true);
        }
        else
        {
            CurrState = State;
            PausePanel.SetActive(false);
            DeadScreen.SetActive(State == GameState.Death);
            MenuPanel.SetActive(State == GameState.Menu);
            PlayScreen.SetActive(State == GameState.Play);
            m_Player.Active = true;
        }
    }
}

// Hi, my name is Dung