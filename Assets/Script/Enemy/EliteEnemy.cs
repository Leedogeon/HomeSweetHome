using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : Enemy
{
    [SerializeField] private GameObject Bullet;
    private Transform BulletShoot;
    private float Walkspeed = 5f;
    bool CanAttack = true;
    [SerializeField] GameObject Door;
    protected override void Start()
    {
        base.Start();
        Hp = 5;
        isMove = true;
        BulletShoot = transform.GetChild(0);
    }

    public override void DetectPlayer()
    {
        player = Physics2D.OverlapCircle(transform.position, detectRadius * 2.5f, playerLayer);

        if (player != null)
        {
            InArea = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        if(Hp <= 0 && Door !=null)
        {
            Animator DoorAnim =  Door.GetComponent<Animator>();
            if (DoorAnim != null)
            {
                DoorAnim.SetBool("isOpen", true);
                Door.layer = 25;
            }
        }

        if (player != null)
        {
            anim.SetBool("IsFind", true);

            // 플레이어 방향 계산
            direction = (player.transform.position - transform.position).normalized;
            direction.y = 0f;

            if (isMove && !anim.GetBool("Death"))
                rigid.velocity = direction * Walkspeed;

            if (direction.x < 0) // 만약 플레이어가 왼쪽에 있으면
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip
            }
            else if (direction.x > 0) // 만약 플레이어가 오른쪽에 있으면
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip back
            }
        }
        else
        {
            anim.SetBool("IsFind", false);
        }

        if (InArea && CanAttack)
        {
            
            StartCoroutine(RangeAttack());
        }


    }

    IEnumerator RangeAttack()
    {
        CanAttack = false;
        GameObject Bullets = Instantiate(Bullet,BulletShoot.position,Quaternion.identity);

        float time = 0f;
        float delay = 1f;
        while(time <= delay)
        {
            rigid.velocity = Vector2.zero;
            time+= Time.deltaTime;
            yield return null;
        }

        anim.SetBool("CanAttack", false);
        yield return new WaitForSeconds(2f);
        Destroy(Bullets);
        anim.SetBool("CanAttack", true);
        CanAttack = true;
    }
}
