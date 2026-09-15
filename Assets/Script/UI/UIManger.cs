using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
    public TMP_Text[] txt;
    private void Start()
    {
        KeySet();

    }
    private void Update()
    {
        KeySet();
    }

    private void KeySet()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            if (KeySetting.keys[(KeyAction)i].ToString() == "UpArrow")
            {
                txt[i].text = "ก่";
            }
            else if (KeySetting.keys[(KeyAction)i].ToString() == "DownArrow")
            {
                txt[i].text = "ก้";
            }
            else if (KeySetting.keys[(KeyAction)i].ToString() == "LeftArrow")
            {
                txt[i].text = "ก็";
            }
            else if (KeySetting.keys[(KeyAction)i].ToString() == "RightArrow")
            {
                txt[i].text = "กๆ";
            }
            else txt[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }
}
