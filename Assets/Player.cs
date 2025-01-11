using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    float MoveForward;
    float MoveUp;
    float speed = 5f;

    bool isJump = false;
    float jumpPower = 5f;
    // ���� ī��Ʈ
    int jumpCountBase = 1;
    // �ΰ��� ���� ī��Ʈ
    int jumpCount;
    // �������� ȹ�� �� ���� ī��Ʈ ����
    bool doubleJump = false;

    bool isDash = false;
    float DashPower = 3f;
    float DashTime = .3f;
    float DashCoolDown = 1f;
    float DashCoolCheck;
    // �뽬��
    Vector2 startPos;
    Vector2 targetPos;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
        
        if(DashCoolCheck >= 0 )
        {
            DashCoolCheck -= Time.deltaTime;
        }
        if (DashCoolCheck <= 0) isDash = false;
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
        if (Input.GetButtonDown("Dash") && !isDash)
        {
            StartCoroutine(Dash());
        }
        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
    void Move()
    {
        // �̵�, ������ �ӵ� ����
        rigid.velocity = new Vector2(MoveForward * (isJump? speed/3:speed), rigid.velocity.y);
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
        DashCoolCheck = DashCoolDown;
        // ���� ��ġ�� ��ǥ ��ġ ����
        Vector2 startPos = transform.position;
        Vector2 targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y);
        if (MoveUp != 0 && MoveForward!=0) targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y + (DashPower * Mathf.Sign(MoveUp)));
        
        float elapsedTime = 0f;

        // �뽬 �ִϸ��̼� ó��
        while (elapsedTime < DashTime)
        {
            
            elapsedTime += Time.deltaTime;

            // Lerp�� ����Ͽ� �뽬 �߰� ��ġ ���
            transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / DashTime);
            yield return null; // ���� �����ӱ��� ���
        }

        // �뽬 ���� �� ��ġ ����
        transform.position = targetPos;
    }
    void Attack()
    {
        print("Attack");
    }

    // 2D�� 2D ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("test");
        if (collision.gameObject.tag == "Floor")
        {
            print(collision.gameObject.tag);
            isJump = false;
            jumpCount = jumpCountBase;
        }
    }
}
