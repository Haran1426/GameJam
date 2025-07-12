using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy1 : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("�÷��̾ �����ϴ� �ݰ�")]
    public float detectionRange = 5f;
    [Tooltip("�÷��̾� ���� ���θ� ǥ���� �±�")]
    public string playerTag = "Player";

    [Header("Movement")]
    [Tooltip("�̵� �ӵ�")]
    public float moveSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        // ������ playerTag�� ������ ������Ʈ ã��
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

        // �÷��̾�� �� ���� �Ÿ� ���
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            // �÷��̾� �������� ���� ���
            Vector2 dir = (player.position - transform.position).normalized;
            movement = dir;
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            // Rigidbody2D�� �̿��� �̵�
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // �� �信�� ��Ÿ� �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

