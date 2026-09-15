using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator anim;
    public int Hp;
    public GameObject DamagedEffect;
    public GameObject SpawnedEffect;
    public float detectRadius = 20f; // 탐지 반지름
    public LayerMask playerLayer; // 플레이어 레이어 지정
    public bool InArea = false;
    protected Vector2 direction;
    protected Collider2D player;
    public AudioClip DamagedSound;
    public bool isMove = true;
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        DamagedEffect = Resources.Load<GameObject>("Effect/Hit");
        DamagedSound = Resources.Load<AudioClip>("Sounds/Enemy_Damaged (mp3cut.net)");
    }

    protected virtual void Update()
    {
        if(anim != null)
            anim.SetInteger("Hp", Hp);

        if (Hp <= 0)
        {
            if(gameObject.layer == 12)
            {
                gameObject.layer = 13;
            }
            anim.SetBool("Death",true);
        }
        DetectPlayer();
    }

    public virtual void DetectPlayer()
    {
        player = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);

        if (player != null)
        {
            Debug.Log("플레이어 감지됨! " + player.name);
            InArea = true;
        }
    }



    public virtual void TakeDamaged(int damage, Vector2 scale)
    {
        if (Hp <= 0) return;
        SoundManager.instance.SFXPlay("Attack", DamagedSound);
        rigid.velocity = Vector2.zero;
        DEffect(scale);
        anim.SetTrigger("Damaged");
        Hp -= damage;
        rigid.velocity = new Vector2 (2.5f * scale.x, 0);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void DEffect(Vector2 scale)
    {
        SpawnedEffect = Instantiate(DamagedEffect);
        if(scale.x < 0)
        {
            SpawnedEffect.transform.localScale = scale;
        }
        SpawnedEffect.transform.position = transform.position + new Vector3(scale.x*2f,2.5f,0f) ;
        SpawnedEffect.transform.SetParent(transform);
        Destroy(SpawnedEffect,1f);
    }
}
