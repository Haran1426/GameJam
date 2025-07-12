using UnityEngine;

public class BackgroundFollower : MonoBehaviour
{
    public Transform player;  // 플레이어의 위치를 참조할 변수
    public Vector3 offset;    // 배경과 플레이어 간의 오프셋(거리)

    void Update()
    {
        // 플레이어의 위치를 따라가며, offset만큼의 거리만큼 뒤로 이동
        Vector3 newPosition = player.position + offset;
        transform.position = newPosition;
    }
}
