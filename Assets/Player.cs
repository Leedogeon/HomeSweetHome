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
        // �¿� �̵���
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
        // �̵�, ������ �ӵ� ����
        rigid.velocity = new Vector2(MoveForward * (isJump? speed/3:speed), rigid.velocity.y);
    }
    void Jump()
    {
        // ����
        isJump = true;
        rigid.velocity = Vector2.up*jumpPower;
    }
    /*void Dash()
    {
        DashStart = Time.time;
        // �뽬, ���� �����̵� ���·� ����
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

        // ���� ��ġ�� ��ǥ ��ġ ����
        Vector2 startPos = transform.position;
        Vector2 targetPos = new Vector2(transform.position.x + (DashPower * Mathf.Sign(MoveForward)), transform.position.y);

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
        isDash = false;
    }
}
