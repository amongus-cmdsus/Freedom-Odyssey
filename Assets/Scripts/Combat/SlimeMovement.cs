using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
 
public class RandomMovement : MonoBehaviour 
{
    public float movementSpeed = 5;

    private bool isWandering = false;
    private bool isWalkingF = false;

    double interval = 0.02; 

    double nextTime = 0;

    Vector3 direction;

    void Start()
    {
         direction = Random.insideUnitCircle.normalized;
    }


    // Update is called once per frame
    private void Update() 
    {
       {
            if(isWandering == false)
            {
            StartCoroutine(Wander());
            }
                if (isWalkingF == true)
            {
         if (Time.time >= nextTime)
        {

            nextTime += interval;


            transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
            
            }
            }
       }
    

    IEnumerator Wander()
    {
        int walkWait = Random.Range(2,5);

        int walkTime = Random.Range(2,5);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalkingF = true;

        yield return new WaitForSeconds(walkTime);

        isWalkingF = false;
            
        isWandering = false;

    }
    }
}
