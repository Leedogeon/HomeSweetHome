using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public GameObject leverIMG;
    public Camera MainCam;
    public Camera BossCam;
    [SerializeField] GameObject Lever;
    [SerializeField] BossHPUI bossUI;
    void Start()
    {
        player = FindObjectOfType<Player>();
        if(leverIMG != null )
        {
            leverIMG.SetActive( false );
        }
    }

    public void ChangeBtnColor(Color newColor, Image BtnName)
    {
        if (BtnName != null)
        {
            BtnName.color = newColor;
        }
        else
        {
            Debug.Log("falied");
        }
    }

    private void Update()
    {
        if(player != null)
        {
            if(player.HasKey)
            {
                leverIMG.SetActive ( true );
                if(Lever != null) Lever.SetActive ( false );
            }
            else if(!player.HasKey)
            {
                leverIMG.SetActive(false);
            }
        }
    }

    public void CamChange(string CamName)
    {
        if (CamName == "Main")
        {
            gameObject.GetComponent<Canvas>().worldCamera = MainCam;
        }
        else if (CamName == "Boss")
        {
            gameObject.GetComponent<Canvas>().worldCamera = BossCam;

        }
    }

}
