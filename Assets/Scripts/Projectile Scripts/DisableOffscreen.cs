using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOffscreen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
