using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Play,
        Pause,
        Death,
        Settings,
        MenuControllerSetiing,
        PauseControllerSetting,
    }
    [SerializeField] GameObject DeadScreen, PausePanel, MenuPanel, PlayScreen, ObjectPools, SettingsPanel, MenuBackgroundButtons, ControllerPanel, ControllerText;
    [SerializeField] PlayerController m_Player;
    [SerializeField] Inventory_UI m_InventoryUI;
    [SerializeField] AttributeUIController m_AtrributeUI;
    [SerializeField] GameObject m_DayNightSystem;
    [SerializeField] TextMeshProUGUI m_ControllerText;
    [SerializeField] private Keybindings keybindings;
    [SerializeField] private DefaultKeybindings m_DefaultKeybindings;
    private GameState CurrState; // bo khong dung nua, currstate thay bang GetCurrState()
    [SerializeField] private GameState[] GameStateList = new GameState[100];
    private int GameStateIndex; // cac gamestate duoc luu tu 0 den GameStateIndex - 1. Tuc GameStateIndex chua co gi

    private void Awake() 
    {
        GameStateIndex = 0;
        for(int i = 0; i < keybindings.keybindingsCheck.Length; i++)
        {
            keybindings.keybindingsCheck[i].keybindingActions = m_DefaultKeybindings.keybindingsCheck[i].keybindingActions;
            keybindings.keybindingsCheck[i].keyCode = m_DefaultKeybindings.keybindingsCheck[i].keyCode;
        }
    }
    void Start()
    {
        SetState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if((GetCurrState() == GameState.Play || GetCurrState() == GameState.Pause) && InputManager.instance.GetKeyDown(KeybindingActions.Pause)) // bat tat bang pause 
        {
            if(!PausePanel.GetComponent<PausePanel>().OnTrigger())
            {
                SetState(GameState.Pause);
            }
            else 
            {
                BackState();
            }
        }
        
        if(GetCurrState() != GameState.Play && GetCurrState() != GameState.Pause && InputManager.instance.GetKeyDown(KeybindingActions.Pause))
        {
            BackState();
        }

        if(InputManager.instance.GetKeyDown(KeybindingActions.Inventory) && GetCurrState() == GameState.Play && !PausePanel.gameObject.activeSelf)
        {
            bool check = !m_InventoryUI.gameObject.activeSelf;
            if(check)
                m_InventoryUI.Setup(false);
            m_InventoryUI.gameObject.SetActive(check);
        }
    }

    public void SetState(GameState State)
    {
        if(State == GameState.Play || State == GameState.Death) // khi vao mot gamestate ma khong muon co the quay lai state truoc do(vi du gamestate.play khong muon quay lai gamestate.menu)
        {
            SetEmptyStateList();//tao mot statelist moi voi gamestate.play la phan tu dau tien cua list => khong the ans esc de quay lai menu duoc
        }
        
        if(State == GameState.Pause)
        {
            m_Player.Active = false;
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            SetCurrState(State);
        }
        else if(State == GameState.Settings)
        {
            ControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            // ControllerText.SetActive(State == GameState.Settings);
            PausePanel.SetActive(State == GameState.Pause);
            MenuBackgroundButtons.SetActive(false);
            SetCurrState(State);
        }
        else if(State == GameState.MenuControllerSetiing)
        {
            ControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            SetCurrState(State);
        }
        else
        {
            Time.timeScale = 1;
            SetCurrState(State);
            PausePanel.SetActive(State == GameState.Pause);
            DeadScreen.SetActive(State == GameState.Death);
            MenuPanel.SetActive(State == GameState.Menu);
            PlayScreen.SetActive(State == GameState.Play);
            ControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            MenuBackgroundButtons.SetActive(true);
            SetActivePlayer(State);
            ObjectPools.SetActive(State == GameState.Play || State == GameState.Pause);
            m_AtrributeUI.gameObject.SetActive(State == GameState.Play);
            m_InventoryUI.gameObject.SetActive(false);
            if(State == GameState.Play || State == GameState.Pause) m_DayNightSystem.SetActive(true);
            else m_DayNightSystem.SetActive(false);
            m_Player.Active = true;
        }
    }

    public bool BackState() // quay lai gamestate truoc do
    {
        if(GameStateIndex <= 1)
        {
            print("Khong co gamestate o truoc");
            return false;
        }
        GameStateIndex-=2;
        SetState(GameStateList[GameStateIndex]);
        return true;
    }

    public void SetActivePlayer(GameState State)
    {
        m_Player.gameObject.SetActive(State != GameState.Menu);
    }
    public GameState GetCurrState()
    {
        return GameStateList[GameStateIndex-1];
    }
    public void SetCurrState(GameState gameState)
    {
        GameStateList[GameStateIndex] = gameState;
        GameStateIndex++;
    }
    public void SetEmptyStateList()
    {
        GameStateIndex = 0;
    }
}

// Hi, my name is Dung