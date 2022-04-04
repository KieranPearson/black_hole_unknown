using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    [SerializeField] private Transform[] asteroidClusters;

    public Transform[] GetAsteroidClusters()
    {
        return asteroidClusters;
    }
}
