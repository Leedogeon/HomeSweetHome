using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResolutionOption : MonoBehaviour
{
    private SelectOption selectOption;
    public CanvasGroup self;

    public GameObject ResolutionCanvas;
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;
    public TMP_Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionIndex = 0;



    private void Start()
    {
        ResolutionCanvas = GameObject.Find("OptionCanvas");

        // 활성화
        self = GetComponent<CanvasGroup>();
        self.alpha = 1;
        self.interactable = true;
        self.blocksRaycasts = true;

        selectOption = FindObjectOfType<SelectOption>();
        resolutionDropdown = GetComponentInChildren<TMP_Dropdown>();
        //resolutionDropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        InitUI();
    }
    void InitUI()
    {
        //resolutions.AddRange(Screen.resolutions);
        // 지원가능 해상도 디버그
        /*foreach (Resolution item in Screen.resolutions)
        {
            Debug.Log(item.width + "x" + item.height + " " + item.refreshRateRatio);
        }*/

        for (int i = 0;i < Screen.resolutions.Length;i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value >= 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        resolutionDropdown.options.Clear();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option =new TMP_Dropdown.OptionData();
            option.text = item.width + " x " + item.height + " " + item.refreshRateRatio + "hz";
            resolutionDropdown.options.Add(option);
            if(item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;

        }
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropBoxOptionChange(int x)
    {
        resolutionIndex = x;
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = fullscreenBtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void retrunBtn()
    {

        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, screenMode);
        if (selectOption != null)
        {
            selectOption.UIVisible(selectOption.self);
        }
        //SceneManager.UnloadSceneAsync("OptionScene");
        //Time.timeScale = 1.0f;
        ResolutionCanvas.SetActive(false);
    }
}
