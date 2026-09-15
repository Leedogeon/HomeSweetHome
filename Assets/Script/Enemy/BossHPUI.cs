using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [SerializeField] BossControl boss;
    bool hpONOFF = false;
    [SerializeField] private Image HpImage;
    float bossMaxHp = 0f;
    private void Start()
    {
        boss = FindObjectOfType<BossControl>();
        if(boss!= null )
        {
            bossMaxHp = boss.Hp;
        }
    }
    void Update()
    {
        HpImage.fillAmount = boss.Hp / bossMaxHp;
    }

    public void HPON()
    {
        hpONOFF = !hpONOFF;
        gameObject.SetActive(hpONOFF);
    }
}
