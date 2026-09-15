using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.tag == "Player" || other.gameObject.tag == "Platform" || other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
