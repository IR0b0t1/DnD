using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask waterLayer;
    public Animator _animator;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        UpdateAnimation();
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

}
