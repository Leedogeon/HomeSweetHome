
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

// 빌드오류때문에 제거, 조건문 추가
//using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.U2D;
#endif
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Transform TP;
    Rigidbody2D rigid;
    Animator anim;
    public Collider2D playerCollider;

    public float MaxHp = 5;
    public float Hp { get; private set; } = 5;
    public bool HasKey = false;
    public bool AtKey = false;

    private int defaultLayer;
    private int dashLayer;
    private int jumpLayer;
    private int UpLayer;

    #region effect
    public GameObject RunEffect;
    private GameObject SpawnRunEffect;
    public GameObject jumpEffect;
    private GameObject jumpEffectPos;
    public GameObject fallEffect;
    private GameObject fallEffectPos;
    public GameObject AttackEffect;
    private GameObject SpawnedAttackEffect;
    public GameObject SkillEffect;
    public GameObject SkillEffectLeft;
    private GameObject SpawnedSkillEffect;
    public GameObject BlockEffect;
    private GameObject SpawnedBlockEffect;

    public ParticleSystem ps;
    private SphereCollider hitcol;

    #endregion

    #region move
    float MoveForward;
    float MoveUp;
    //bool isMove = false;
    float speed = 15f;


    RaycastHit2D hit;
    RaycastHit2D Bhit;
    [SerializeField] Transform footPosition;
    [SerializeField] Transform SlopeCheckPos;
    [SerializeField] Transform backPos;
    [SerializeField] float Raydistance;
    [SerializeField] float angle;
    [SerializeField] Vector2 perp;
    bool isSlope = false;
    [SerializeField] LayerMask SlopeMask;
    // 뒤집기
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;
    #endregion

    #region jump
    /*[SerializeField] private LayerMask platformLayer;*/
    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    [SerializeField] public float groundCheckRadius = 3f;

    public LayerMask GroundLayer;
    public LayerMask FloorLayer;
    public LayerMask PosinLayer;
    public List<Collider2D> currentPlatforms = new List<Collider2D>();
    private bool isDropping = false;

    bool isJump = false;
    bool isFall = false;
    bool canJump = true;
    bool isDownJump = false;
    public float downJumpTime = 0f;
    public float jumpPower = 25;
    public float jumpTimerLimit = .2f;
    float jumpTimer = 0f;
    // 점프 카운트
    int jumpCountBase = 1;
    // 인게임 점프 카운트
    int jumpCount;
    // 더블점프 획득 시 점프 카운트 변경
    bool doubleJump = false;

    bool DropCheck = true;

    #endregion

    // 맵이동
    GameObject curPortal;
    bool isPort = false;
    bool isSpawn = false;
    bool isDoor = false;
    bool isShortCut = false;
    // 공격
    public bool isAttack = false;  // 직접 접근을 막음
    private bool canSkill = true;
    public bool isSkill = false;
    private float skillTime = 0.5f;
    private float skillCoolDown = 2.5f;

    // 대쉬용
    bool isDash = false;
    bool canDash = true;
    float DashPower = 45f;
    float DashTime = .2f;
    float DashCoolDown = 1f;


    //TrailRenderer tr;

    // 피격시
    bool isDamaged = false;

    // 방어
    bool isBlock = false;
    float BlockTime = .5f;
    public float BlockCoolDown = 1f;
    bool canBlock = true;

    // 넉백거리, 이동시간
    //float KnockBackPower = 10f;
    float KnockBack = .5f;
    //bool isKnockBack = false;
    // 무적시간
    float invincibilityTime = .75f;
    bool invincibility = false;

    //[SerializeField] private UI keyBoardUI; // Inspector에서 연결
    private Portal portal;
    private Door door;
    private SkillUI skillUI;

    public AudioClip clip;
    public AudioClip clip2;
    void Start()
    {
        Hp = MaxHp;
        playerCollider = GetComponent<Collider2D>();
        defaultLayer = LayerMask.NameToLayer("Player");
        dashLayer = LayerMask.NameToLayer("DashLayer");
        jumpLayer = LayerMask.NameToLayer("JumpLayer");
        UpLayer = LayerMask.NameToLayer("UpLayer");
        groundCheck = transform.Find("GroundCheck");

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //AttackEffect = Resources.Load<GameObject>("Effect/Test_1");

        if (doubleJump)
        {
            jumpCountBase = 2;
        }
        jumpCount = jumpCountBase;
        //keyBoardUI = GameObject.Find("KeyBoardUI")?.GetComponent<UI>();
        portal = FindObjectOfType<Portal>();
        door = FindObjectOfType<Door>();
        skillUI = FindObjectOfType<SkillUI>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Initialize.Instance.isPaused) return;

        isSlope = IsSlope();
        if (Hp <= 0)
        {
            if (Initialize.Instance != null)
            {
                Initialize.Instance.Death();
            }
        }

        if (KeySetting.keys.Count > 0)
        {
            GetInput();
            #region grounded

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, GroundLayer | FloorLayer | PosinLayer);

            #endregion

            if (!isAttack)
            {
                Move();
                Flip();
            }

            CheckColliderOverlap();

            if (rigid.velocity.y == 0 && (isGrounded ||isSlope ))
            {
                canJump = true;
                isDropping = false;
            }


            if (rigid.velocity.y < 0 && !isSlope)
            {
                isFall = true;
                isJump = false;
            }
            else isFall = false;
        }
    }


    void GetInput()
    {
        // 좌우 이동값
        MoveForward = Input.GetAxis("Horizontal");
        bool isRun = MoveForward != 0;
        anim.SetBool("isMove",  isRun && !isJump && !isFall);
        anim.SetBool("isJump", isJump && !isSlope);
        anim.SetBool("isFall", isFall && !isSlope);
        //        anim.SetBool("isAttack", isAttack);
        MoveUp = Input.GetAxis("Vertical");

        // 디버그용 4번맵 이동
/*        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.position = TP.transform.position;
            portal.TP();
        }*/


        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]) && Input.GetKey(KeySetting.keys[KeyAction.DOWN]))
        {
            DropDown();
        }
 
        if (!isDropping && Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]) && canJump && !isDash)
        {

            jumpEffectPos = Instantiate(jumpEffect);
            jumpEffectPos.transform.position = groundCheck.position;
            Destroy(jumpEffectPos, .5f);
            jumpEffectPos = null;
            canJump = false;
            isJump = true;
            jumpTimer = 0f;
            //jumpCount--;
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.JUMP]) && jumpTimer < jumpTimerLimit)
        {
            Jump();
        }
        if (Input.GetKeyUp(KeySetting.keys[KeyAction.JUMP]))
        {
            jumpTimer = float.MaxValue;
        }


        if (Input.GetKeyDown(KeySetting.keys[KeyAction.DASH]) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.ATTACK]) && !isAttack && !isSkill && !isBlock && !isDamaged)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.SKILL]) && canSkill && !isAttack && !isBlock)
        {
            StartCoroutine(Skill());
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.BLOCK]) && canBlock && canSkill && !isAttack)
        {
            StartCoroutine(Block());
        }


        if (Input.GetKeyDown(KeySetting.keys[KeyAction.OPEN]))
        {
            if (isPort || isSpawn)
            {
                NextMap();

            }
            if (AtKey)
            {
                HasKey = true;

            }
            if (isDoor && HasKey)
            {
                door.OpenDoor();
            }
            if(isShortCut)
            {
                transform.position = TP.transform.position;
                portal.TP();
            }
        }
    }
    void Move()
    {
        // flip을 이용할지 scale을 이용할지 선택해야됨
        /*if (MoveForward > 0) spriteRenderer.flipX = false;
        else if (MoveForward < 0) spriteRenderer.flipX = true;*/
        // 이동, 점프시 속도 감소
        if (!isDash && !isDamaged)
        {
            if (isSlope && angle < 45f && !isJump && angle!=0)
            {
                rigid.velocity = perp * speed * MoveForward * -1f;
            }
            else
            {
                if (isSkill) rigid.velocity = Vector2.zero;
                else rigid.velocity = new Vector2(MoveForward * (isJump ? speed / 1.1f : speed), rigid.velocity.y);
            }

        }
        


    }
    public bool IsSlope()
    {
        hit = Physics2D.CircleCast(SlopeCheckPos.position, .2f, Vector2.down, Raydistance, SlopeMask);
        angle = Vector2.Angle(hit.normal, Vector2.up);
        perp = Vector2.Perpendicular(hit.normal).normalized;
        //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);

       Bhit = Physics2D.CircleCast(backPos.position, .2f, Vector2.down, 5f, SlopeMask);

        if (Bhit)
        {
            return true;
        }
       return hit;
    }
    protected Vector2 DirectionToSlope(Vector2 direction, RaycastHit2D hit)
    {
        // 경사로의 법선 벡터 (단위 벡터)
        Vector2 slopeNormal = hit.normal.normalized;

        // 경사로 표면의 방향 (법선과 수직)
        Vector2 slopeDirection = new Vector2(-slopeNormal.y, slopeNormal.x);

        // direction을 slopeDirection에 투영 (Dot Product)
        float dot = Vector2.Dot(direction, slopeDirection);

        // slopeDirection을 따라가는 벡터 반환
        return slopeDirection * dot * (isFacingRight ? 1 : -1);
    }
    void Flip()
    {
        if ((isFacingRight && MoveForward < 0f || !isFacingRight && MoveForward > 0f) && !isAttack)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void REffect()
    {
        if (RunEffect != null && SpawnRunEffect == null)
        {

            SpawnRunEffect = Instantiate(RunEffect);
            SpawnRunEffect.transform.position = footPosition.position;
            SpawnRunEffect.transform.SetParent(transform); // 캐릭터를 부모로 설정

            if (transform.localScale.x < 0)
            {
                Vector2 scale = SpawnRunEffect.transform.localScale;
                scale.x *= -1f;
                SpawnRunEffect.transform.localScale = scale;
            }


            Destroy(SpawnRunEffect, 0.5f);
        }
    }

    void Jump()
    {
        //rigid.velocity = Vector2.zero;
        gameObject.layer = jumpLayer;
        if (jumpTimer < jumpTimerLimit)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower / 1.5f); // 점프 유지
        }

        jumpTimer += Time.deltaTime;

    }

    void DropDown()
    {

        Collider2D hitCollider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, FloorLayer);

        
        if(hit || Bhit)
        {
            hitCollider = hit.collider ? hit.collider : Bhit.collider;
        }
        

        if (hitCollider != null)
        {
            isDropping = true;

            if (!currentPlatforms.Contains(hitCollider))
            {
                print(hitCollider.name);
                currentPlatforms.Add(hitCollider);
                Physics2D.IgnoreCollision(playerCollider, hitCollider, true);

            }
        }

    }
    IEnumerator Dash()
    {
        anim.SetTrigger("DashTrigger");
        isDash = true;
        gameObject.layer = dashLayer;
        canDash = false;
        // 대쉬중 중력영향 무시
        float gravity = rigid.gravityScale;
        rigid.gravityScale = 0f;

        skillUI.isDash = true;
        skillUI.DCD.fillAmount = 0;

        // 대각대쉬, 일반대쉬
        if (MoveUp != 0f && !isGrounded)
            rigid.velocity = new Vector2(DashPower * Mathf.Sign(transform.localScale.x), DashPower * Mathf.Sign(MoveUp));
        else
            rigid.velocity = new Vector2(DashPower * Mathf.Sign(transform.localScale.x), 0f);


        //tr.emitting = true;

        yield return new WaitForSeconds(DashTime);

        //tr.emitting = false;
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = gravity;
        isDash = false;
        gameObject.layer = defaultLayer;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;

    }



    public Transform Attackpos;
    public Vector2 boxSize;

    void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Attackpos.position, boxSize, 0);

        foreach (Collider2D obj in collider2Ds)
        {
            if (obj.tag == "Enemy" || obj.gameObject.layer == 12)
            {
                float relativeX = Mathf.Sign(transform.position.x - obj.transform.position.x);
                obj.GetComponent<Enemy>().TakeDamaged(1, transform.localScale * relativeX);
            }
            if (obj.tag == "BossAttack")
            {
                obj.GetComponent<SocketControl>().Hp -= 1;
            }
            if (obj.tag == "BossCore")
            {
                obj.GetComponent<bossCore>().TakeDamaged();
            }
            if (obj.tag == "ShortCutObj")
            {
                obj.GetComponent<ShortCut>().OpenDoor();
            }
        }



        SoundManager.instance.SFXPlay("Attack", clip);
        anim.SetTrigger("AttackTrigger");
        isAttack = true;
        TriggerEffect();
        if (!isJump)
        {
            rigid.velocity = Vector2.zero;
        }
    }
    IEnumerator Skill()
    {
        isSkill = true;
        canSkill = false;
        anim.SetTrigger("SkillTrigger");


        SoundManager.instance.SFXPlay("Attack", clip2);
        skillUI.SCD.fillAmount = 0;
        skillUI.isSkill = true;
        yield return new WaitForSeconds(skillTime);
        
        isSkill = false;
        yield return new WaitForSeconds(skillCoolDown);
        canSkill = true;

    }

    void TriggerEffect()
    {
        if (AttackEffect != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(transform.localScale.x * 7f, 2f, 0);
            SpawnedAttackEffect = Instantiate(AttackEffect, spawnPos, Quaternion.identity);
            SpawnedAttackEffect.transform.SetParent(transform);
            Transform childA1E = SpawnedAttackEffect.transform.Find("Test_A1E");
            Transform childParticle = SpawnedAttackEffect.transform.Find("Particle");

            // 부모 방향 확인 후, 자식들만 반전
            if (childA1E != null && transform.localScale.x < 0)
            {
                Vector2 scale = childA1E.localScale;
                scale.x *= -1;
                childA1E.localScale = scale;
            }

            if (childParticle != null && transform.localScale.x < 0)
            {
                Vector3 scale = childParticle.localScale;
                scale.x *= -1;
                childParticle.localScale = scale;
            }
        }
        else Debug.Log("False");
    }
    void TriggerEffectSkill()
    {
        if (SkillEffect != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(transform.localScale.x * 7f, 0);

            if(transform.localScale.x < 0)
            {
                SpawnedSkillEffect = Instantiate(SkillEffectLeft);
                //SpawnedSkillEffect = Instantiate(SkillEffectLeft, spawnPos, Quaternion.identity);
            }
            else SpawnedSkillEffect = Instantiate(SkillEffect);
            SpawnedSkillEffect.transform.position = spawnPos;
            Destroy(SpawnedSkillEffect, 1.5f);
        }
        else Debug.Log("False");
    }

    public void EndAttack()
    {
        //anim.ResetTrigger("AttackTrigger");
        isAttack = false;
        Destroy(SpawnedAttackEffect);
    }

    public IEnumerator Damaged(float relativeX)
    {
        if (invincibility) yield break;
        if(isBlock)
        {
            yield break;
        }
        Hp -= 1;
        isDamaged = true;
        anim.SetBool("Damaged", true);
        // 무적상태
        invincibility = true;
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, .4f);
        // localScale반대로 이동
        rigid.velocity = new Vector2(4f * relativeX, 0f);

        yield return new WaitForSeconds(KnockBack);
        anim.SetBool("Damaged", false);
        isDamaged = false;
        yield return new WaitForSeconds(invincibilityTime);

        invincibility = false;
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1, 1, 1, 1f);

    }

    IEnumerator Block()
    {
        canBlock = false;
        isBlock = true;
        TBlockEffect();
        skillUI.BCD.fillAmount = BlockCoolDown;
        skillUI.isBlock = true;
        yield return new WaitForSeconds(BlockTime);
        isBlock = false;
        yield return new WaitForSeconds(BlockCoolDown);
        canBlock = true;
        
    }
    void TBlockEffect()
    {
        if(BlockEffect != null)
        {
            SpawnedBlockEffect = Instantiate(BlockEffect);
            SpawnedBlockEffect.transform.position = transform.position;
            Destroy(SpawnedBlockEffect, 1.5f);
            SpawnedBlockEffect.transform.SetParent(transform);
        }
    }
    // 2D라서 2D 사용
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // 발판에 닿아 있다면
        {

            /*            if (rigid.velocity.y > 0) // 위로 밀리는 경우 속도를 0으로
                        {
                            rigid.velocity = new Vector2(rigid.velocity.x, 0);
                        }*/

        }
        if(!invincibility)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                float relativeX = Mathf.Sign(transform.position.x - collision.transform.position.x);

                StartCoroutine(Damaged(relativeX));
            }
        }
        


        if (collision.gameObject.layer == 19)
        {
            gameObject.layer = UpLayer;
        }

    }



    void NextMap()
    {
        if (curPortal != null)
        {
            if (isPort)
            {
                Vector2 newxPos = portal.NextMap(curPortal);
                transform.position = newxPos;
            }

            if (isSpawn)
            {
                Vector2 prevPos = portal.PrevMap(curPortal);
                transform.position = prevPos;
            }

        }

    }
    void CheckColliderOverlap()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Floor", "Slide"));// 검사할 레이어 설정
        Collider2D[] results = new Collider2D[10]; // 결과를 담을 배열

        // OverlapCollider를 사용하여 겹치는 콜라이더들을 배열에 담는다.
        int count = playerCollider.OverlapCollider(contactFilter, results);
        /*for (int i = 0; i < count; i++)
        {
            Collider2D hit = results[i];
            // 겹치는 콜라이더가 있으면 처리
            Debug.Log("Player is touching " + hit.name);
        }*/

        // 탐색이 O(n)이라 HashSet으로 변경
        HashSet<Collider2D> currentHits = new HashSet<Collider2D>(results.Take(count));
        List<Collider2D> remove = new List<Collider2D>();

        // 현재 접촉중인 오브젝트가 하단점프한 오브젝트에 해당하는지 체크하고 없다면 제거할 리스트에 추가
        foreach (Collider2D col in currentPlatforms)
        {
            if (!currentHits.Contains(col))
                remove.Add(col);
        }
        if (remove.Count > 0)
        {
            foreach (Collider2D col in remove)
            {
                // 제거리스트에 저장된 오브젝트가 있는경우 Ignore을 해제하고 리스트에서 제거
                if (currentPlatforms.Contains(col))
                {
                    Physics2D.IgnoreCollision(playerCollider, col, false);
                    currentPlatforms.Remove(col);
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !invincibility)
        {
            float relativeX = Mathf.Sign(transform.position.x - collision.transform.position.x);

            StartCoroutine(Damaged(relativeX));
        }

        if (collision.gameObject.layer == 17 || collision.gameObject.layer == 18)
        {
            /*isDropping = false;*/
            gameObject.layer = defaultLayer;
            isJump = false;
            /*canJump = true;*/
        }



        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            StartCoroutine(Damaged(-1));
            isJump = false;
        }

        if (collision.gameObject.tag == "Portal")
        {
            curPortal = collision.gameObject;
            isPort = true;
        }
        if (collision.gameObject.tag == "Spawn")
        {
            curPortal = collision.gameObject;
            isSpawn = true;
        }
        if (collision.gameObject.tag == "ShortCut")
        {
            isShortCut = true;
        }

        if (collision.gameObject.tag == "Object")
        {
            AtKey = true;
        }
        if (collision.gameObject.tag == "Door")
        {
            isDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            curPortal = null;
            isPort = false;
        }
        if (collision.gameObject.tag == "Spawn")
        {
            curPortal = null;
            isSpawn = false;
        }

        if (collision.gameObject.tag == "Object")
        {
            AtKey = false;
        }
        if (collision.gameObject.tag == "Door")
        {
            isDoor = false;
        }
        if (collision.gameObject.tag == "ShortCut")
        {
            isShortCut = false;
        }
    }
}