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
    [Tooltip("��Ʈ�� ������ (����)")]
    public Transform leftPatrolPoint;
    [Tooltip("��Ʈ�� ���� (������)")]
    public Transform rightPatrolPoint;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    // ��Ʈ�� ���� �÷���: true�� ����������, false�� ��������
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
            // �÷��̾� ����
            Vector2 dir = (player.position - transform.position).normalized;
            movement = new Vector2(dir.x, 0f);
        }
        else
        {
            // ��Ʈ��
            if (leftPatrolPoint == null || rightPatrolPoint == null)
            {
                movement = Vector2.zero;
            }
            else
            {
                // ���� �̵� ���⿡ ����
                movement = movingRight ? Vector2.right : Vector2.left;

                // ��迡 ���������� ���� ��ȯ
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
        // ���� ���� �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ��Ʈ�� ���� �ð�ȭ
        if (leftPatrolPoint != null && rightPatrolPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(leftPatrolPoint.position, rightPatrolPoint.position);
            Gizmos.DrawWireSphere(leftPatrolPoint.position, 0.2f);
            Gizmos.DrawWireSphere(rightPatrolPoint.position, 0.2f);
        }
    }
}
