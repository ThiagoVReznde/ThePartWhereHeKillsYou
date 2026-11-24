using UnityEngine;

public class Launchpad : MonoBehaviour
{
    [Header("Configurações de Impulso")]
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private bool resetCurrentVelocity = true;

    [SerializeField] private GameObject[] wallsToEnable;

    private AudioSource audioClip;


    // nova abordagem pra nao ficar a bagunça das cameras do player
    private void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioClip != null && !audioClip.isPlaying)
        {
            audioClip.Play();
        }
            
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }

        Launch(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Launch(collision.gameObject);
    }

    private void Launch(GameObject target)
    {
        if (!target.CompareTag("Player")) return;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (resetCurrentVelocity)
            {
                rb.linearVelocity = Vector3.zero;
            }

            rb.useGravity = true;
            rb.AddForce(transform.up * launchForce, ForceMode.Impulse);
        }
    }
}