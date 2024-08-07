using UnityEngine;

public class BlackHoleMovement : MonoBehaviour
{
    public GameObject eventHorizon;
    public GameObject disk;

    public float rotationSpeed;

    void Update()
    {
        eventHorizon.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); 
        disk.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
