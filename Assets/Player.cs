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
    // ������
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;

    bool isJump = false;
    float jumpPower = 5f;
    // ���� ī��Ʈ
    int jumpCountBase = 1;
    // �ΰ��� ���� ī��Ʈ
    int jumpCount;
    // �������� ȹ�� �� ���� ī��Ʈ ����
    bool doubleJump = false;


    // ����
    float AttackTime = 0.2f;
    // �뽬��
    bool isDash = false;
    bool canDash = true;
    float DashPower = 10f;
    float DashTime = .2f;
    float DashCoolDown = 1f;
    TrailRenderer tr;

    // �ǰݽ�
    bool isDamaged = false;
    // �˹�Ÿ�, �̵��ð�
    float KnockBackPower = 3f;
    float KnockBack = .5f;
    bool isKnockBack = false;
    // �����ð�
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
        // �¿� �̵���
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
        // flip�� �̿����� scale�� �̿����� �����ؾߵ�
        /*if (MoveForward > 0) spriteRenderer.flipX = false;
        else if (MoveForward < 0) spriteRenderer.flipX = true;*/
        // �̵�, ������ �ӵ� ����
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
        // ����
        isJump = true;
        rigid.velocity = Vector2.up*jumpPower;
    }
    IEnumerator Dash()
    {
        isDash = true;
        canDash = false;
        // �뽬�� �߷¿��� ����
        float gravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        
        // �밢�뽬, �Ϲݴ뽬
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
        // ��������
        invincibility = true;
        // localScale�ݴ�� �̵�
        rigid.velocity = new Vector2(KnockBackPower * Mathf.Sign(MoveForward) * -transform.localScale.x, 0f);
        
        yield return new WaitForSeconds(KnockBack);
        isDamaged = false;

        yield return new WaitForSeconds(invincibilityTime);
        invincibility = false;
        print("��������");

    }


    // 2D�� 2D ���
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
