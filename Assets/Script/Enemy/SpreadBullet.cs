using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{
    //[SerializeField] private Transform ShootPoint;
    [SerializeField] private GameObject bulletPrefeb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float BulletsCount;
    [SerializeField] private float spreadAngle;
    private float nextFireTime;    


    public void Shoot(Transform TargetPos,GameObject ShootPoint)
    {
        
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = ((Vector2)TargetPos.transform.position - (Vector2)ShootPoint.transform.position).normalized;
            float angleStep = spreadAngle / (BulletsCount - 1);
            float angle = -spreadAngle / 2;

            for (int i = 0; i < BulletsCount; i++)
            {
                float bulletDirX = direction.x * Mathf.Cos(angle * Mathf.Deg2Rad) - direction.y * Mathf.Sin(angle * Mathf.Deg2Rad);
                float bulletDirY = direction.x * Mathf.Sin(angle * Mathf.Deg2Rad) + direction.y * Mathf.Cos(angle * Mathf.Deg2Rad);
                Vector2 BulletDirection = new Vector2(bulletDirX, bulletDirY).normalized;
                GameObject bullet = Instantiate(bulletPrefeb, ShootPoint.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletDirection * bulletSpeed;
                angle += angleStep;

                Destroy(bullet, 3f);
            }   
    }
}
