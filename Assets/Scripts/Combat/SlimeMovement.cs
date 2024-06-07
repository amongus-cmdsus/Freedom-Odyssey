using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
 
public class RandomMovement : MonoBehaviour 
{
    public float movementSpeed = 5;
    Vector3 direction;

    int waitSeconds;
    float seconds;

    // Update is called once per frame
    private void Update() {
        if (transform.position == direction) {
            direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
            waitSeconds = Random.Range(0,3);
            if(seconds != waitSeconds){
                seconds = seconds + Time.deltaTime;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
    }
}
       
    

   