using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float rotationY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if(Input.GetKey(KeyCode.E))
        {
            rotationY += 0.5f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationY -= 0.5f;
        }
        Camera.main.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
