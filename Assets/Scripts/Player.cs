using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("플레이어")]
    public float moveSpeed;
    public float jumpPower;

    [Header("ui")]
    public int maxHP = 100;
    public int currentHP;
    public int attackDamage;
    private float attackDelay = 1f;
    private float changeDelay = 2f;
    private float skillDelay = 15f;
    public bool isSkill = false;
    public Animator anim;
    public EmotionState emotionState;
    public Image Image;
    public Image Image2;
    public List<Sprite> emoge;
    public Slider healthSlider;
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
    public bool isGround;
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
        UpdateHealthBar();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        
        if (moveInput < 0) 
        {
            sr.flipX = false; 
        }
        else if (moveInput > 0) 
        {
            sr.flipX = true; 
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); 
            AnimOn(2); 
        }
        else if (Mathf.Abs(moveInput) > 0.1f) 
        {
            AnimOn(1); 
        }
        else
        {
            AnimOn(0); 
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
            Debug.Log("스킬 활성화");
        }
    }


    void FixedUpdate()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? 4f : 0f; 
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
                    Debug.Log("행복");
                    Image.sprite = emoge[0];
                    Image2.sprite = emoge[1];
                    break;

                case EmotionState.SAD:
                    moveSpeed = 4f;
                    attackDamage = 20;
                    attackDelay = 2f;
                    Image.sprite = emoge[1];
                    Image2.sprite = emoge[2];
                    break;

                case EmotionState.ANGER:
                    moveSpeed = 10f;
                    attackDamage = 15;
                    attackDelay = 0.3f;
                    Debug.Log("분노");
                    Image.sprite = emoge[2];
                    Image2.sprite = emoge[0];
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
        Debug.Log("스킬활성화");

        yield return new WaitForSeconds(7f);
        isSkill = false;
        skillDelay = 15f;
        if (changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
        }
        changeCoroutine = StartCoroutine(EmoChangeState());

        Debug.Log("스킬 비활성화");
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerAnimState", n); 
    }
    private void UpdateHealthBar()
    {
        // 체력 비율을 계산하여 슬라이더의 값 설정
        healthSlider.value = (float)currentHP / maxHP; // Slider의 Value를 체력 비율로 설정
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }
    private void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Debug.Log("게임 오버");
        }
        UpdateHealthBar(); // 체력바 업데이트
    }
}
