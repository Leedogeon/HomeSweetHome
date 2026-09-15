using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌한 오브젝트: " + collision.name);
        if (collision.tag == "Enemy")
        {
            float relativeX = Mathf.Sign(transform.position.x - collision.transform.position.x);
            collision.GetComponent<Enemy>().TakeDamaged(1, transform.localScale * relativeX);
        }
    }
}
