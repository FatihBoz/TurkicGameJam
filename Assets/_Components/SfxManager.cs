using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bowRelease;
    [SerializeField] private AudioClip swordHit;
    [SerializeField] private AudioClip arrowHit;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBowReleaseSF()
    {
        audioSource.PlayOneShot(bowRelease);
    }

    public void PlaySwordHit()
    {
        audioSource.PlayOneShot(swordHit);
    }

    public void PlayArrowHit()
    {
        audioSource.PlayOneShot(arrowHit);
    }

}
