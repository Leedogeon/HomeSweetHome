using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{

    private GameObject Bullet;
    private Transform BulletShoot;
    float ShootTimer = 3f;
    bool isShoot = true;
    
    protected override void Start()
    {
        base.Start();

        Hp = 2;
        Bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        BulletShoot = transform.Find("BulletShoot");
    }

    protected override void Update()
    {
        if (Hp <= 0) base.Death();
        if (isShoot && InArea)
        {
            StartCoroutine(Shoot());
        }
        DetectPlayer();
    }

    public override void DetectPlayer()
    {
        player = Physics2D.OverlapCircle(transform.position, detectRadius*2.5f, playerLayer);

        if (player != null)
        {
            InArea = true;
        }
        else InArea = false;
    }
    IEnumerator Shoot()
    {
        isShoot = false;
        GameObject NewBullet = Instantiate(Bullet, BulletShoot.transform.position, Bullet.transform.rotation);
        yield return new WaitForSeconds(ShootTimer);
        isShoot = true;

    }

    public override void TakeDamaged(int damage, Vector2 scale)
    {
        DEffect(scale);
        Hp -= damage;
        rigid.velocity = new Vector2(2.5f * scale.x, 0);
    }
}
