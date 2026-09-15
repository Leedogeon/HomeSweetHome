using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private LayerMask TargetLayer;
    public ParticleSystem ps;
    public bool isFront = true;
    public AudioClip clip;
    private void Start()
    {
        if(this.GetComponent<ParticleSystem>() != null)
        {
            ps = this.GetComponent<ParticleSystem>();

        }
        Player = Initialize.Instance.GetPlayerInstance().GetComponent<Player>();

        if (Player.transform.localScale.x > 0)
            isFront = true;
        else isFront = false;

    }
    private void Update()
    {
        
        transform.position += new Vector3(.15f*(isFront? 1:-1), 0, 0);


        Collider2D hit = Physics2D.OverlapCircle(transform.position, .5f, TargetLayer);
        if (hit != null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
     
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamaged(2,transform.localScale);
        }
        if(other.gameObject.tag == "BossAttack")
        {
            other.GetComponent<SocketControl>().Hp -= 1;
        }

    }


    void soundTest()
    {
        SoundManager.instance.SFXPlay("Skill",clip);
    }
}
