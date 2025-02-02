using UnityEngine;

public class SoundDistanceFade : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform playerTransform;
    public float minDistance ;
    public float maxDistance ;
    public float minVolume = 0f;
    public float maxVolume ;

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform not assigned.");
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float volumeRatio = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, volumeRatio);

        audioSource.volume = targetVolume;

        if (distance <= minDistance)
        {
            audioSource.volume = maxVolume;
        }
        else if (distance >= maxDistance)
        {
            audioSource.volume = minVolume;
        }
    }
}