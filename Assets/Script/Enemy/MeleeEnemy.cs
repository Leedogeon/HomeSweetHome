using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private float Walkspeed = 5f;
    protected override void Start()
    {
        base.Start();
        Hp = 3;
        isMove = true;
        Player playerDis = FindObjectOfType<Player>();


    }


    public override void DetectPlayer()
    {
        Vector2 boxSize = new Vector2(20f, 1f); // 가로 10f, 세로 1f
        //Vector2 boxCenter = (Vector2)transform.position + (Vector2)transform.right * -(boxSize.x / 2);
        Vector2 boxCenter = transform.position;
        player = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);
    }

    protected override void Update()
    {
        base.Update();

        if (player!=null)
        {
            anim.SetBool("IsFind", true);

                // 플레이어 방향 계산
                direction = (player.transform.position - transform.position).normalized;
                direction.y = 0f;

            if (isMove && !anim.GetBool("Death"))
            {
                //rigid.velocity = direction * Walkspeed;
                Vector2 newVelocity = new Vector2(direction.x * Walkspeed, rigid.velocity.y);
                rigid.velocity = newVelocity;
            }

            if (direction.x < 0) // 만약 플레이어가 왼쪽에 있으면
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip
            }
            else if (direction.x > 0) // 만약 플레이어가 오른쪽에 있으면
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip back
            }
        }
        else
        {
            anim.SetBool("IsFind", false);
        }

    }

    /*    private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector2 boxSize = new Vector2(10f, 1f);
            Vector2 boxCenter = (Vector2)transform.position + (Vector2)transform.right * -(boxSize.x / 2);

            Gizmos.DrawWireCube(boxCenter, boxSize);
        }*/
}
