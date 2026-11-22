using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider cc;

    private ExcursionTunnel currentTunnel;
    private CapsuleCollider funnelCollider;

    [SerializeField] private Camera firstCamera;
    [SerializeField] private Camera secondCamera;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Camera thirdCamera;
    [SerializeField] private Camera fourthCamera;
    [SerializeField] private Camera fifthCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

        firstCamera.enabled = true;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        fourthCamera.enabled = false;
        fifthCamera.enabled = false;        
    }

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

        if (other.CompareTag("TriggerCamChange1"))
        {
            firstCamera.enabled = !firstCamera.enabled;
            secondCamera.enabled = !secondCamera.enabled;
        }

        if (other.CompareTag("TriggerStopExcursion"))
        {
            if (currentTunnel != null)
            {
                currentTunnel.gameObject.SetActive(false);
                currentTunnel = null;
                rb.useGravity = true;
            }

            if (cameraFollow != null)
            {
                cameraFollow.isFollowing = false;
            }
        }

        if (other.CompareTag("LaunchPad"))
        {
            secondCamera.enabled = !secondCamera.enabled;
            thirdCamera.enabled = !thirdCamera.enabled;
        }

        if (other.CompareTag("TriggerCamChange2"))
        {
            thirdCamera.enabled = !thirdCamera.enabled;
            fourthCamera.enabled = !fourthCamera.enabled;
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
