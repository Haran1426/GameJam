using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EmotionState
{
    HAPPY,
    SAD,
    ANGER
}

public enum SkillState
{
    NOMAL,
    SKILL
}

public class Player : MonoBehaviour
{
    [Header("플레이어 움직임")]
    public float moveSpeed;
    public float jumpPower;

    [Header("플레이어 상태")]
    public int maxHP = 100;
    public int currentHP;
    public int attackDamage;
    private float attackDelay = 1f;
    public float changeDelay = 2f;
    public float skillDelay = 15f;
    public bool isSkill = false;
    public Animator anim;
    public EmotionState emotionState;
    bool Jump;

    [Header("대쉬 설정")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    public enum AnimState
    {
        IDLE,
        WALK,
        JUMP,
        DASH,
        ATTACK
    }

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool isGround;
    private float moveInput;
    private Coroutine changeCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = rb.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
        attackDamage = 10;
        StartCoroutine(EmoChangeState());
        emotionState = EmotionState.HAPPY;
    }

    void Start()
    {
        changeCoroutine = StartCoroutine(EmoChangeState());
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        // 방향 전환
        if (moveInput < 0)
            sr.flipX = false;
        else if (moveInput > 0)
            sr.flipX = true;

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !Jump && !isDashing)
        {
            Jump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            AnimOn((int)AnimState.JUMP);
        }

        // 대쉬 입력
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
        }

        // 이동 & 대기 애니메이션
        if (!Jump && !isDashing)
        {
            if (Mathf.Abs(moveInput) > 0.1f)
                AnimOn((int)AnimState.WALK);
            else
                AnimOn((int)AnimState.IDLE);
        }

        // 감정 전환
        if (changeDelay > 0f) changeDelay -= Time.deltaTime;
        if (skillDelay > 0f) skillDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && changeDelay <= 0f && !isSkill)
        {
            emotionState = (EmotionState)(((int)emotionState + 1) % System.Enum.GetValues(typeof(EmotionState)).Length);
            EmoChangeState();
            Debug.Log(emotionState);
            changeDelay = 2f;
        }

        // 스킬 발동
        if (Input.GetKeyDown(KeyCode.Q) && skillDelay <= 0f && !isSkill)
        {
            StartCoroutine(ActiveSkill());
            Debug.Log("스킬 사용");
        }

        if (Input.GetMouseButtonDown(0))
        {
            AnimOn((int)AnimState.ATTACK);
        }
    }

    void FixedUpdate()
    {
        Vector2 Position = transform.position;
        Position.y -= 1;
        isGround = Physics2D.Raycast(Position, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));

        if (isGround && Jump)
        {
            Jump = false;
            if (!isDashing)
                AnimOn((int)AnimState.IDLE);
        }

        if (!isDashing)
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? 4f : 0f;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        float direction = sr.flipX ? -1f : 1f;
        rb.velocity = new Vector2(direction * dashSpeed, 0f);

        AnimOn((int)AnimState.DASH);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        if (isGround)
            AnimOn((int)AnimState.IDLE);
    }

    IEnumerator EmoChangeState()
    {
        while (true)
        {
            if (isSkill)
            {
                yield return null;
                continue;
            }

            switch (emotionState)
            {
                case EmotionState.HAPPY:
                    moveSpeed = 7f;
                    attackDamage = 10;
                    attackDelay = 1f;
                    Debug.Log("회복");
                    break;
                case EmotionState.SAD:
                    moveSpeed = 4f;
                    attackDamage = 20;
                    attackDelay = 2f;
                    break;
                case EmotionState.ANGER:
                    moveSpeed = 10f;
                    attackDamage = 15;
                    attackDelay = 0.3f;
                    Debug.Log("아픔");
                    break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ActiveSkill()
    {
        isSkill = true;

        moveSpeed = 10f;
        attackDamage = 20;
        attackDelay = 0.3f;
        Debug.Log("스킬 사용중");

        yield return new WaitForSeconds(7f);
        isSkill = false;
        skillDelay = 15f;

        if (changeCoroutine != null)
            StopCoroutine(changeCoroutine);

        changeCoroutine = StartCoroutine(EmoChangeState());

        Debug.Log("스킬 종료");
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerAnimState", n);
    }
}
