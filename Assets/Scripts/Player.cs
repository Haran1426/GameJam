using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float changeDelay = 2f;
    private float skillDelay = 15f;
    public bool isSkill = false;
    public Animator anim;
    public EmotionState emotionState;

    public enum AnimState
    {
        IDLE,
        WALK,
        JUMP,
        ATTACK
    }
    public AnimState animState = AnimState.IDLE;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool isGround; // 바닥에 닿았는지 체크하는 변수
    private float moveInput;
    private Coroutine changeCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = rb.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
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

        // 방향 전환 (A키는 왼쪽, D키는 오른쪽)
        if (moveInput < 0) // 왼쪽으로 이동
        {
            sr.flipX = false; // 왼쪽을 보게 함
        }
        else if (moveInput > 0) // 오른쪽으로 이동
        {
            sr.flipX = true; // 오른쪽을 보게 함
        }

        // 스페이스바로 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // 수평 속도는 그대로 두고, 수직 속도만 jumpPower로 변경
            AnimOn(2); // 점프 애니메이션
        }
        else if (Mathf.Abs(moveInput) > 0.1f) // 좌우 이동 처리
        {
            AnimOn(1); // 걷기 애니메이션
        }
        else
        {
            AnimOn(0); // 대기 애니메이션
        }

        if (changeDelay > 0f) changeDelay -= Time.deltaTime;
        if (skillDelay > 0f) skillDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && changeDelay <= 0f && !isSkill)
        {
            emotionState = (EmotionState)(((int)emotionState + 1) % System.Enum.GetValues(typeof(EmotionState)).Length);
            EmoChangeState();
            Debug.Log(emotionState);
            changeDelay = 2f;
        }

        if (Input.GetKeyDown(KeyCode.Q) && skillDelay <= 0f && !isSkill)
        {
            StartCoroutine(ActiveSkill());
            Debug.Log("스킬 사용");
        }
    }


    void FixedUpdate()
    {
        // Raycast로 바닥 확인 (Player 위치에서 아래로 Ray를 쏴서 바닥 감지)
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground")); // 0.2f 범위로 Ray를 쏴서 바닥을 체크

        // 수평 이동
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? 4f : 0f; // 땅에 있을 때는 마찰을 줘서 미끄러지지 않도록 처리
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
        {
            StopCoroutine(changeCoroutine);
        }
        changeCoroutine = StartCoroutine(EmoChangeState());

        Debug.Log("스킬 종료");
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerAnimState", n); // 애니메이션 상태 변경
    }
}
