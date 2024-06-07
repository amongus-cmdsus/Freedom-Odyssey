using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5;
    public float distanceToGroundMax;
    public float distanceToGroundMin;
    float ground_pos;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Move character around on the xz axis
        transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);

        //Making sure that the character is grounded
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground, Mathf.Infinity)){
            if(ground.distance > distanceToGroundMax){
                transform.position = transform.position + new Vector3(0,-0.1f,0);
            }

            if(ground.distance < distanceToGroundMin){
                transform.position = transform.position + new Vector3(0,0.1f,0);
            }
        }
    }
}
