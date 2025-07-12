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
    [Header("�÷��̾� ������")]
    public float moveSpeed;
    public float jumpPower;

    [Header("�� �G")]
    public Transform groundCheck;
    public float groundRabius = 0.2f;
    public LayerMask GroundLayer;

    [Header("�÷��̾� ����")]
    public int maxHP = 100;
    public int currentHP;
    public int attackDamege;
    public float attackDelay = 1f;
    public float changeDelay = 2f;
    public float skillDelay = 15f;
    public bool isSkill = false;
    public EmotionState emostate;

    private Rigidbody2D rb;
    private bool isGround;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        StartCoroutine(EmoChangeState());
        emostate = EmotionState.HAPPY;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGround) rb.velocity = new Vector2(rb.velocity.x, jumpPower);

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
            Debug.Log("��ų ���");
        }
        
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundRabius, GroundLayer);

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? moveSpeed : 0;
    }

    IEnumerator EmoChangeState()
    {
        while (true)
        {
            switch (emostate)
            {
                case (EmotionState.HAPPY):
                    moveSpeed = 7f;
                    attackDamege = 10;
                    attackDelay = 1f;
                    Debug.Log("ȸ��");

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
        attackDamege = 20;
        attackDelay = 0.3f;
        Debug.Log("��ų �����");

        yield return new WaitForSeconds(7f);
        isSkill = false;
        skillDelay = 10f;
        EmoChangeState();
        Debug.Log("��ų ����");
    }

}
