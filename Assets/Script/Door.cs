using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public Player player;
    public GameObject nextDoor;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
    }


    public void OpenDoor()
    {
        anim.SetBool("isOpen", true);

        if (nextDoor != null)
        {
            Animator dooranim = nextDoor.GetComponent<Animator>();
            if(dooranim != null)
            {
                dooranim.SetBool("isOpen", true);
                nextDoor.layer = 25;
            }
        }
        
    }
}
