using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public Collider2D playerCollider;

    public GameObject RunEffect;
    private GameObject SpawnRunEffect;
    public GameObject jumpEffect;
    private GameObject jumpEffectPos;
    public GameObject fallEffect;
    private GameObject fallEffectPos;

    private int jumpLayer;

    bool isFacingRight = true;
    public float speed = 15f;
    private float MoveForward;
    private float MoveUp;

    private bool isDropping = false;
    bool canJump;
    bool isJump;
    float jumpTimer;
    bool isDash = false;
    bool canDash = true;
    float DashPower = 45f;
    float DashTime = .2f;
    float DashCoolDown = 1f;
    float jumpTimerLimit = .2f;
    public float downJumpTime = 0f;
    public float jumpPower = 25;

    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    [SerializeField] public float groundCheckRadius = 3f;

    public LayerMask GroundLayer;
    public LayerMask FloorLayer;
    public List<Collider2D> currentPlatforms = new List<Collider2D>();

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        jumpLayer = LayerMask.NameToLayer("JumpLayer");
        groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        MoveForward = Input.GetAxis("Horizontal");
        MoveUp = Input.GetAxis("Vertical");
        Move();
        Flip();
        //Jump();
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, GroundLayer | FloorLayer);
    }

    public void Move()
    {
        // flip을 이용할지 scale을 이용할지 선택해야됨
        /*if (MoveForward > 0) spriteRenderer.flipX = false;
        else if (MoveForward < 0) spriteRenderer.flipX = true;*/
        // 이동, 점프시 속도 감소

        rigid.velocity = new Vector2(MoveForward * speed, rigid.velocity.y);


    }

    public void Flip()
    {
        if ((isFacingRight && MoveForward < 0f || !isFacingRight && MoveForward > 0f) /*&& !isAttack*/)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public IEnumerator Dash()
    {
        anim.SetTrigger("DashTrigger");
        isDash = true;
        //gameObject.layer = dashLayer;
        canDash = false;
        // 대쉬중 중력영향 무시
        float gravity = rigid.gravityScale;
        rigid.gravityScale = 0f;

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
        //gameObject.layer = defaultLayer;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;

    }

    /*    public void Jump()
        {
            //rigid.velocity = Vector2.zero;
            gameObject.layer = jumpLayer;
            if (jumpTimer < jumpTimerLimit)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower / 1.5f); // 점프 유지
            }

            jumpTimer += Time.deltaTime;

        }*/

    /*    public void DropDown()
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, FloorLayer);

            if (hitCollider != null)
            {
                isDropping = true;

                if (!currentPlatforms.Contains(hitCollider))
                {
                    currentPlatforms.Add(hitCollider);
                    Physics2D.IgnoreCollision(playerCollider, hitCollider, true);

                }
            }

        }*/
    /*
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                isJumping = true;
                anim.SetBool("isJump", true);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isJumping = false;
                anim.SetBool("isJump", false);
            }
        }*/
}
