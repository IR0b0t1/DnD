using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask waterLayer;
    public Animator _animator;

    [Header("Audio")]
    public AudioClip walkSoundClip;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            Debug.LogWarning("PlayerMovement: No AudioSource found, added one automatically. Set to loop.", this);
        }
        else
        {
            audioSource.loop = true;
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        UpdateAnimation();
        UpdateWalkingSound();
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            Vector2 nextPos = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;

            Collider2D hitWater = Physics2D.OverlapCircle(nextPos, 0.01f, waterLayer);

            if (hitWater == null)
            {
                rb.MovePosition(nextPos);
            }
        }
    }

    void UpdateAnimation()
    {
        _animator.SetBool("IsRunningUp", false);
        _animator.SetBool("IsRunningDown", false);
        _animator.SetBool("IsRunningLeft", false);
        _animator.SetBool("IsRunningRight", false);

        if (movement.y > 0)
        {
            _animator.SetBool("IsRunningUp", true);
        }
        else if (movement.y < 0)
        {
            _animator.SetBool("IsRunningDown", true);
        }
        else if (movement.x < 0)
        {
            _animator.SetBool("IsRunningLeft", true);
        }
        else if (movement.x > 0)
        {
            _animator.SetBool("IsRunningRight", true);
        }
    }

    void UpdateWalkingSound()
    {
        if (audioSource == null || walkSoundClip == null) return;

        if (movement != Vector2.zero)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkSoundClip;
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}