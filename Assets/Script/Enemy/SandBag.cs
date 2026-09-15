using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : Enemy
{
    private void Start()
    {
        
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Hp = int.MaxValue;
        DamagedEffect = Resources.Load<GameObject>("Effect/Hit");
    }

    public override void TakeDamaged(int damage, Vector2 scale)
    {
        DEffect(scale);
        anim.SetTrigger("Damaged");
    }
}
