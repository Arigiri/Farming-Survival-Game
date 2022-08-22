using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField] private Keybindings keybindings;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public KeyCode GetKeyForAction(KeybindingActions keybindingActions)
    {
        foreach(Keybindings.KeybindingsCheck keybindingsCheck in keybindings.keybindingsCheck)
        {
            if(keybindingsCheck.keybindingActions == keybindingActions)
            {
                return keybindingsCheck.keyCode;
            }
        }

        print("Quen chua khoi tao phim trong scriptableObject KeybindingActions kia");
        return KeyCode.None; //Chua khoi tao phim kia
    }

    public bool GetKeyDown(KeybindingActions key)
    {
        foreach(Keybindings.KeybindingsCheck keybindingsCheck in keybindings.keybindingsCheck)
        {
            if(keybindingsCheck.keybindingActions == key)
            {
                return Input.GetKeyDown(keybindingsCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKey(KeybindingActions key)
    {
        foreach(Keybindings.KeybindingsCheck keybindingsCheck in keybindings.keybindingsCheck)
        {
            if(keybindingsCheck.keybindingActions == key)
            {
                return Input.GetKey(keybindingsCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKeyUp(KeybindingActions key)
    {
        foreach(Keybindings.KeybindingsCheck keybindingsCheck in keybindings.keybindingsCheck)
        {
            if(keybindingsCheck.keybindingActions == key)
            {
                return Input.GetKeyUp(keybindingsCheck.keyCode);
            }
        }
        return false;
    }
}

//Huong dan su dung :D
// Neu muon viet Input.GetKeyDown(KeyCode.Escape) de mo PausePanel
// --> Hay viet InputManager.instance.GetKeyDown(KeybindingActions.Pause)
// InputManager la mot singleton public nen khong can phai khai bao [serializeField]
// Them list cac hanh dong vao enum trong script KeybindingActions
// Gan them nut o trong ScriptableObject Keybindings