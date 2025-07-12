using UnityEngine;

public class BackgroundFollower : MonoBehaviour
{
    public Transform player;  // �÷��̾��� ��ġ�� ������ ����
    public Vector3 offset;    // ���� �÷��̾� ���� ������(�Ÿ�)

    void Update()
    {
        // �÷��̾��� ��ġ�� ���󰡸�, offset��ŭ�� �Ÿ���ŭ �ڷ� �̵�
        Vector3 newPosition = player.position + offset;
        transform.position = newPosition;
    }
}
