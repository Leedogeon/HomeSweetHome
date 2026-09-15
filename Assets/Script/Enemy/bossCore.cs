using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossCore : MonoBehaviour
{
    [SerializeField] GameObject Boss;

    public void TakeDamaged()
    {
        Boss.GetComponent<BossControl>().TakeDamaged();
    }
}
