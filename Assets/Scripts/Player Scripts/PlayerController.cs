using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = rb2.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -10.0f;
            rb2.velocity = velocity;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x = 10.0f;
            rb2.velocity = velocity;
        }
        else
        {
            velocity.x = 0f;
            rb2.velocity = velocity;
        }
    }
}
