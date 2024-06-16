using UnityEngine;

public class SeeThroughBuilding : MonoBehaviour
{
    Vector3 cameraPosition;

    public GameObject player;

    Material[] currentBuildingMat;
    Material[] previousBuildingMaterial;
    bool assignBuilding = true;

    void Update()
    {
        cameraPosition = player.transform.position + new Vector3(0, 10, -10);

        RaycastHit buildingInTheWay;

        if (Physics.Raycast(transform.position, player.transform.position - cameraPosition, out buildingInTheWay, Mathf.Infinity))
        {
            if (buildingInTheWay.transform.gameObject.layer == 3)
            {
                currentBuildingMat = buildingInTheWay.transform.gameObject.GetComponent<MeshRenderer>().materials;

                if (assignBuilding)
                {
                    previousBuildingMaterial = buildingInTheWay.transform.gameObject.GetComponent<MeshRenderer>().materials;
                    assignBuilding = false;
                }

                for (int i = 0; i < currentBuildingMat.Length; i++)
                {
                    currentBuildingMat[i].SetFloat("_Surface", 1);
                    currentBuildingMat[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    currentBuildingMat[i].SetInt("_ZWrite", 0);
                    currentBuildingMat[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    currentBuildingMat[i].renderQueue = 3000;

                    Color currentColor = currentBuildingMat[i].color;
                    currentColor.a = 0.02f;
                    currentBuildingMat[i].color = currentColor;
                }

                //Check to see if hitting a different building
                if (previousBuildingMaterial[0] != currentBuildingMat[0])
                {
                    for (int i = 0; i < previousBuildingMaterial.Length; i++)
                    {
                        previousBuildingMaterial[i].SetFloat("_Surface", 0);
                        previousBuildingMaterial[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        previousBuildingMaterial[i].SetInt("_ZWrite", 1);
                        previousBuildingMaterial[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        previousBuildingMaterial[i].renderQueue = -1;
                    }
                    assignBuilding = true;
                }
            }
            else if (previousBuildingMaterial != null) 
            {
                for (int i = 0; i < previousBuildingMaterial.Length; i++)
                {
                    previousBuildingMaterial[i].SetFloat("_Surface", 0);
                    previousBuildingMaterial[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    previousBuildingMaterial[i].SetInt("_ZWrite", 1);
                    previousBuildingMaterial[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    previousBuildingMaterial[i].renderQueue = -1;
                }
                assignBuilding = true;
            }
        }
    }
}
