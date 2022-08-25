using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keybindingActionsText;
    [SerializeField] TextMeshProUGUI KeyCodeText;
    [SerializeField] GetKeyBinding m_GetKeyBinding;
    [SerializeField] ControllerSystem m_ControllerSystem;
    public int KeybindingIndex;
    private KeybindingActions keybindingActions;
    private KeyCode keyCode;
    private string KeyCodeTextTmp;
    private bool ThisButtonClicked;
    
    private void Awake() 
    {
        ThisButtonClicked = false;
    }
    public KeybindingActions GetKeybindingActions()
    {
        return keybindingActions;
    }

    public KeyCode GetKeyCode()
    {
        return keyCode;
    }

    public void SetKeybindingActions(KeybindingActions Actions)
    {
        keybindingActions = Actions;
    }
    public void SetKeyCode(KeyCode key)
    {
        keyCode = key;
    }
    public void Setup()
    {
        if(keybindingActions == KeybindingActions.None)
        {
            SetEmpty();
            return;
        }
        keybindingActionsText.text = keybindingActions.ToString();
        KeyCodeText.text = keyCode.ToString();
    }
    public void SetEmpty()
    {
        keybindingActionsText.text = "";
        KeyCodeText.text = "";
    }
    public void GetKey()
    {
        ThisButtonClicked = true;
        m_GetKeyBinding.OnRun = true;
        KeyCodeTextTmp = KeyCodeText.text;
        KeyCodeText.text = "";
    }

    private void Update() 
    {
        if(ThisButtonClicked == false || m_GetKeyBinding.RecentKeyPressed == KeyCode.None)    
            return;

        if(m_GetKeyBinding.RecentKeyPressed == KeyCode.Mouse0 || m_GetKeyBinding.RecentKeyPressed == KeyCode.Mouse1 || m_GetKeyBinding.RecentKeyPressed == KeyCode.Mouse2)
        {
            //tra ve phim luc nay
            KeyCodeText.text = KeyCodeTextTmp;
        }
        else
        {
            // Gan phim day bang phim m_GetKeyBinding.RecentKeyPressed
            m_ControllerSystem.SetKeyCode(KeybindingIndex, m_GetKeyBinding.RecentKeyPressed);
        }
        m_GetKeyBinding.RecentKeyPressed = KeyCode.None;
        ThisButtonClicked = false;
    }
}
