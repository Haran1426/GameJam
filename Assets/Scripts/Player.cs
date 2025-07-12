using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmotionState
{
    HAPPY,
    SAD,
    ANGER
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
    public float attackDelay = 1f;
    public float changeDelay = 2f;
    public EmotionState emostate;

    private Rigidbody2D rb;
    private bool isGround;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        emostate = EmotionState.HAPPY;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGround) rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        if(changeDelay > 0f) changeDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && changeDelay <= 0f)
        {
            emostate = (EmotionState)(((int)emostate + 1) % System.Enum.GetValues(typeof(EmotionState)).Length);
            Debug.Log(emostate);
            changeDelay = 2f;
        }

        Debug.Log(changeDelay);
        if (changeDelay <= 0f) Debug.Log("ÄðÅ¸ÀÓ ´Ù½Ã µ¼");

        switch (emostate)
        {
            case (EmotionState.HAPPY):
                moveSpeed = 7f;
                attackDamege = 10;
                attackDelay = 1f;

                break;

            case (EmotionState.SAD):
                moveSpeed = 3f;
                attackDamege = 7;
                attackDelay = 2f;

                break;

            case (EmotionState.ANGER):
                moveSpeed = 10f;
                attackDamege = 15;
                attackDelay = 0.3f;

                break;
        }

    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundRabius, GroundLayer);

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        rb.drag = isGround ? moveSpeed : 0;
    }

}
