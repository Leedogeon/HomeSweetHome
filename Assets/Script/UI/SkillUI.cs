using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Player Player;
    [SerializeField] public Image BCD;
    [SerializeField] public Image SCD;
    [SerializeField] public Image DCD;
    public bool isBlock = false;
    public bool isSkill = false;
    public bool isDash = false;

    float fadeAlpha = .3f;
    float normalAlpha = 1f;
    [SerializeField] public Image[] Images;
    private IEnumerator Start()
    {
        while (Initialize.Instance.GetPlayerInstance() == null)
        {
            yield return null; // 다음 프레임까지 대기
        }
        Player = Initialize.Instance.GetPlayerInstance().GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBlock)
        {
            BCD.fillAmount = BCD.fillAmount - Time.deltaTime/2;
        }        
        if(BCD.fillAmount < 0)
        {
            isBlock = false;
        }

        if(isSkill)
        {
            SCD.fillAmount = SCD.fillAmount + Time.deltaTime / 3;
        }
        if(SCD.fillAmount >= 1)
        {
            isSkill = false;
        }

        if (isDash)
        {
            DCD.fillAmount = DCD.fillAmount + Time.deltaTime;
        }
        if(DCD.fillAmount >= 1)
        {
            isDash = false;
        }

    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach(Image img in Images)
            {
                Color c = img.color;
                c.a = fadeAlpha;
                img.color = c;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (Image img in Images)
            {
                Color c = img.color;
                c.a = normalAlpha;
                img.color = c;
            }
        }
    }

}
