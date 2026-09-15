using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    public bool BossStart = false;
    public float Hp = 15f;
    [SerializeField] Player player;
    [SerializeField] SpreadBullet spreadBullet;
    [SerializeField] PoisonAttack poisonAttack;
    [SerializeField] GameObject BulletBig;
    [SerializeField] GameObject BulletCenter;
    [SerializeField] GameObject ElectricObj;
    [SerializeField] GameObject ElectricPrev;
    [SerializeField] GameObject ElectricPos;
    [SerializeField] GameObject LeftArm;
    [SerializeField] GameObject LeftGun;
    [SerializeField] GameObject RightArm;
    [SerializeField] GameObject RightGun;
    [SerializeField] GameObject Center;
    [SerializeField] GameObject CenterGun;
    [SerializeField] GameObject BlackHole;
    [SerializeField] Transform[] BlackHolePos;

    [SerializeField] GameObject ShootEffect;

    [SerializeField] GameObject black;
    [SerializeField] GameObject core;
    [SerializeField] Transform coreStart;
    [SerializeField] Transform coreEnd;

    [SerializeField] BossHPUI bossHP;

    [SerializeField] GameObject[] Floors;
    float ShootTimer;
    float BoomTimer;
    float ElecTimer;
    public float PoisionTimer;
    float BlackHoleTimer;

    bool isShoot = true;
    bool isCenterShoot = true;
    bool ElecOn = true;
    bool isSpread = true;
    public bool isPoison = true;
    bool isBlack = true;

    bool bossPause = false;
    float stopStack = 5f;
    float attackStack = 0f;

    bool[] Attacks;
    bool[] Fileds;
    List<int> arr;
    float colorTimer = 0.4f;
    float stack = 1f;
    float Timer = 0f;

    float AttackTimer = 4f;
    float AttackCoolDown = 0f;
    float fieldTimer = 7f;
    float fieldCoolDown = 0f;
    SpriteRenderer Rg;
    SpriteRenderer Lg;
    SpriteRenderer Cg;

    bool CanHeal = true;


    GameObject[] ShootPoint;

    [SerializeField] CameraChange cameraChange;
    private void Start()
    {
        Rg = RightGun.GetComponent<SpriteRenderer>();
        Lg = LeftGun.GetComponent<SpriteRenderer>();
        Cg = CenterGun.GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        spreadBullet = GetComponent<SpreadBullet>();
        poisonAttack = GetComponent<PoisonAttack>();
        ShootPoint = new GameObject[] { RightGun, LeftGun };
    }
    private void Update()
    {
        if (Hp <= 0)
        {
            StartCoroutine(Death());
        }
        if (attackStack == stopStack)
        {
            bossPause = true;
            StartCoroutine(bossStop());
        }
        if (player == null)
        {
            bossPause = true;
            attackStack = 0;
            if(CanHeal)
                StartCoroutine(Heal());
        }
        else bossPause = false;

        if (BossStart && !bossPause)
        {
            AttackCoolDown += Time.deltaTime;
            fieldCoolDown += Time.deltaTime;
            if (fieldTimer <= fieldCoolDown)
            {
                Fileds = new bool[] { ElecOn, isPoison, isBlack };
                List<int> fieldIndex = new List<int>();
                for (int i = 0; i < Fileds.Length; i++)
                {
                    if (Fileds[i])
                    {
                        fieldIndex.Add(i);
                    }
                }

                int selected2 = -1;
                if (fieldIndex.Count > 0)
                {
                    int randomIndex = Random.Range(0, fieldIndex.Count);
                    selected2 = fieldIndex[randomIndex];
                }

                if (selected2 == 0)
                {
                    StartCoroutine(Electric());
                }
                else if (selected2 == 1)
                {
                    isPoison = false;
                    if (player != null)
                        poisonAttack.Poison(player.transform);
                    StartCoroutine(PoisionTime());
                }
                else if (selected2 == 2)
                {
                    int randomindex = Random.Range(0, 4);
                    Transform pos = BlackHolePos[randomindex];
                    StartCoroutine(blackHole(pos));
                }
                fieldCoolDown = 0f;
            }



            if (AttackTimer <= AttackCoolDown)
            {
                Attacks = new bool[] { isShoot, isCenterShoot, isSpread };
                List<int> trueIndexes = new List<int>();

                for (int i = 0; i < Attacks.Length; i++)
                {
                    if (Attacks[i])
                    {
                        trueIndexes.Add(i);
                    }
                }

                int selected = -1;
                if (trueIndexes.Count > 0)
                {
                    int randomIndex = Random.Range(0, trueIndexes.Count);
                    selected = trueIndexes[randomIndex];
                }


                if (selected == 0)
                {
                    attackStack++;
                    StartCoroutine(ArmAttack());
                }
                else if (selected == 1)
                {
                    attackStack++;
                    StartCoroutine(CenterAttack());
                }
                else if (selected == 2)
                {
                    attackStack++;
                    int randomInedx = Random.Range(0, 2);
                    if (player != null)
                        spreadBullet.Shoot(player.transform, ShootPoint[randomInedx]);
                }
                AttackCoolDown = 0f;

            }
        }


    }

    IEnumerator Heal()
    {
        CanHeal = false;
        if (Hp + 3f > 15f)
        {
            Hp = 15f;
        }
        else Hp += 3f;
        yield return new WaitForSeconds(2f);
        CanHeal = true;
    }
    IEnumerator Death()
    {
        Cg.color = new Color(0f, 0f, 0f, 1f);
        Vector3 startScale = Cg.transform.localScale;
        Vector3 endScale = startScale * 5f;
        float duration = 2f;
        float timer = 0f;
        while(duration > timer)
        {
            timer += Time.deltaTime;
            float t = timer / duration; // 0 ¡æ 1
            Cg.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        cameraChange.BossDeath();
        Destroy(gameObject);
    }
    public void PlayerAwake()
    {
        player = FindObjectOfType<Player>();
    }
    public void TakeDamaged()
    {
        Hp -= 1;
    }
    IEnumerator bossStop()
    {
        attackStack = 0f;
        float wait = 3f;
        yield return new WaitForSeconds(wait);
        black.SetActive(true);
        float timer = 0f;
        float duration = 1.5f;

        while(duration > timer)
        {
            timer += Time.deltaTime;
            float t = timer/ duration;
            core.transform.position = Vector3.Lerp(coreStart.transform.position, coreEnd.transform.position, t);
            yield return null;
        }

        bossHP.HPON();

        float attackTime = 3f;
        yield return new WaitForSeconds(attackTime);

        bossHP.HPON();

        timer = 0f;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            core.transform.position = Vector3.Lerp(coreEnd.transform.position, coreStart.transform.position, t);
            yield return null;
        }
        
        bossPause = false;
        black.SetActive(false);
    }

    IEnumerator ArmAttack()
    {
        
        GameObject REffect = Instantiate(ShootEffect);
        GameObject LEffect = Instantiate(ShootEffect);
        REffect.transform.position = RightGun.transform.position;
        LEffect.transform.position = LeftGun.transform.position;
        Destroy(REffect,2.5f);
        Destroy(LEffect, 2.5f);
        yield return new WaitForSeconds(2.5f);

        if (isShoot)
            {
                StartCoroutine(Shoot(LeftArm));
                StartCoroutine(Shoot(RightArm));
            }
        
            
    }

    IEnumerator PoisionTime()
    {

        PoisionTimer = Random.Range(5, 8);

        yield return new WaitForSeconds(PoisionTimer);
        isPoison = true;
    }

    IEnumerator blackHole(Transform pos)
    {
        isBlack = false;
        GameObject hole = Instantiate(BlackHole,pos);

        Destroy(hole, 3f);

        BlackHoleTimer = Random.Range(5,8);

        yield return new WaitForSeconds(BlackHoleTimer);
        isBlack = true;
    }

    IEnumerator CenterAttack()
    {
        isCenterShoot = false;

        float timer = 0f;
        float duration = 2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float red = Mathf.Lerp(1f, 0f, t);
            Cg.color = new Color(red, 0f, 0f, 1f);

            yield return null;
        }
        Cg.color = new Color(0f, 0f, 0f, 1f);

        StartCoroutine(Boom());

    }


    IEnumerator Electric()
    {
        foreach (GameObject g in Floors)
        {
            g.SetActive(true);
        }
        ElecOn = false;
        GameObject prevObj = Instantiate(ElectricPrev, ElectricPos.transform.position, ElectricPrev.transform.rotation);

        float duration = .3f;
        float elec = 1f;
        yield return new WaitForSeconds (duration);
        Destroy(prevObj);
        yield return new WaitForSeconds (elec);

        GameObject Elec = Instantiate(ElectricObj, ElectricPos.transform.position, ElectricObj.transform.rotation);
        Destroy(Elec, 3f);
        
        ElecTimer = Random.Range(5, 8);

        yield return new WaitForSeconds(ElecTimer);
        foreach (GameObject g in Floors)
        {
            g.SetActive(false);
        }
        ElecOn = true;
    }


    IEnumerator Shoot(GameObject Arm)
    {
        isShoot = false;
        GameObject NewBullet = Instantiate(BulletBig, Arm.transform.position, BulletBig.transform.rotation);

        ShootTimer = Random.Range(3,6);
        yield return new WaitForSeconds(ShootTimer);
        isShoot = true;
    }
    IEnumerator Boom()
    {
        GameObject NewBoom = Instantiate(BulletCenter, Center.transform.position, BulletCenter.transform.rotation);

        BoomTimer = Random.Range(5,8);
        yield return new WaitForSeconds(BoomTimer);
        isCenterShoot = true;
        Cg.color = new Color(1f, 0f, 0f, 1f);
    }
}
