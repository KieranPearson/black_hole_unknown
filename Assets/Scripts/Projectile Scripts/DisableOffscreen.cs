using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOffscreen : MonoBehaviour
{
    public static event System.Action<GameObject> OnProjectileRemoved;

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        OnProjectileRemoved?.Invoke(gameObject);
    }
}
