using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy2 : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 5f;
    public string playerTag = "Player";

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Patrol")]
    [Tooltip("패트롤 시작점 (왼쪽)")]
    public Transform leftPatrolPoint;
    [Tooltip("패트롤 끝점 (오른쪽)")]
    public Transform rightPatrolPoint;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    // 패트롤 방향 플래그: true면 오른쪽으로, false면 왼쪽으로
    private bool movingRight = true;

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
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // 플레이어 추적
            Vector2 dir = (player.position - transform.position).normalized;
            movement = new Vector2(dir.x, 0f);
        }
        else
        {
            // 패트롤
            if (leftPatrolPoint == null || rightPatrolPoint == null)
            {
                movement = Vector2.zero;
            }
            else
            {
                // 현재 이동 방향에 따라
                movement = movingRight ? Vector2.right : Vector2.left;

                // 경계에 도달했으면 방향 전환
                if (movingRight && transform.position.x >= rightPatrolPoint.position.x)
                    movingRight = false;
                else if (!movingRight && transform.position.x <= leftPatrolPoint.position.x)
                    movingRight = true;
            }
        }

    }

    void FixedUpdate()
    {
        if (movement.x != 0f)
        {
            Vector2 targetPos = new Vector2(
                rb.position.x + movement.x * moveSpeed * Time.fixedDeltaTime,
                rb.position.y
            );
            rb.MovePosition(targetPos);
        }
    }

    void OnDrawGizmosSelected()
    {
        // 추적 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 패트롤 구간 시각화
        if (leftPatrolPoint != null && rightPatrolPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(leftPatrolPoint.position, rightPatrolPoint.position);
            Gizmos.DrawWireSphere(leftPatrolPoint.position, 0.2f);
            Gizmos.DrawWireSphere(rightPatrolPoint.position, 0.2f);
        }
    }
}
