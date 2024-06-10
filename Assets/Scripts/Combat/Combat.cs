using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Combat : MonoBehaviour
{
    [HideInInspector]
    public Vector3 playerPosition;
    public Vector3 playerNewPosition;

    //Overlay
    public GameObject ground;
    [HideInInspector]
    public GameObject instantiatedGround;
    MeshRenderer instantiatedGroundMeshRenderer;
    public Material combatMat;

    public List<Vector3> ThisUnitsMoveablePoints(Vector3 currentPosition, int moveRange, float heightAboveGround)
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

        //Creating a box around the player based on attack range 
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

    public void CreateOverlay()
    {
        instantiatedGround = Instantiate(ground, ground.transform.position + new Vector3(0, 0.01f, 1), ground.transform.rotation);

        instantiatedGroundMeshRenderer = instantiatedGround.GetComponent<MeshRenderer>();
        instantiatedGroundMeshRenderer.material = combatMat;
        instantiatedGroundMeshRenderer.material.SetVector("_GridOffset", -playerPosition + new Vector3(0.5f, 0, 0.5f));
        instantiatedGroundMeshRenderer.material.SetVector(Shader.PropertyToID("_PlayersPosition"), playerPosition);

        Destroy(instantiatedGround.GetComponent<BoxCollider>());
    }

    private void Update()
    {
        if (instantiatedGroundMeshRenderer != null)
        {
            instantiatedGroundMeshRenderer.material.SetVector(Shader.PropertyToID("_PlayersPosition"), playerNewPosition);
        }
    }
}
