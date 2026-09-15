using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Rigidbody2D rigid;
    public Transform StartPos;
    public Transform EndPos;

    public bool isUp = true;
    public bool isDown = false;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(transform.position.y < StartPos.position.y && isUp)
        {
            goUP();
        }
        else if(transform.position.y >= StartPos.position.y && isUp)
        {
            rigid.velocity = Vector2.zero;
            isUp = false;
            isDown = true;
        }

        if (transform.position.y > EndPos.position.y && isDown)
        {
            goDown();
        }
        else if (transform.position.y <= EndPos.position.y && isDown)
        {
            rigid.velocity = Vector2.zero;
            isUp = true;
            isDown = false;
        }

    }
    void goUP()
    {
        rigid.velocity = new Vector2(0, 10f);
    }

    void goDown()
    {
        rigid.velocity = new Vector2(0, -5f);
    }
}
