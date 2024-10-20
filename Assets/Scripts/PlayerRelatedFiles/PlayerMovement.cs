using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;
    public Transform cam;

    private Vector3 verticalVelocity;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        PlanarMovemement();
        Gravity();
        CharJump();
    }

    // Move character across the plane
    void PlanarMovemement()
    {
        Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if (inputs.magnitude >= 0.1f)
        {
            character.Move(FaceTowardsDir(inputs) * playerSpeed * Time.deltaTime);
        }
    }

    // Face towards the direction we are moving
    Vector3 FaceTowardsDir(Vector3 inputs)
    {
        float smoothTime = 0.1f ;
        float turnSmoothVelocity = 0f;

        // Get angle to face relative to camera
        float angleToFace = Mathf.Atan2(inputs.x, inputs.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        // Set the move direction to the angle before we smooth it
        Vector3 moveDir = Quaternion.Euler(0f, angleToFace, 0f) * Vector3.forward;
        // Smooth out the angle because otherwise it turns too quickly
        angleToFace = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToFace, ref turnSmoothVelocity, smoothTime);
        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, angleToFace, 0f);

        return moveDir;
    }

    // Jump 
    void CharJump()
    {
        // Changes the height position of the player
        if (Input.GetButtonDown("Jump") && character.isGrounded)
        {
            verticalVelocity.y += jumpHeight;
        }

        character.Move(verticalVelocity * Time.deltaTime);
    }

    // Applies gravity
    void Gravity()
    {
        if (!character.isGrounded)
        {
            verticalVelocity.y += gravityValue * Time.deltaTime;
        } else
        {
            verticalVelocity.y = 0;
        }
    }
}