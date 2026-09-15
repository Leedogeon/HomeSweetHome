using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    protected float bulletSpeed = 10.0f;
    Vector2 direction;
    Player player;
    protected virtual void Start()
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

        // 5초뒤 제거
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
