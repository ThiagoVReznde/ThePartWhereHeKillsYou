using System.Collections;
using UnityEngine;

public class CenterTile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float delayBeforeCenter = 0.5f;
    [SerializeField] private float heightOffset = 1f;

    [SerializeField] private Camera fourthCamera;
    [SerializeField] private Camera fifthCamera;


    private bool isCentering = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCentering && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CenterPlayerRoutine(collision.gameObject));
            StartCoroutine(ChangeCameraRoutine(collision.gameObject));
        }
    }
    
    private IEnumerator ChangeCameraRoutine(GameObject player)
    {
        // Aguarda o breve momento após o impacto
        yield return new WaitForSeconds(2f);

        fourthCamera.enabled = !fourthCamera.enabled;
        fifthCamera.enabled = !fifthCamera.enabled;
    }

    private IEnumerator CenterPlayerRoutine(GameObject player)
    {
        isCentering = true;

        // Aguarda o breve momento após o impacto
        yield return new WaitForSeconds(delayBeforeCenter);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Para o impulso do launchpad
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Calcula o centro da tile + altura para o player não ficar enterrado no chão
        Vector3 targetPosition = transform.position + Vector3.up * heightOffset;
        
        // Teleporta direto para o centro
        player.transform.position = targetPosition;
    }
}