using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -3f);
    void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.position + offset;
    }
}
