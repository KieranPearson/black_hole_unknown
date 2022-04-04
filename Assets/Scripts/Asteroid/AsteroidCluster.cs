using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCluster : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroids;

    public GameObject[] GetAsteroids()
    {
        return asteroids;
    }
}
