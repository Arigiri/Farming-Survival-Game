using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GetKeyBinding : MonoBehaviour
{
    public KeyCode RecentKeyPressed;
    public bool OnRun;
    // Update is called once per frame
    private void Awake() 
    {
        // print("Call Awake");
        RecentKeyPressed = KeyCode.None;
        OnRun = false;
    }
    void Update()
    {
        if(OnRun == false)  return;
        if(Input.anyKeyDown)
        {
            foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                    {
                        Debug.Log("KeyCode down: " + kcode);
                        RecentKeyPressed = kcode;
                        OnRun = false;
                    }
            }
        }
    }
}
