using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketControl : MonoBehaviour
{
    public int Hp = 2;
    Rigidbody2D rigid;
    bool isFall = false;
    float doFall = 0.5f;
    float spread = 3f;
    float stop = 3f;
    bool start = false;
    [SerializeField] GameObject PoisonPrefab;
    GameObject PoisonBox;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(Attack());
    }


    private void Update()
    {
        
        if (isFall)
        {
            rigid.velocity = new Vector2(0f, -100f);
        }
        else if(!isFall)
        {
            rigid.velocity = Vector2.zero;
            
        }

        if(Hp <=0)
        {
            Destroy(gameObject);
            if(PoisonBox!= null)
            {
                Destroy(PoisonBox);
            }
        }
    }


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(doFall);
        isFall = true;
    }

    IEnumerator PoisonSpread()
    {
        yield return new WaitForSeconds (spread);

        PoisonBox = Instantiate(PoisonPrefab);
        PoisonBox.transform.position = gameObject.transform.position + new Vector3(0,-2f);

        Vector3 startScale = PoisonBox.transform.localScale;
        Vector3 endScale = startScale * 1.2f;
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration; // 0 ¡æ 1
            PoisonBox.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(stop);
        Destroy(PoisonBox);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 18)
        {
            isFall = false;
            StartCoroutine(PoisonSpread());
        }
    }
}
