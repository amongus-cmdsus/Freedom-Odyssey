using UnityEngine;

public class SeeThroughBuilding : MonoBehaviour
{
    Vector3 cameraPosition;

    public GameObject player;

    Material[] currentBuildingInTheWay;
    Material[] previousBuildingMaterial;
    bool assignBuilding = true;

    void Update()
    {
        cameraPosition = player.transform.position + new Vector3(0, 10, -10);

        RaycastHit objectInTheWay;

        if (Physics.Raycast(transform.position, player.transform.position - cameraPosition, out objectInTheWay, Mathf.Infinity))
        {
            if (objectInTheWay.transform.gameObject.layer == 3)
            {
                currentBuildingInTheWay = objectInTheWay.transform.gameObject.GetComponent<MeshRenderer>().materials;

                if (assignBuilding)
                {
                    previousBuildingMaterial = objectInTheWay.transform.gameObject.GetComponent<MeshRenderer>().materials;
                    assignBuilding = false;
                }

                //Set each material in the current building transparent
                for (int i = 0; i < currentBuildingInTheWay.Length; i++)
                {
                    SetTransparent(currentBuildingInTheWay[i]);
                }

                //If its hitting a different building, then set the previous building to be normal again
                if (previousBuildingMaterial[0] != currentBuildingInTheWay[0])
                {
                    for (int i = 0; i < previousBuildingMaterial.Length; i++)
                    {
                        SetOpaque(previousBuildingMaterial[i]);
                    }
                    assignBuilding = true;
                }
            }
            else if (previousBuildingMaterial != null) //Check if hitting ground instead now
            {
                for (int i = 0; i < previousBuildingMaterial.Length; i++)
                {
                    SetOpaque(previousBuildingMaterial[i]);
                }
                assignBuilding = true;
            }
        }
    }

    void SetTransparent(Material material)
    {
        material.SetFloat("_Surface", 1);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Color currentColor = material.color;
        currentColor.a = 0.02f;
        material.color = currentColor;
    }

    void SetOpaque(Material material)
    {
        material.SetFloat("_Surface", 0);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
}
