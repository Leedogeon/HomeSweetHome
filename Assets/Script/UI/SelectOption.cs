using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectOption : MonoBehaviour
{
    public CanvasGroup self;
    public GameObject SelectCanvas;
    public GameObject optionCanvas;
    public GameObject KeySettingCavas;
    public bool IsGameScence = false;
    void Start()
    {
        self = GetComponent<CanvasGroup>();
        if(self == null )
        {
            self = gameObject.AddComponent<CanvasGroup>();
        }
        SelectCanvas = GameObject.Find("SelectCanvas");
        optionCanvas = GameObject.Find("OptionCanvas");
        optionCanvas.SetActive(false);
        KeySettingCavas = GameObject.Find("KeyboardCanvas");
        KeySettingCavas.SetActive(false);
    }

    public void Resolution()
    {
        UIVisible(self);
        optionCanvas.SetActive(true);
    }

    public void KeySettingOpiton()
    {
        UIVisible(self);
        KeySettingCavas.SetActive(true);
    }

    public void UIVisible(CanvasGroup Option)
    {
        
        bool isvisible = Option.alpha > 0;
        // 투명도 조절
        Option.alpha = isvisible ? 0 : 1;
        // 버튼 클릭여부
        Option.interactable = !isvisible;
        // UI 클릭 비활성화
        Option.blocksRaycasts = !isvisible;
    }

    public void returnBtn()
    {
        SceneManager.UnloadSceneAsync("OptionScene");
        Time.timeScale = 1.0f;
        if (IsGameScence)
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
        
    }
    public void ApplyBtn()
    {

        Initialize.Instance.isPaused = false;
        if (IsGameScence)
        {
            Time.timeScale = 1.0f;
            SelectCanvas.SetActive(false);
        }
        else SceneManager.UnloadSceneAsync("OptionScene");


    }
}
