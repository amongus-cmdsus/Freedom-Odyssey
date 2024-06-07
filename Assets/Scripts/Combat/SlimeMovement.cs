using System.Security.Cryptography.X509Certificates;
using UnityEngine;
 
public class RandomMovement : MonoBehaviour 
{
    public float movementSpeed = 5;
    Vector3 direction;

    int waitSeconds;
    float seconds;

    void Start (){
        waitSeconds = Random.Range(1,10);
        direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
    }
    // Update is called once per frame
    private void Update() {
        if (transform.position == direction) {
            if(seconds != waitSeconds){
                seconds = seconds + Time.deltaTime;
                direction = transform.position;
            }
            
            if (seconds >= waitSeconds){
                seconds = 0;
                waitSeconds = Random.Range(1,10);
                direction = new Vector3(Random.Range(-10,10),0.8f,Random.Range(-10,10));
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed*Time.deltaTime);
    }
}