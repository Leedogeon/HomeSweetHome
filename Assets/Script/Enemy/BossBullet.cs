using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] Player player;
    Rigidbody2D rigid;
    float bulletSpeed = 35.0f;
    Vector2 direction;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            // 플레이어 방향 계산
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            Debug.LogWarning("플레이어를 찾을 수 없음!");
            direction = Vector2.left; // 기본 방향 (예: 왼쪽)
        }

        rigid.velocity = direction * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //rigid.velocity = new Vector2(-bulletSpeed, 0);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Destroy(gameObject);
        }

    }


}
