using UnityEngine;
using Cinemachine;

public class BulletAim : MonoBehaviour
{
    public CinemachineFreeLook normalCam;
    public CinemachineVirtualCamera bulletAimCam;
    public GameObject aimTarget;
    public Canvas crosshair;

    public float mouseSens;

    private CharacterController character;
    private Quaternion playerRotation;
    private Quaternion aimRotation;
    private PlanarMove playerMovement;

    private void Start()
    {
        playerRotation = gameObject.transform.rotation;
        aimRotation = aimTarget.transform.rotation;
        playerMovement = gameObject.GetComponent<PlanarMove>();
        character = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            bulletAimCam.Priority = 20;
            normalCam.Priority = 10;

            playerMovement.enabled = false;

            crosshair.enabled = true;
            BulletAimCamControl();
            BulletAimMove();
        } 
        else
        {
            playerMovement.enabled = true;

            crosshair.enabled = false;
            bulletAimCam.Priority = 10;
            normalCam.Priority = 20;
        }
    }

    void BulletAimCamControl()
    {
        float horizontalMouse = Input.GetAxis("Mouse X") * mouseSens;
        playerRotation.y += horizontalMouse;
        transform.rotation = Quaternion.Euler(0f, playerRotation.y, 0f);

        float verticalMouse = Input.GetAxis("Mouse Y") * -mouseSens;
        aimRotation.x += verticalMouse;
        aimTarget.transform.rotation = Quaternion.Euler(aimRotation.x, playerRotation.y, 0f);
    }

    void BulletAimMove()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        character.Move(direction * 5f * Time.deltaTime);
    }
}
