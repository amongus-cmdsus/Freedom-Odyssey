using System.Collections.Generic;
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

    //Switching between modes
    //0 = not in combat | 1 = starting combat | 2 = 
    int combatMode;
    PlayerMovement playerMovement;

    //Combat Grid Overlay
    public GameObject ground;
    GameObject groundOverlay;
    public Material combatMat;

    //Movement during combat
    List<float> xPositions;
    float xPositionMin;
    float xPositionMax;

    List<float> zPositions;
    float zPositionMin;
    float zPositionMax;

    public GameObject debugTool;

    void Start() 
    {
        playerMovement = this.gameObject.GetComponent<PlayerMovement>();
        xPositions = new List<float>();
        zPositions = new List<float>();
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
         
        if (combatMode == 1)
        {
            //Disabling players input while keeping the height control
            playerMovement.horizontalInput = 0;
            playerMovement.verticalInput = 0;
            playerMovement.allowedToMove = false;

            groundOverlay.GetComponent<MeshRenderer>().material.SetVector(Shader.PropertyToID("_PlayersPosition"), transform.position);
            groundOverlay.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), AttackRange);

            xPositionMax = transform.position.x + AttackRange;
            xPositionMin = transform.position.x - AttackRange;

            zPositionMax = transform.position.z + AttackRange;
            zPositionMin = transform.position.z - AttackRange;

            for (float i = xPositionMin; i <= xPositionMax; i++)
            {
                xPositions.Add(i);
                Instantiate(debugTool, new Vector3(i,1,transform.position.z), Quaternion.identity);
            }

            for (float i = zPositionMin; i <= zPositionMax; i++)
            {
                zPositions.Add(i);
                Instantiate(debugTool, new Vector3(transform.position.x, 1, i), Quaternion.identity);
            }

            Debug.Break();
        }
    }
}
