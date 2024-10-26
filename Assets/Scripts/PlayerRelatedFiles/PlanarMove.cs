using UnityEngine;

public class PlanarMove : MonoBehaviour
{
    private CharacterController character;
    public Transform cam;

    public float playerSpeed = 2.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        character = gameObject.GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        PlanarMovemement();
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
    public float turnSmoothTime = 0.1f;
    Vector3 FaceTowardsDir(Vector3 inputs)
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
}