using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -3f);

    [SerializeField] private Camera firstCamera;
    [SerializeField] private Camera secondCamera;

    public bool isFollowing = true;
    
    void LateUpdate()
    {
        if (player == null || !isFollowing) return;
        transform.position = player.position + offset;
    }
}