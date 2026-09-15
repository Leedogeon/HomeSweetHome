using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonLight : MonoBehaviour
{
    public GameObject W1;


    private void Start()
    {
        Transform Wbtn = GameObject.Find("w1")?.transform;
        if (Wbtn != null)
        {
             W1 = Wbtn.gameObject;
        }
    }
    public void ChangeBtnColor(Color newColor, GameObject BtnName)
    {
        if (BtnName != null)
        {
            SpriteRenderer sprite = BtnName.GetComponent<SpriteRenderer>();

            if (sprite!=null)
            {
                sprite.color = newColor;
            }
        }
        else
        {
            Debug.Log("falied");
        }
    }

    private void Update()
    {
        ChangeBtnColor(Color.yellow, W1);
    }
}
