using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSoundController : MonoBehaviour
{
    public AudioClip idleClip; 
    public AudioClip forwardClip;
    public AudioClip backwardClip;
    public AudioClip turnClip;

    private AudioSource audioSource;
    private CarMovement carMovement;

    // Thresholds to detect movement states
    private float moveThreshold = 0.1f;
    private float turnThreshold = 5f; // degrees

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        carMovement = GetComponent<CarMovement>();

        if (audioSource == null)
            Debug.LogError("CarSoundController requires an AudioSource component.");

        if (carMovement == null)
            Debug.LogError("CarSoundController requires a CarMovement component on the same GameObject.");
    }

    void Update()
    {
        if (carMovement == null || audioSource == null)
            return;

        // Get joystick inputs from CarMovement's joystick reference
        float vertical = carMovement.joystick.Vertical;
        float horizontal = carMovement.joystick.Horizontal;

        // Determine movement state
        bool isMovingForward = vertical > moveThreshold;
        bool isMovingBackward = vertical < -moveThreshold;
        bool isTurning = Mathf.Abs(horizontal) > moveThreshold;

        AudioClip clipToPlay = idleClip;

        if (isMovingForward)
        {
            clipToPlay = forwardClip;
        }
        else if (isMovingBackward)
        {
            clipToPlay = backwardClip;
        }
        else if (isTurning)
        {
            clipToPlay = turnClip;
        }
        else
        {
            clipToPlay = idleClip;
        }

        PlaySound(clipToPlay);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
