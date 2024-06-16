using UnityEngine;

public class RandomMovement : MonoBehaviour 
{
    public GameObject player;
    public float movementSpeed = 5;
    Vector3 direction;

    public int waitTimeMax;
    float seconds;

    void Start ()
    {
        waitTimeMax = Random.Range(1,10);
        direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
    }

    private void Update() 
    {   
        if(Vector3.Distance(transform.position, player.transform.position) <= 5)
        {
            //Check if we are at the destination
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime * 2);
        }
        else
        {
            if (transform.position == direction) 
            {
                //Wait for someTime
                if(seconds != waitTimeMax)
                {
                    seconds = seconds + Time.deltaTime;
                    direction = transform.position;
                }
                
                //Decide new direction once finished waiting
                if (seconds >= waitTimeMax)
                {
                    seconds = 0;
                    waitTimeMax = Random.Range(1,10);
                    direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
        }

    }
}