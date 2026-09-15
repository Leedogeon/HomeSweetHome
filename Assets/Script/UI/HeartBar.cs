using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Image MaxHealth;
    [SerializeField] private Image CurHealth;


    private IEnumerator Start()
    {
        while (Initialize.Instance.GetPlayerInstance() == null)
        {
            yield return null; // 다음 프레임까지 대기
        }
        Player = Initialize.Instance.GetPlayerInstance().GetComponent<Player>();
        MaxHealth.fillAmount = Player.Hp / 10f;

    }

    

    private void Update()
    {
        if(Player == null)
        {
            UpdateCharacterReference();
        }

        Renewal();
    }

    public void Renewal()
    {
        CurHealth.fillAmount = Player.Hp / 10f;
    }

    private void UpdateCharacterReference()
    {
        GameObject PlayerInstance = Initialize.Instance.GetPlayerInstance();
        if (PlayerInstance != null)
        {
            Player = Initialize.Instance.GetPlayerInstance().GetComponent<Player>();
        }
        else
        {
            Debug.LogWarning("PlayerInstance를 찾을 수 없습니다. 리스폰 후 다시 확인됩니다.");
        }
    }
}
