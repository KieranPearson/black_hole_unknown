using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOffscreen : MonoBehaviour
{
    [SerializeField] bool bottomOfScreenOnly;

    void OnBecameInvisible()
    {
        if (bottomOfScreenOnly)
        {
            if (transform.position.y > 0) return;
        }
        gameObject.SetActive(false);
    }
}
