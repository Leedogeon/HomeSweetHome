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
    bool isDash = false;
    float DashPower = 3f;
    float DashTime = .3f;
    float DashCoolDown = 1f;
    Vector2 startPos;
    Vector2 targetPos;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    
    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        
    }
    void GetInput()
    {
        // 좌우 이동값
        MoveForward = Input.GetAxis("Horizontal");
        MoveUp = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Dash") && !isDash)
        {
            StartCoroutine(Dash());
        }
    }
    void Move()
    {
        // 이동, 점프시 속도 감소
        rigid.velocity = new Vector2(MoveForward * (isJump? speed/3:speed), rigid.velocity.y);
    }
    void Jump()
    {
        // 점프
        isJump = true;
        rigid.velocity = Vector2.up*jumpPower;
    }
    /*void Dash()
    {
        DashStart = Time.time;
        // 대쉬, 현재 순간이동 상태로 구현
        isDash = true;
        print("dash");
        //transform.position += transform.right * DashPower;
        startPos = transform.position;
        targetPos = new Vector2 (startPos.x + DashPower,startPos.y);
        float t = (Time.time - DashTime) / DashTime;
        if(t > 1f)
        {
            t = 1f;
        }
        Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
        transform.position = newPos;
        isDash = false;
    }*/
    IEnumerator Dash()
    {
        isDash = true;

        // 시작 위치와 목표 위치 설정
        Vector2 startPos = transform.position;
        Vector2 targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y);

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
        isDash = false;
    }
}
