using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Not combat
    public float movementSpeed = 5;
    public float heightAboveGround;
    Vector3 position;

    float horizontalInput;
    float verticalInput;

    //Combat 
    public Combat combatManager;

    Collider[] enemyCheck;
    public float enemyCheckRadius;
    
    bool inCombat;

    public int moveRange;
    List<Vector3> moveablePoints;
    Vector3 closestMoveablePoint;

    void Update()
    {
        if (!inCombat)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            //Gives height for sprite
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, out RaycastHit ground, Mathf.Infinity))
            {
                position = ground.point + new Vector3(0, heightAboveGround, 0);
            }

            //Taking in player's input
            position += new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);

            transform.position = position;

            //Check all colliders in range to see if there are enemies in the vicinity
            enemyCheck = Physics.OverlapSphere(transform.position, enemyCheckRadius);

            foreach (Collider enemy in enemyCheck)
            {
                //If enemy is detected then create the combat overlay by duplicating the terrain then moving it up a little bit
                if (enemy.tag == "Enemy")
                {
                    combatManager.playerPosition = transform.position;
                    combatManager.CreateOverlay();

                    closestMoveablePoint = transform.position;
                    
                    inCombat = true;
                }
            }
        }
        else
        {
            moveablePoints = combatManager.ThisUnitsMoveablePoints(transform.position, moveRange, heightAboveGround);
            
            if (Input.GetMouseButtonDown(0) && transform.position == closestMoveablePoint)
            {
                moveablePoints = combatManager.ThisUnitsMoveablePoints(transform.position, moveRange, heightAboveGround);
                float closestDistanceToPoint = Mathf.Infinity;
                Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit worldPos;

                if (Physics.Raycast(mousePos, out worldPos, 100f))
                {
                    for (int i = 0; i < moveablePoints.Count; i++)
                    {
                        if (Vector3.Distance(moveablePoints[i], worldPos.point) <= closestDistanceToPoint)
                        {
                            closestMoveablePoint = moveablePoints[i];
                            closestDistanceToPoint = Vector3.Distance(moveablePoints[i], worldPos.point);
                        }
                    }
                }
            }

            if (transform.position != closestMoveablePoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, closestMoveablePoint, movementSpeed * Time.deltaTime);
            }

            if (transform.position == closestMoveablePoint)
            {
                combatManager.playerNewPosition = transform.position;
            }
        }
    }
}
