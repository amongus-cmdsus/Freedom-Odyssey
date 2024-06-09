using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Combat : MonoBehaviour
{
    //speed is nerfed by weapon weight

    //Player stats
    public GameObject player;
    public int attackRange;
    public float speed;

    //Collider array for enemy checks and a list of enemies in range
    Collider[] enemyCheck;
    public float enemyCheckRadius;

    //Switching between stages of combat
    int combatMode;
    PlayerMovement playerMovement;

    //Combat Grid Overlay
    public GameObject ground;
    GameObject groundOverlay;
    public Material combatMat;

    //Movement during combat
    float xPositionMin;
    float xPositionMax;
    float[] xDistance;

    float zPositionMin;
    float zPositionMax;
    float[] zDistance;

    float[] gridValue;

    List<Vector3> points;
    Vector3 closestPoint = new Vector3();
    float closestDistance = Mathf.Infinity;


    public GameObject debugTool;

    void Start() 
    {
        playerMovement = this.gameObject.GetComponent<PlayerMovement>();
        points = new List<Vector3>();
    }

    void FixedUpdate()
    {
        if (combatMode == 0)
        {
            enemyCheck = Physics.OverlapSphere(transform.position, enemyCheckRadius);

            //Check all colliders in range to see if there are enemies in the vicinity
            foreach (Collider enemy in enemyCheck)
            {
                //If enemy is detected then create the combat overlay by duplicating the terrain then moving it up a little bit
                if (enemy.tag == "Enemy")
                {
                    groundOverlay = Instantiate(ground, ground.transform.position + new Vector3(0, 0.01f, 1), ground.transform.rotation);
                    groundOverlay.GetComponent<MeshRenderer>().material = combatMat;
                    groundOverlay.GetComponent<MeshRenderer>().material.SetVector("_GridOffset", -transform.position + new Vector3(0.5f,0,0.5f));

                    Destroy(groundOverlay.GetComponent<BoxCollider>());
                    
                    combatMode = 1;
                    return;
                }
                
            }
        }

        //Creating grid and defining which tiles is moveable
        if (combatMode == 1)
        {
            //Disabling players input while keeping the height control
            playerMovement.horizontalInput = 0;
            playerMovement.verticalInput = 0;
            playerMovement.allowedToMove = false;

            //Passing on player's info into the grid shader
            groundOverlay.GetComponent<MeshRenderer>().material.SetVector(Shader.PropertyToID("_PlayersPosition"), transform.position);
            groundOverlay.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), attackRange);

            //Creating a box around the player based on attack range 
            xPositionMax = transform.position.x + attackRange;
            xPositionMin = transform.position.x - attackRange;

            zPositionMax = transform.position.z + attackRange;
            zPositionMin = transform.position.z - attackRange;

            xDistance = new float[(int)Mathf.Round(xPositionMax) - (int)Mathf.Round(xPositionMin) + 1];
            zDistance = new float[(int)Mathf.Round(zPositionMax) - (int)Mathf.Round(zPositionMin) + 1];

            gridValue = new float[xDistance.Length * zDistance.Length];

            points.Clear();

            //For each tile in the box, see if the grid value is higher than the attack range
            for (int i = 0; i < xDistance.Length; i++)
            {
                xDistance[i] = Mathf.Round(transform.position.x - (xPositionMin + i));

                for (int j = 0; j < zDistance.Length; j++)
                {
                    zDistance[j] = Mathf.Round(transform.position.z - (zPositionMin + j));
                    gridValue[i*j] = Mathf.Abs(xDistance[i]) + Mathf.Abs(zDistance[j]);

                    if (gridValue[i*j] <= attackRange)
                    {
                        Vector3 XZPointCoordinate = new Vector3(transform.position.x - xDistance[i], 0, transform.position.z - zDistance[j]);
                        if (Physics.Raycast(XZPointCoordinate + new Vector3(0, 100, 0), Vector3.down, out RaycastHit ground, Mathf.Infinity))
                        {
                            points.Add(XZPointCoordinate + new Vector3(0, ground.point.y + playerMovement.heightAboveGround, 0));
                        }
                    }
                }
            }

            transform.position = closestPoint;
            combatMode = 2;
        }

        if (combatMode == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                closestDistance = Mathf.Infinity;
                Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit worldPos;

                if (Physics.Raycast(mousePos, out worldPos, 100f))
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (Vector3.Distance(points[i],worldPos.point) <= closestDistance)
                        {
                            closestPoint = points[i];
                            closestDistance = Vector3.Distance(points[i], worldPos.point);
                        }
                    }
                }
            }

            if (transform.position != closestPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, closestPoint, speed * Time.deltaTime);
            } 
            else
            {
                combatMode = 1;
            }
        }
    }
}
