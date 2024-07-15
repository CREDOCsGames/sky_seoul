using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SettingManager : MonoSingletonManager<SettingManager>
{
    //Resolution

    public void SetResolution(int width)
    {
        SoundManager.Instance.ButtonSound();

        Screen.SetResolution(width, (width / 16) * 9, false);

        if (width >= 1920)
        {
            Screen.fullScreen = true;
        }
    }

    //Sound
    [SerializeField] Slider[] volumes;

    public void SetVolume(int num)
    {
        switch (num)
        {
            case 0: SoundManager.Instance.SetVolumeMaster(volumes[num].value); break;
            case 1: SoundManager.Instance.SetVolumeSFX(volumes[num].value); break;
            case 2: SoundManager.Instance.SetVolumeBGM(volumes[num].value); break;
        }
    }

    //Language
    public Action changeLanguage;
    public void SetLanguage(string language)
    {
        SoundManager.Instance.ButtonSound();

        DataManager.Instance.Data.Language = language;

        if (changeLanguage != null)
        {
            changeLanguage.Invoke();
        }

        OptionTextSetting();
    }

    [SerializeField] GameObject settingUI;

    public void LeaveOption()
    {
        SoundManager.Instance.ButtonSound();

        InputManager.Instance.SettingOpened = false;

        Time.timeScale = 1f;
        settingUI.SetActive(false);
    }

    public void OpenOption()
    {
        SoundManager.Instance.ButtonSound();

        InputManager.Instance.SettingOpened = true;

        Time.timeScale = 0f;

        settingUI.SetActive(true);
    }

    //StartSetting
    public void OptionKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingUI.activeSelf)
        {
            OpenOption();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && settingUI.activeSelf)
        {
            LeaveOption();
        }
    }

    [SerializeField] TextMeshProUGUI[] texts;

    public void OptionTextSetting()
    {
        for (int i = 0; i < TextManager.Instance.GetCSVLength("OptionText"); i++)
        {
            texts[i].text = TextManager.Instance.LoadString("OptionText", i);
        }
    }

    private void SetLoadData()
    {
        volumes[0].value = DataManager.Instance.Data.MasterVolume;
        volumes[1].value = DataManager.Instance.Data.SFXVolume;
        volumes[2].value = DataManager.Instance.Data.BGMVolume;
    }

    private void Start()
    {
        InputManager.Instance.keyDownAction += OptionKeyDown;

        SetLoadData();

        OptionTextSetting();

        settingUI.SetActive(false);
    }

    //SaveData
    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveData();
    }
}
