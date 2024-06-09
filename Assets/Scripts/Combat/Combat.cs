using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Combat : MonoBehaviour
{
    //speed is nerfed by weapon weight

    //Player stats
    public GameObject player;
    public float AttackRange;

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

    Vector3[] points;

    public GameObject debugTool;

    void Start() 
    {
        playerMovement = this.gameObject.GetComponent<PlayerMovement>();
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
                    groundOverlay = Instantiate(ground, ground.transform.position + new Vector3(0, 1, 1), Quaternion.identity);
                    groundOverlay.GetComponent<MeshRenderer>().material = combatMat;
                    groundOverlay.GetComponent<MeshRenderer>().material.SetVector("_GridOffset", -transform.position + new Vector3(0.5f,0,0.5f));

                    Destroy(groundOverlay.GetComponent<BoxCollider>());
                    
                    combatMode = 1;
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
            groundOverlay.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), AttackRange);

            xPositionMax = transform.position.x + AttackRange;
            xPositionMin = transform.position.x - AttackRange;

            zPositionMax = transform.position.z + AttackRange;
            zPositionMin = transform.position.z - AttackRange;

            xDistance = new float[(int)Mathf.Round(xPositionMax) - (int)Mathf.Round(xPositionMin) + 1];
            zDistance = new float[(int)Mathf.Round(zPositionMax) - (int)Mathf.Round(zPositionMin) + 1];

            gridValue = new float[xDistance.Length * zDistance.Length];

            points = new Vector3[xDistance.Length * zDistance.Length];

            for (int i = 0; i < xDistance.Length; i++)
            {
                xDistance[i] = Mathf.Round(transform.position.x - (xPositionMin + i));

                for (int j = 0; j < zDistance.Length; j++)
                {
                    zDistance[j] = Mathf.Round(transform.position.z - (zPositionMin + j));
                    gridValue[i*j] = Mathf.Abs(xDistance[i]) + Mathf.Abs(zDistance[j]);

                    if (gridValue[i*j] <= AttackRange)
                    {
                        points[i*j] = new Vector3(transform.position.x - xDistance[i], 1, transform.position.z - zDistance[j]);
                    }
                }
            }

            combatMode = 2;
        }

        if (combatMode == 2)
        {

        }
    }
}
