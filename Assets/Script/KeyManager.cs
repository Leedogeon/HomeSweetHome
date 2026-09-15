using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum KeyAction { UP, DOWN, LEFT, RIGHT ,ATTACK,JUMP,DASH,BLOCK,SKILL,OPEN,KEYCOUNT}

public static class KeySetting { public static Dictionary<KeyAction, KeyCode>keys  = new Dictionary<KeyAction, KeyCode>();}

public class KeyManager : MonoBehaviour
{
    public GameObject KeyboardCanvas;
    private SelectOption selectOption;

    KeyCode[] defaultKeys = new KeyCode[] {KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.W,KeyCode.S,KeyCode.R, KeyCode.Q,KeyCode.E};

    private void Start()
    {
        KeyboardCanvas = GameObject.Find("KeyboardCanvas");
        selectOption = FindObjectOfType<SelectOption>();
    }
    private void Awake()
    {
        if(KeySetting.keys.Count == 0)
        {
            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            {
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
            }
        }
        
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            // 0~3은 방향키 이동이라서 변경 금지
            if (key < 4) return;
            bool canChange = true;
            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            {
                // 이미 해당하는 키가 할당되어있는경우 무시
                if (KeySetting.keys[(KeyAction)i] == keyEvent.keyCode)
                {
                    canChange = false;
                    break;
                }
            }
            // 입력한 키로 할당
            if (canChange)
                KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;

            key = -1;
        }        
    }
    int key = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }

    public void retrunBtn()
    {
        if (selectOption != null)
        {
            selectOption.UIVisible(selectOption.self);
        }
        KeyboardCanvas.SetActive(false);
    }
}
