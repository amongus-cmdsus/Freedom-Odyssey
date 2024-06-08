using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5;
    Vector3 position;
    public float heightAboveGround;

    public bool allowedToMove;
    public float horizontalInput;
    public float verticalInput;

    private void Start()
    {
        allowedToMove = true;
    }

    void FixedUpdate()
    {
        if (allowedToMove) 
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        //Gives height for sprite
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground, Mathf.Infinity))
        {
            position = ground.point + new Vector3(0, heightAboveGround, 0);
        }

        //Taking in player's input
        position += new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime); 

        transform.position = position;
    }
}
