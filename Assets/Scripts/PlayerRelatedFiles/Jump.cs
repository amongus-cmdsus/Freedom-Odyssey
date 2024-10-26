using UnityEngine;

public class Jump : MonoBehaviour
{  
    [HideInInspector]
    public bool isDashing;
    [HideInInspector]
    public Vector3 verticalVelocity;
    public float jumpHeight = 1.0f;
    private CharacterController character;
    private bool isGrounded;
    private float previousYPos;

    private void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        CharJump();
    }

    void CharJump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, character.bounds.extents.y + 0.1f);

        // Changes the height position of the player
        if (isGrounded)
        {
            verticalVelocity.y = 0;

            if (Input.GetButton("Jump"))
            {
                verticalVelocity.y += jumpHeight * Time.deltaTime;
            }

            previousYPos = character.transform.position.y;
        }
        else if (!isDashing)
        {
            ApplyGravity();
        }

        character.Move(verticalVelocity);
    }

    public float gravityValue = -9.81f;
    public float fallSpeedModifer = 2;
    private float modifiedGravityValue;
    void ApplyGravity()
    {
        // Fall faster than rise
        if (character.transform.position.y < previousYPos)
        {
            modifiedGravityValue = gravityValue * fallSpeedModifer;
        }
        else
        {
            modifiedGravityValue = gravityValue;
        }

        previousYPos = character.transform.position.y;

        verticalVelocity.y += modifiedGravityValue * Time.deltaTime;
    }
}
