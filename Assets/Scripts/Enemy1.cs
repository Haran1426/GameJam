using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy1 : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 5f;
    public string playerTag = "Player";

    [Header("Movement")]
    public float moveSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag(playerTag);
        if (go != null) player = go.transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
           
            Vector2 dir = (player.position - transform.position).normalized;
 
            movement = new Vector2(dir.x, 0f);
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (movement.x != 0f)
        {
       
            Vector2 targetPos = new Vector2(rb.position.x + movement.x * moveSpeed * Time.fixedDeltaTime,
                                            rb.position.y);
            rb.MovePosition(targetPos);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}