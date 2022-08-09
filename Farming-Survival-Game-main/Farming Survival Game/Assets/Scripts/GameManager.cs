using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Menu,
        Play,
        Pause,
        Death
    }
    [SerializeField] GameObject DeadScreen, PausePanel, MenuPanel;
    private GameState CurrState;
    private GameState TempState;
    void Start()
    {
        SetState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if(!PausePanel.GetComponent<PausePanel>().OnTrigger())
            {
                TempState = CurrState;
                SetState(GameState.Pause);
            }
            else 
            {
                SetState(TempState);
            }

    }

    private void SetState(GameState State)
    {
        if(State == GameState.Pause)
            PausePanel.SetActive(true);
        else
        {
            CurrState = State;
            PausePanel.SetActive(false);
            DeadScreen.SetActive(State == GameState.Death);
            MenuPanel.SetActive(State == GameState.Menu);
        }
    }
}

// Hi, my name is Dung