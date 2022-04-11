using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOffscreen : MonoBehaviour
{
    [SerializeField] bool bottomOfScreenOnly;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    void OnBecameInvisible()
    {
        if (bottomOfScreenOnly)
        {
            if (thisTransform.position.y > 0) return;
        }
        gameObject.SetActive(false);
    }
}
