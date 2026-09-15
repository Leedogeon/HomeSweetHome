using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoom : MonoBehaviour
{
    [SerializeField] Player player;
    Rigidbody2D rigid;
    float bulletSpeed = 40.0f;
    Vector2 direction;
    bool scaleChg = true;
    float deleteTimer = .2f;

    [SerializeField] GameObject spreadbullet;
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
            rigid.velocity = Vector2.zero;
            /*if(scaleChg)
            {
                StartCoroutine(ScaleUp(gameObject));
            }*/
            SpreadBullet(collision.transform);
        }
    }

    public IEnumerator ScaleUp(GameObject target)
    {
        scaleChg = false;
        Vector3 startScale = target.transform.localScale;
        Vector3 endScale = startScale * 4f;
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration; // 0 → 1
            target.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // 마지막에 정확히 맞춰주기 (부동소수점 보정)
        target.transform.localScale = endScale;

        yield return new WaitForSeconds(deleteTimer);
        Destroy(gameObject);
    }

    public void SpreadBullet(Transform spot)
    {
        Destroy(gameObject);
        GameObject bullet = Instantiate(spreadbullet,spot.transform.position, Quaternion.identity);

        Destroy(bullet, 2f);
    }
}
