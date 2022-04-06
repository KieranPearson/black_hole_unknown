using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDisabler : MonoBehaviour
{
    public void DisableExplosion()
    {
        gameObject.SetActive(false);
    }
}
