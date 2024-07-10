using UnityEngine;

public class RandomMovement : MonoBehaviour 
{
    public GameObject player;
    public float movementSpeed = 5;
    Vector3 direction;

    public int waitTime;
    float seconds;

    void Start ()
    {
        waitTime = Random.Range(1,10);
        direction = new Vector3(Random.Range(-10,10), 0.8f, Random.Range(-10,10));
    }

    // Move towards player if they are in certain range
    // If no player in range, then move towards a random point and wait for some time
    private void Update() 
    {   
        if(Vector3.Distance(transform.position, player.transform.position) <= 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime * 2);
        }
        else
        {
            if (transform.position == direction) 
            {
                if(seconds != waitTime)
                {
                    seconds = seconds + Time.deltaTime;
                    direction = transform.position;
                }
                
                if (seconds >= waitTime)
                {
                    seconds = 0;
                    waitTime = Random.Range(1,10);
                    direction = new Vector3(Random.Range(-10,10), 0.8f, Random.Range(-10,10));
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
        }

    }
}