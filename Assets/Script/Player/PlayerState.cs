using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float MaxHp = 5;
    public float Hp { get; private set; } = 5;

    private void Start()
    {
        Hp = MaxHp;
    }

    public void TakeDamage(float amount)
    {
        Hp -= amount;
        if (Hp <= 0)
        {
            if (Initialize.Instance != null)
            {
                Initialize.Instance.Death();
            }
        }
    }
}
