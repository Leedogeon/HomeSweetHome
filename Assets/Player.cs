using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    float MoveForward;
    float MoveUp;
    float speed = 5f;
    // 뒤집기
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;

    bool isJump = false;
    float jumpPower = 5f;
    // 점프 카운트
    int jumpCountBase = 1;
    // 인게임 점프 카운트
    int jumpCount;
    // 더블점프 획득 시 점프 카운트 변경
    bool doubleJump = false;


    // 공격
    float AttackTime = 0.2f;
    // 대쉬용
    bool isDash = false;
    bool canDash = true;
    float DashPower = 10f;
    float DashTime = .2f;
    float DashCoolDown = 1f;
    TrailRenderer tr;

    // 피격시
    bool isDamaged = false;
    // 넉백거리, 이동시간
    float KnockBackPower = 3f;
    float KnockBack = .5f;
    bool isKnockBack = false;
    // 무적시간
    float invincibilityTime = .75f;
    bool invincibility = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        if (doubleJump)
        {
            jumpCountBase = 2;
        }
        jumpCount = jumpCountBase;
        
    }

    
    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Flip();

    }
    void GetInput()
    {
        // 좌우 이동값
        MoveForward = Input.GetAxis("Horizontal");
        MoveUp = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump") && jumpCount >=1)
        {
            Jump();
        }
        if (Input.GetButtonDown("Dash") && canDash)
        {
            print("Dash");
            StartCoroutine(Dash());
        }
        if(Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(KeyCode.P) && !invincibility)
        {
            StartCoroutine(Damaged());
        }
    }
    void Move()
    {
        // flip을 이용할지 scale을 이용할지 선택해야됨
        /*if (MoveForward > 0) spriteRenderer.flipX = false;
        else if (MoveForward < 0) spriteRenderer.flipX = true;*/
        // 이동, 점프시 속도 감소
        if (!isDash && !isDamaged)
        {
            rigid.velocity = new Vector2(MoveForward * (isJump ? speed / 3 : speed), rigid.velocity.y);
        }
    }
    void Flip()
    {
        if(isFacingRight && MoveForward <0f || !isFacingRight && MoveForward >0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void Jump()
    {
        jumpCount--;
        // 점프
        isJump = true;
        rigid.velocity = Vector2.up*jumpPower;
    }
    IEnumerator Dash()
    {
        isDash = true;
        canDash = false;
        // 대쉬중 중력영향 무시
        float gravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        
        // 대각대쉬, 일반대쉬
        if(MoveUp != 0f)
            rigid.velocity = new Vector2(DashPower * Mathf.Sign(MoveForward), DashPower * Mathf.Sign(MoveUp));
        else
            rigid.velocity = new Vector2(DashPower * Mathf.Sign(MoveForward), 0f);

        tr.emitting = true;

        yield return new WaitForSeconds(DashTime);
        tr.emitting = false;
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = gravity;
        isDash = false;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
        
    }

    IEnumerator Attack()
    {
        anim.SetBool("isAttack",true);

        yield return new WaitForSeconds(AttackTime);
        anim.SetBool("isAttack", false);
    }

    IEnumerator Damaged()
    {
        isDamaged = true;
        // 무적상태
        invincibility = true;
        // localScale반대로 이동
        rigid.velocity = new Vector2(KnockBackPower * Mathf.Sign(MoveForward) * -transform.localScale.x, 0f);
        
        yield return new WaitForSeconds(KnockBack);
        isDamaged = false;

        yield return new WaitForSeconds(invincibilityTime);
        invincibility = false;
        print("무적해제");

    }


    // 2D라서 2D 사용
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            print(collision.gameObject.tag);
            isJump = false;
            jumpCount = jumpCountBase;
        }
    }
}
