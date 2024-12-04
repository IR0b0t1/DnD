using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Player speed variable, controls velocity multiplier
    public float playerSpeed = 1f;
    public float jumpAmount = 1f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // d key changes value to 1, a key changes value to -1
        float xMove = Input.GetAxisRaw("Horizontal");

        // w key changes value to 1, s key changes value to -1
        float zMove = Input.GetAxisRaw("Vertical");


        // Creates velocity in direction of value equal to keypress (WASD)
        // rb.velocity.y deals with falling + jumping by setting velocity to y.
        rb.velocity = new Vector3(xMove, rb.velocity.y, zMove) * playerSpeed;
    }
}
