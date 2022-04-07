using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PowerupHandler : MonoBehaviour
{
    [SerializeField] private float speed;

    Rigidbody2D rb2;

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb2.AddForce(transform.up * 7f, ForceMode2D.Impulse);
    }
}
