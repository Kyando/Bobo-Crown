using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Intensity of the shake
    public float base_shakeIntensity = 0.1f;

    // Duration of the shake in seconds
    public float shakeDuration = 0.5f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    // Call this method to start the screen shake
    public void Shake(float shakeIntensity)
    {
        this.base_shakeIntensity = shakeIntensity;
        if (!IsInvoking("StopShake"))
        {
            InvokeRepeating("DoShake", 0, 0.01f);
            Invoke("StopShake", shakeDuration);
        }
    }

    void DoShake()
    {
        float offsetX = Random.Range(-1f, 1f) * base_shakeIntensity;
        float offsetY = Random.Range(-1f, 1f) * base_shakeIntensity;

        transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        transform.position = originalPosition;
    }
}