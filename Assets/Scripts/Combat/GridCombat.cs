using System.Collections.Generic;
using UnityEngine;

public class GridCombat : MonoBehaviour
{
    [HideInInspector]
    public Vector3 playerPosition;
    [HideInInspector]
    public Vector3 playerNewPosition;

    // Overlay
    public GameObject ground;
    public Material gridMat;
    [HideInInspector]
    public GameObject instantiatedGround;
    MeshRenderer instantiatedGroundMeshRenderer;

    // Movement
    public float heightAboveGround;
    public float movementSpeed;

    public int moveRange;
    List<Vector3> moveablePoints;
    Vector3 closestMoveablePoint;

    void CreateOverlay()
    {
        instantiatedGround = Instantiate(ground, ground.transform.position + new Vector3(0, 0.01f, 0), ground.transform.rotation);

        instantiatedGroundMeshRenderer = instantiatedGround.GetComponent<MeshRenderer>();
        instantiatedGroundMeshRenderer.material = gridMat;
        instantiatedGroundMeshRenderer.material.SetVector("_GridOffset", -playerPosition + new Vector3(0.5f, 0, 0.5f));
        instantiatedGroundMeshRenderer.material.SetVector(Shader.PropertyToID("_PlayersPosition"), playerPosition);

        Destroy(instantiatedGround.GetComponent<BoxCollider>());
    }

    List<Vector3> MoveblePoints(Vector3 currentPosition, int moveRange, float heightAboveGround)
    {
        instantiatedGround.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_AttackRange"), moveRange);

        float xPositionMin;
        float xPositionMax;
        float[] xDistance;

        float zPositionMin;
        float zPositionMax;
        float[] zDistance;
        float[] gridValue;

        List<Vector3> moveablePoints = new List<Vector3>();

        xPositionMax = currentPosition.x + moveRange;
        xPositionMin = currentPosition.x - moveRange;

        zPositionMax = currentPosition.z + moveRange;
        zPositionMin = currentPosition.z - moveRange;

        xDistance = new float[(int)Mathf.Round(xPositionMax) - (int)Mathf.Round(xPositionMin) + 1];
        zDistance = new float[(int)Mathf.Round(zPositionMax) - (int)Mathf.Round(zPositionMin) + 1];

        gridValue = new float[xDistance.Length * zDistance.Length];

        //For each tile in the box, see if the grid value is less than the attack range
        //If grid value is less than attack range, then add it to a list of moveable points
        for (int i = 0; i < xDistance.Length; i++)
        {
            xDistance[i] = Mathf.Round(currentPosition.x - (xPositionMin + i));

            for (int j = 0; j < zDistance.Length; j++)
            {
                zDistance[j] = Mathf.Round(currentPosition.z - (zPositionMin + j));
                gridValue[i * j] = Mathf.Abs(xDistance[i]) + Mathf.Abs(zDistance[j]);

                if (gridValue[i * j] <= moveRange)
                {
                    Vector3 XZPointCoordinate = new Vector3(currentPosition.x - xDistance[i], 0, currentPosition.z - zDistance[j]);
                    if (Physics.Raycast(XZPointCoordinate + new Vector3(0, 100, 0), Vector3.down, out RaycastHit ground, Mathf.Infinity))
                    {
                        moveablePoints.Add(XZPointCoordinate + new Vector3(0, ground.point.y + heightAboveGround, 0));
                    }
                }
            }
        }

        return moveablePoints;
    }

    private void Update()
    {
        if (instantiatedGroundMeshRenderer != null)
        {
            instantiatedGroundMeshRenderer.material.SetVector(Shader.PropertyToID("_PlayersPosition"), playerNewPosition);
        }

        moveablePoints = MoveblePoints(transform.position, moveRange, heightAboveGround);

        if (Input.GetMouseButtonDown(0) && transform.position == closestMoveablePoint)
        {
            moveablePoints = MoveblePoints(transform.position, moveRange, heightAboveGround);
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
            playerNewPosition = transform.position;
        }
    }
}
