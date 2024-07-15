using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingletonManager<InputManager>
{
    public Action keyDownAction;

    public bool SettingOpened;

    private void Update()
    {
        if (Time.timeScale <= 0f && !SettingOpened) return;

        if (Input.anyKeyDown)
        {
            if (keyDownAction != null)
            {
                keyDownAction.Invoke();
            }
        }
    }
}
