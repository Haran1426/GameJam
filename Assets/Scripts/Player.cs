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
    public float moveSpeed = 5f;
    public float jumpPower = 15f;

    [Header("¶¥ ®G")]
    public Transform groundCheck;
    public float groundRabius = 0.2f;
    public LayerMask GroundLayer;

    [Header("ÇÃ·¹ÀÌ¾î »óÅÂ")]
    public int playerHP = 100;
    public EmotionState emostate;

    private Rigidbody2D rb;
    private bool isGround;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGround) rb.velocity = new Vector2(rb.velocity.x, jumpPower);

    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundRabius, GroundLayer);

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}
