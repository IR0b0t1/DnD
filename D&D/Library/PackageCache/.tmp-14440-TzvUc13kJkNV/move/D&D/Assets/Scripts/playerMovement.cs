using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Player speed variable, controls velocity multiplier
    public float playerSpeed = 5f;
    public float rotationY = 0f;
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

        // Preserve the current y velocity for falling and jumping
        float yMove = 0f;

        // Checking which button is pressed
        if (Input.GetKey(KeyCode.E))
        {
            rotationY += 0.5f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationY -= 0.5f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            yMove = 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            yMove = -1f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 10f;
        }
        else
        {
            playerSpeed = 5f;
        }

        // Local move
        Vector3 localMove = new Vector3(xMove, yMove, zMove);
        Vector3 worldMove = transform.TransformDirection(localMove);

        // Player movement
        rb.velocity = worldMove * playerSpeed;

        // Player rotation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
