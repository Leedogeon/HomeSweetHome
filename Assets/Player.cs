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
    // 점프 카운트
    int jumpCountBase = 1;
    // 인게임 점프 카운트
    int jumpCount;
    // 더블점프 획득 시 점프 카운트 변경
    bool doubleJump = false;

    bool isDash = false;
    float DashPower = 3f;
    float DashTime = .3f;
    float DashCoolDown = 1f;
    float DashCoolCheck;
    // 대쉬용
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
        // 좌우 이동값
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
        // 이동, 점프시 속도 감소
        rigid.velocity = new Vector2(MoveForward * (isJump? speed/3:speed), rigid.velocity.y);
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
        DashCoolCheck = DashCoolDown;
        // 시작 위치와 목표 위치 설정
        Vector2 startPos = transform.position;
        Vector2 targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y);
        if (MoveUp != 0 && MoveForward!=0) targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y + (DashPower * Mathf.Sign(MoveUp)));
        
        float elapsedTime = 0f;

        // 대쉬 애니메이션 처리
        while (elapsedTime < DashTime)
        {
            
            elapsedTime += Time.deltaTime;

            // Lerp를 사용하여 대쉬 중간 위치 계산
            transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / DashTime);
            yield return null; // 다음 프레임까지 대기
        }

        // 대쉬 종료 후 위치 고정
        transform.position = targetPos;
    }
    void Attack()
    {
        print("Attack");
    }

    // 2D라서 2D 사용
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
