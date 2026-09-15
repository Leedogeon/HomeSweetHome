using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private GameObject Bullet;
    private Transform BulletShoot;
    float ShootTimer = 1f;
    bool isShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        Bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        BulletShoot = transform.Find("BulletShoot");
    }

    // Update is called once per frame
    void Update()
    {
        if(isShoot)
        {
            StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
        isShoot = false;
        GameObject NewBullet = Instantiate(Bullet, BulletShoot.transform.position, Bullet.transform.rotation);
        yield return new WaitForSeconds(ShootTimer);
        isShoot = true;

    }
}
