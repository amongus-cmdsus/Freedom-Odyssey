using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    //speed is nerfed by weapon weight

    //Player stats
    public GameObject player;

    //Enemy stats
    public class Enemy{
        public int enemyHealth;
        public Enemy(int _enemyHealth){
            enemyHealth = _enemyHealth;
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
                        enemiesInRange.Add(new Enemy(extraEnemies.GetComponent<Stats>().health));
                    }
                }
                combatMode = 1;
            }
        }

        if(combatMode == 1){
            playerMovement.enabled = false;

        }
    }
}
