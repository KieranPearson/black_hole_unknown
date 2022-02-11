using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] float SPEED;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, SPEED);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
