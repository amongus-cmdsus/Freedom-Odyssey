using System.Security.Cryptography.X509Certificates;
using UnityEngine;
 
public class RandomMovement : MonoBehaviour 
{
    public float movementSpeed = 5;
    Vector3 direction;

    int someTime;
    float seconds;

    void Start ()
    {
        someTime = Random.Range(1,10);
        direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
    }

    private void Update() 
    {
        //Check if we are at the destination
        if (transform.position == direction) 
        {
            //Wait for someTime
            if(seconds != someTime)
            {
                seconds = seconds + Time.deltaTime;
                direction = transform.position;
            }
            
            //Decide new direction once fiinshed waiting
            if (seconds >= someTime)
            {
                seconds = 0;
                someTime = Random.Range(1,10);
                direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
    }
}