using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDebug : MonoBehaviour
{
    public GameObject player;
    float xDistanceFromPlayer;
    float zDistanceFromPlayer;

    void FixedUpdate()
    {
        xDistanceFromPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        zDistanceFromPlayer = Mathf.Abs(player.transform.position.z - transform.position.z);
        print("xDist is " + Mathf.Round(xDistanceFromPlayer));
        print("zDist is " + Mathf.Round(zDistanceFromPlayer));
        print("Multiplied dist is " + (Mathf.Round(zDistanceFromPlayer) * Mathf.Round(xDistanceFromPlayer)));
    }
}
