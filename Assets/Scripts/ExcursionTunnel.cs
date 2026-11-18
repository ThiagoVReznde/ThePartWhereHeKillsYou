using UnityEngine;
public class ExcursionTunnel : MonoBehaviour
{
    [Header("Funnel Settings")]
    public float flowSpeed = 6f;
    public float centeringForce = 4f;
    public bool isReversed = false;

    [Header("Beam Raycast")]
    public float maxLength = 20f;
    public LayerMask obstacleLayers;

    void FixedUpdate()
    {
        UpdateBeamLength();
    }

    void UpdateBeamLength()
    {
        float targetLength = maxLength;

        // Shoot a ray forward from the emitter to find walls
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxLength, obstacleLayers))
        {
            targetLength = hit.distance;
        }

        // Scale the beam's Y-axis to stop at the wall
        // (Make sure your funnel mesh/collider pivot is at the base, not the center)
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x, targetLength, currentScale.z);
    }

}