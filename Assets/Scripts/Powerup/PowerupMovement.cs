using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PowerupMovement : MonoBehaviour
{
    Rigidbody2D rb2;

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    public void MoveUp()
    {
        rb2.velocity = Vector3.zero;
        rb2.AddForce(transform.up * 7f, ForceMode2D.Impulse);
    }
}
