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
    [Header("�÷��̾� ������")]
    public float moveSpeed;
    public float jumpPower;

    [Header("�÷��̾� ����")]
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
    public bool isGround; // �ٴڿ� ��Ҵ��� üũ�ϴ� ����
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

        // ���� ��ȯ (AŰ�� ����, DŰ�� ������)
        if (moveInput < 0) // �������� �̵�
        {
            sr.flipX = false; // ������ ���� ��
        }
        else if (moveInput > 0) // ���������� �̵�
        {
            sr.flipX = true; // �������� ���� ��
        }

        // �����̽��ٷ� ���� ó��
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // ���� �ӵ��� �״�� �ΰ�, ���� �ӵ��� jumpPower�� ����
            AnimOn(2); // ���� �ִϸ��̼�
        }
        else if (Mathf.Abs(moveInput) > 0.1f) // �¿� �̵� ó��
        {
            AnimOn(1); // �ȱ� �ִϸ��̼�
        }
        else
        {
            AnimOn(0); // ��� �ִϸ��̼�
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
            Debug.Log("��ų ���");
        }
    }


    void FixedUpdate()
    {
        // Raycast�� �ٴ� Ȯ�� (Player ��ġ���� �Ʒ��� Ray�� ���� �ٴ� ����)
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground")); // 0.2f ������ Ray�� ���� �ٴ��� üũ

        // ���� �̵�
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? 4f : 0f; // ���� ���� ���� ������ �༭ �̲������� �ʵ��� ó��
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
                    Debug.Log("ȸ��");
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
                    Debug.Log("����");
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
        Debug.Log("��ų �����");

        yield return new WaitForSeconds(7f);
        isSkill = false;
        skillDelay = 15f;
        if (changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
        }
        changeCoroutine = StartCoroutine(EmoChangeState());

        Debug.Log("��ų ����");
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerAnimState", n); // �ִϸ��̼� ���� ����
    }
}
