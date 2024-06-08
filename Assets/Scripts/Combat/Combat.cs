using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    //Combat Mode?
    int combatMode;
    PlayerMovement playerMovement;
    public GameObject ground;

    void Start() {
        playerMovement = this.gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        enemyCheck = Physics.OverlapSphere(transform.position, enemyCheckRadius);

        //Check all colliders in range to see if there are enemies in the vicinity
        foreach (Collider enemy in enemyCheck){
            if (enemy.tag == "Enemy"){
                //If enemy is detected then do another check for extra surrounding enemies and add them to a list
                extraEnemyCheck = Physics.OverlapSphere(transform.position, extraEnemyCheckRadius);

                foreach(Collider extraEnemies in extraEnemyCheck){
                    if(extraEnemies.tag == "Enemy"){
                        enemiesInRange.Add(new Enemy(extraEnemies.GetComponent<Stats>().health, extraEnemies.gameObject));
                    }
                }
                combatMode = 1;
            }
        }

        if(combatMode == 1){
            playerMovement.enabled = false;

            ground.GetComponent<MeshRenderer>().material.SetVector(Shader.PropertyToID("_PlayersPosition"), transform.position);
            ground.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), AttackRange);
        }
    }
}
