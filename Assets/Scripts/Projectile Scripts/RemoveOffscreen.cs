using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOffscreen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
