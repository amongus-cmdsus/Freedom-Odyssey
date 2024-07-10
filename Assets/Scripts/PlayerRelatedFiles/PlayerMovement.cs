using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5;
    public float heightAboveGround;
    Vector3 position;

    float horizontalInput;
    float verticalInput;

    Collider[] enemyCheck;
    public float enemyCheckRadius;
    
    bool inCombat;

    void Update()
    {
        inCombat = EnemyCheck(Physics.OverlapSphere(transform.position, enemyCheckRadius));

        if (!inCombat)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, out RaycastHit ground, Mathf.Infinity))
            {
                position = ground.point + new Vector3(0, heightAboveGround, 0);
            }

            position += new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);

            transform.position = position;
        }
    }

    bool EnemyCheck(Collider[] enemyCheck)
    {
        if (enemyCheck == null || enemyCheck.Length == 0)
        {
            return false;
        }

        foreach (Collider enemy in enemyCheck)
        {
            if (enemy.tag == "Enemy")
            {
                return true;
            }
        }

        return false;
    }
}
