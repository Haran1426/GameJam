using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Header("ÇÃ·¹ÀÌ¾î ¿òÁ÷ÀÓ")]
    public float moveSpeed;
    public float jumpPower;

    [Header("¶¥ ®G")]
    public Transform groundCheck;
    public float groundRabius = 0.2f;
    public LayerMask GroundLayer;

    [Header("ÇÃ·¹ÀÌ¾î »óÅÂ")]
    public int maxHP = 100;
    public int currentHP;
    public int attackDamege;
    private float attackDelay = 1f;
    private float changeDelay = 2f;
    private float skillDelay = 15f;
    public bool isSkill = false;
    public Animator anim;
    public EmotionState emostate;

    public enum AnimState
    {
        IDLE,
        WALK,
        JUMP,
        ATTACK
    }
    public AnimState Animstate= AnimState.IDLE;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGround;
    private float moveInput;
    private Coroutine changeCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = rb.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
        StartCoroutine(EmoChangeState());
        emostate = EmotionState.HAPPY;
    }

    void Start()
    {
        changeCoroutine = StartCoroutine(EmoChangeState());    
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            AnimOn(2);
        }
        else if (Mathf.Abs(moveInput) > 0.1f)
        {
            AnimOn(1);
        }
        else AnimOn(0);

        if(changeDelay > 0f) changeDelay -= Time.deltaTime;
        if(skillDelay > 0f) skillDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && changeDelay <= 0f && !isSkill)
        {
            emostate = (EmotionState)(((int)emostate + 1) % System.Enum.GetValues(typeof(EmotionState)).Length);
            EmoChangeState();
            Debug.Log(emostate);
            changeDelay = 2f;
        }

        if(Input.GetKeyDown(KeyCode.Q) && skillDelay <= 0f && !isSkill)
        {
            StartCoroutine(ActiveSkill());
            Debug.Log("½ºÅ³ »ç¿ë");
        }
        
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundRabius, GroundLayer);

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? 4f : 0f;
    }

    IEnumerator EmoChangeState()
    {
        while (true)
        {
            if(isSkill)
            {
                yield return null;
                continue;
            }

            switch (emostate)
            {
                case (EmotionState.HAPPY):
                    moveSpeed = 7f;
                    attackDamege = 10;
                    attackDelay = 1f;
                    Debug.Log("È¸º¹");

                    break;

                case (EmotionState.SAD):
                    moveSpeed = 4f;
                    attackDamege = 20;
                    attackDelay = 2f;

                    break;

                case (EmotionState.ANGER):
                    moveSpeed = 10f;
                    attackDamege = 15;
                    attackDelay = 0.3f;
                    Debug.Log("¾ÆÇÄ");

                    break;
            }
            yield return new WaitForSeconds(1f);
        }
        
    }

    IEnumerator ActiveSkill()
    {
        isSkill = true;

        moveSpeed = 10f;
        attackDamege = 20;
        attackDelay = 0.3f;
        Debug.Log("½ºÅ³ »ç¿ëÁß");

        yield return new WaitForSeconds(7f);
        isSkill = false;
        skillDelay = 15f;
        if(changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
        }
        changeCoroutine = StartCoroutine(EmoChangeState());

        Debug.Log("½ºÅ³ Á¾·á");
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerAnimState", n);
    }

}
