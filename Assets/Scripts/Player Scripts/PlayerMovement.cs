using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask waterLayer;

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
}
