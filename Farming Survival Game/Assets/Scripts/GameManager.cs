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
        Crafting,
        MenuControllerSetiing,
        PauseControllerSetting,
    }
    [SerializeField] GameObject DeadScreen, PausePanel, MenuPanel, PlayScreen, ObjectPools, SettingsPanel, MenuBackgroundButtons, MenuControllerPanel, PauseControllerPanel, ControllerText, CraftingUI;
    [SerializeField] PlayerController m_Player;
    [SerializeField] Inventory_UI m_InventoryUI;
    [SerializeField] AttributeUIController m_AttributeUI;
    [SerializeField] GameObject m_DayNightSystem;
    [SerializeField] TextMeshProUGUI m_ControllerText;
    [SerializeField] private ControllerSystem m_ControllerSystem;
    [SerializeField] private ChestUI m_ChestUI;
    [SerializeField] private List<GameObject> UIList;
    private GameState CurrState; // bo khong dung nua, currstate thay bang GetCurrState()
    [SerializeField] private GameState[] GameStateList = new GameState[100];
    private int GameStateIndex; // cac gamestate duoc luu tu 0 den GameStateIndex - 1. Tuc GameStateIndex chua co gi

    private void Awake() 
    {
        GameStateIndex = 0;
        m_ControllerSystem.SetAsDefault();
    }
    void Start()
    {
        SetState(GameState.Menu);
    }
    public bool IsActiveUI() //kiem tra xem co UI nao dang active ko
    {
        foreach(var gameobject in UIList)
        {
            if(gameobject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if((GetCurrState() == GameState.Play || GetCurrState() == GameState.Pause) && InputManager.instance.GetKeyDown(KeybindingActions.Pause)) // bat tat bang pause 
        {
            if(!PausePanel.gameObject.activeSelf)
            {
                SetState(GameState.Pause);
            }
            else 
            {
                BackState();
            }
        }
        
        if(GetCurrState() != GameState.Play && GetCurrState() != GameState.Pause && Input.GetKeyDown(KeyCode.Escape))//An escape de quay lai State truoc do
        {
            BackState();
        }

        if(InputManager.instance.GetKeyDown(KeybindingActions.Inventory) && GetCurrState() == GameState.Play && !PausePanel.gameObject.activeSelf)
        {
            if(m_ChestUI.GetChestUIOnOrOff() == true)
            {
                m_ChestUI.TurnOffChestUI();
            }
            else 
            {
                bool check = !m_InventoryUI.gameObject.activeSelf;
                if(check)
                    m_InventoryUI.Setup(false);
                m_InventoryUI.gameObject.SetActive(check);
            }
        }
        if(InputManager.instance.GetKeyDown(KeybindingActions.Craft) && GetCurrState() == GameState.Play && !PausePanel.gameObject.activeSelf)
        {
            bool check = !CraftingUI.gameObject.activeSelf;
            CraftingUI.gameObject.SetActive(check);
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
            PauseControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            Time.timeScale = 0;
            SetCurrState(State);
        }
        else if(State == GameState.Settings)
        {
            MenuControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            PauseControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            // ControllerText.SetActive(State == GameState.Settings);
            PausePanel.SetActive(State == GameState.Pause);
            MenuBackgroundButtons.SetActive(false);
            SetCurrState(State);
        }
        else if(State == GameState.MenuControllerSetiing)
        {
            if(MenuPanel.gameObject.activeSelf == true) MenuControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            else PauseControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
            SettingsPanel.SetActive(State == GameState.Settings);
            SetCurrState(State);
        }
        else
        {
            ShowPlayScreen(State);
        }
    }
    public void ShowPlayScreen(GameState State)
    {
        Time.timeScale = 1;
        SetCurrState(State);
        PausePanel.SetActive(State == GameState.Pause);
        DeadScreen.SetActive(State == GameState.Death);
        MenuPanel.SetActive(State == GameState.Menu);
        PlayScreen.SetActive(State == GameState.Play);
        MenuControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
        PauseControllerPanel.SetActive(State == GameState.MenuControllerSetiing);
        SettingsPanel.SetActive(State == GameState.Settings);
        MenuBackgroundButtons.SetActive(true);
        SetActivePlayer(State);
        ObjectPools.SetActive(State == GameState.Play || State == GameState.Pause);
        m_AttributeUI.gameObject.SetActive(State == GameState.Play);
        m_InventoryUI.gameObject.SetActive(false);
        CraftingUI.gameObject.SetActive(false);
        if(State == GameState.Play || State == GameState.Pause) m_DayNightSystem.SetActive(true);
        else m_DayNightSystem.SetActive(false);
        m_Player.Active = true;
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