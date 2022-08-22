using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keybindingActionsText;
    [SerializeField] TextMeshProUGUI KeyCodeText;
    private KeybindingActions keybindingActions;
    private KeyCode keyCode;

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
        keybindingActionsText.text = keybindingActions.ToString();
        KeyCodeText.text = keyCode.ToString();
    }
}
