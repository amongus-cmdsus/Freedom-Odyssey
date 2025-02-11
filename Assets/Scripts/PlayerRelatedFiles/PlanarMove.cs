using UnityEngine;

public class PlanarMove : MonoBehaviour
{
    private CharacterController character;
    public Transform cam;
    [Header("Planar Movement")]
    public float playerSpeed = 2.0f;
    public float turnSmoothTime = 0.1f;

    // Jump vars
    [HideInInspector]
    public bool isDashing;
    [HideInInspector]
    public Vector3 verticalVelocity;
    private bool isGrounded;
    private float previousYPos;
    private float modifiedGravityValue;
    [Header("Jump Values")]
    public float gravityValue = -9.81f;
    public float fallSpeedModifer = 2;
    public float jumpHeight = 1.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        character = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlanarMovemement();
    }

    // Move character across the plane
    private void PlanarMovemement()
    {
        Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, character.bounds.extents.y + 0.1f);

        // Changes the height position of the player
        if (isGrounded)
        {
            verticalVelocity.y = 0;
            modifiedGravityValue = 0;

            if (Input.GetButton("Jump"))
            {
                verticalVelocity.y += jumpHeight;
            }

            previousYPos = character.transform.position.y;
        }
        else if (!isGrounded && !isDashing)
        {
            ApplyGravity();
        }

        Vector3 moveDirection = verticalVelocity;

        if (inputs.magnitude >= 0.1f)
        {
            moveDirection += FaceTowardsDir(inputs) * playerSpeed;
        }

        character.Move(moveDirection * Time.deltaTime);
    }

    // Face towards the direction we are moving
    private Vector3 FaceTowardsDir(Vector3 inputs)
    {
        float turnSmoothVelocity = 0f;

        // Get angle to face relative to camera
        float angleToFace = Mathf.Atan2(inputs.x, inputs.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        // Set the move direction to the angle before we smooth it
        Vector3 moveDir = Quaternion.Euler(0f, angleToFace, 0f) * Vector3.forward;
        // Smooth out the angle because otherwise it turns too quickly
        angleToFace = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToFace, ref turnSmoothVelocity, turnSmoothTime);
        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, angleToFace, 0f);

        return moveDir;
    }

    private void ApplyGravity()
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

        verticalVelocity.y += modifiedGravityValue;
    }
}