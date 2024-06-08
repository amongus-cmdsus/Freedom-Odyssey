using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    //speed is nerfed by weapon weight

    //Player stats
    public GameObject player;
    public float AttackRange;

    //Enemy stats
    public class Enemy{
        public GameObject thisEnemy;
        public int enemyHealth;
        public Enemy(int _enemyHealth, GameObject _thisEnemy)
        {
            enemyHealth = _enemyHealth;
            thisEnemy = _thisEnemy;
        }
    }

    //Collider array for enemy checks and a list of enemies in range
    Collider[] enemyCheck;
    public float enemyCheckRadius;
    Collider[] extraEnemyCheck;
    public float extraEnemyCheckRadius;
    List<Enemy> enemiesInRange = new List<Enemy>();

    //Switching between modes
    int combatMode;
    PlayerMovement playerMovement;

    //Combat Grid Overlay
    public GameObject ground;
    GameObject groundOverlay;
    public Material combatMat;

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
                if (enemy.tag == "Enemy")
                {
                    //If enemy is detected then do another check for extra surrounding enemies and add them to a list
                    extraEnemyCheck = Physics.OverlapSphere(transform.position, extraEnemyCheckRadius);

                    foreach (Collider extraEnemies in extraEnemyCheck)
                    {
                        if (extraEnemies.tag == "Enemy")
                        {
                            enemiesInRange.Add(new Enemy(extraEnemies.GetComponent<Stats>().health, extraEnemies.gameObject));
                        }
                    }
                    groundOverlay = Instantiate(ground, ground.transform.position + new Vector3(0, 0.1f, 1), Quaternion.identity);
                    groundOverlay.GetComponent<MeshRenderer>().material = combatMat;
                    groundOverlay.GetComponent<MeshRenderer>().material.SetVector("_Offset", transform.position + new Vector3(1,0,1));
                    combatMode = 1;
                }
            }
        }
         
        if (combatMode == 1)
        {
            playerMovement.enabled = false;
            
            groundOverlay.GetComponent<MeshRenderer>().material.SetVector(Shader.PropertyToID("_PlayersPosition"), transform.position);
            groundOverlay.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), AttackRange);
        }
    }
}
