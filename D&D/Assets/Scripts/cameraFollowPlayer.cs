using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float rotationY = 45f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(-8, 12, -8);
        if(Input.GetKeyDown(KeyCode.E))
        {
            rotationY += 90f;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotationY -= 90f;
        }
        Camera.main.transform.rotation = Quaternion.Euler(45, rotationY, 0);
    }
}
