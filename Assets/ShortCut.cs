using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCut : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] GameObject ShortCutPortal;
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
        ShortCutPortal.SetActive(true);
    }
}
