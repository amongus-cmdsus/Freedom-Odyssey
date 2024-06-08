using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5;
    Vector3 position;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Gives height for sprite
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground, Mathf.Infinity)){
            position = ground.point + new Vector3(0,0.8f,0);
        }

        //Taking in player's input
        position += new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime); 

        transform.position = position;
    }
}