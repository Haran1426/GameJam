using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy1 : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("플레이어를 감지하는 반경")]
    public float detectionRange = 5f;
    [Tooltip("플레이어 감지 여부를 표시할 태그")]
    public string playerTag = "Player";

    [Header("Movement")]
    [Tooltip("이동 속도")]
    public float moveSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        // 씬에서 playerTag로 지정된 오브젝트 찾기
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

        // 플레이어와 적 사이 거리 계산
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            // 플레이어 방향으로 벡터 계산
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
            // Rigidbody2D를 이용한 이동
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // 씬 뷰에서 사거리 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

