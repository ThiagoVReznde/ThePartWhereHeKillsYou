using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider cc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }
    private ExcursionTunnel currentTunnel;
    private CapsuleCollider funnelCollider;

    void FixedUpdate()
    {
        if (currentTunnel != null)
        {
            ApplyFunnelPhysics();
        }
    }



    void OnTriggerEnter(Collider other)
    {
        ExcursionTunnel funnel = other.GetComponent<ExcursionTunnel>();
        if (funnel != null)
        {
            currentTunnel = funnel;
            funnelCollider = other as CapsuleCollider;
            rb.useGravity = false;
        }
    }

    void ApplyFunnelPhysics()
    {
        Transform funnelTransform = currentTunnel.transform;

        // 1. Get the collider's true world center and axis (Y-Axis is CapsuleCollider default)
        Vector3 funnelCenter = funnelCollider != null 
            ? funnelTransform.TransformPoint(funnelCollider.center) 
            : funnelTransform.position;
        Vector3 funnelAxis = funnelTransform.up; // Change to .forward only if Capsule Direction = Z-Axis

        // 2. Find closest point on the central axis using the player's center of mass
        Vector3 toPlayer = rb.worldCenterOfMass - funnelCenter;
        Vector3 projectedPoint = funnelCenter + Vector3.Project(toPlayer, funnelAxis);

        // 3. Calculate centering pull and forward flow
        Vector3 centeringVelocity = (projectedPoint - rb.worldCenterOfMass) * currentTunnel.centeringForce;
        float direction = currentTunnel.isReversed ? -1f : 1f;
        Vector3 flowVelocity = funnelAxis * (currentTunnel.flowSpeed * direction);

        // 4. Faster lerp (10f-15f) prevents overshooting the center
        Vector3 targetVelocity = flowVelocity + centeringVelocity;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 12f);
    }

    void OnTriggerExit(Collider other)
    {
        // Restore gravity when leaving the active funnel
        ExcursionTunnel funnel = other.GetComponent<ExcursionTunnel>();
        if (funnel != null && funnel == currentTunnel)
        {
            currentTunnel = null;
            rb.useGravity = true;
            Debug.Log("Player exited tunnel!");
        }
    }
}
