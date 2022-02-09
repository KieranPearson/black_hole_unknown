using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MIN_X_POS = -3;
    [SerializeField] float MAX_X_POS = 3;

    private Rigidbody2D rb2;
    private Transform thisTransform;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        thisTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = rb2.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            if ((thisTransform.position.x - 10.0f) < MIN_X_POS) { return; }
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
