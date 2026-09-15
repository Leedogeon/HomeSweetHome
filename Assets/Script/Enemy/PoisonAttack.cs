using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonAttack : MonoBehaviour
{
    [SerializeField] private GameObject PoisonSocket;
    [SerializeField] private GameObject StartPos;
    [SerializeField] private float Fall = 0.5f;
    [SerializeField] private float FallSpeed;
    [SerializeField] private GameObject PoisonPrefeb;
    Rigidbody2D rigid;
    private bool isFall = false;


    private void Update()
    {
     
        if(isFall)
        {
            if(rigid != null)
                rigid.velocity = new Vector2(0f, FallSpeed * Time.deltaTime);
        }
        else if(!isFall)
        {
            if (rigid != null)
                rigid.velocity = Vector2.zero;
        }
    }
    public void Poison(Transform target)
    {
        GameObject Socket = Instantiate(PoisonSocket);
        Socket.transform.position = new Vector2(target.position.x, StartPos.transform.position.y);
    }

}
