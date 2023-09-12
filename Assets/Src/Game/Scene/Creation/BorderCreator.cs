using System.Collections.Generic;
using UnityEngine;

public class BorderCreator
{
    public GameObject bordersContainer;
    
    private List<GameObject> borderObjects = new List<GameObject>();
    private float sizeMultiplier;

    public BorderCreator(float sizeMultiplier)
    {
        this.sizeMultiplier = sizeMultiplier;

        bordersContainer = new GameObject();
        bordersContainer.name = "Borders";
    }

    public void createWidthBorders(Vector3 terrainPosition, Vector3 terrainSize)
    {
        float zOffset = terrainSize.z;
        float xOffset = terrainSize.x / 2;

        for (int i = 0; i < 2; i++)
        {
            GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);

            border.transform.localScale = new Vector3(terrainSize.x + 1, terrainSize.y + 2, 1);

            if (i == 0)
                border.transform.position = new Vector3(terrainPosition.x + xOffset, terrainPosition.y + 1, terrainPosition.z - 1);
            else
                border.transform.position = new Vector3(terrainPosition.x + xOffset, terrainPosition.y + 1, terrainPosition.z + zOffset + 1);

            DisableMeshRenderer(border);

            border.transform.name = "WidthBorder_" + (i + 1);
            border.transform.parent = bordersContainer.transform;
            borderObjects.Add(border);
        }
    }

    public void createWidthBorders(Vector3 terrainPosition, Level.Scene scene)
    {
        float xOffset = scene.arena.x;
        float zOffset = scene.arena.y;

        for (int i = 0; i < 2; i++)
        {
            GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
            border.transform.localScale = new Vector3(xOffset * sizeMultiplier, zOffset, 1);

            if (i == 0)
                border.transform.position = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z + zOffset);
            else
                border.transform.position = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z - zOffset);

            DisableMeshRenderer(border);

            border.transform.name = "WidthBorder_" + (i + 1);
            border.transform.parent = bordersContainer.transform;
            borderObjects.Add(border);
        }
    }

    public void createLengthBorders(Vector3 terrainPosition, Vector3 terrainSize)
    {
        float xOffset = terrainSize.x;
        float zOffset = terrainSize.z / 2;

        for (int i = 0; i < 2; i++)
        {
            GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);

            border.transform.localScale = new Vector3(1, terrainSize.y + 2, terrainSize.z + 1);
            if (i == 0)
                border.transform.position = new Vector3(terrainPosition.x - 1, terrainPosition.y + 1, terrainPosition.z + zOffset);
            else
                border.transform.position = new Vector3(terrainPosition.x + xOffset + 1, terrainPosition.y + 1, terrainPosition.z + zOffset);
            
            DisableMeshRenderer(border);

            border.transform.name = "LengthBorder_" + (i + 1);
            border.transform.parent = bordersContainer.transform;
            borderObjects.Add(border);
        }
    }

    public void createLengthBorders(Vector3 terrainPosition, Level.Scene scene)
    {
        float xOffset = scene.arena.x;
        float zOffset = scene.arena.y;

        for (int i = 0; i < 2; i++)
        {
            GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
            border.transform.localScale = new Vector3(1, xOffset, zOffset * sizeMultiplier / (sizeMultiplier / 2));
            
            if (i == 0)
                border.transform.position = new Vector3(terrainPosition.x - (xOffset * sizeMultiplier) / 2, terrainPosition.y, terrainPosition.z);
            else
                border.transform.position = new Vector3(terrainPosition.x + (xOffset * sizeMultiplier) / 2, terrainPosition.y, terrainPosition.z);

            DisableMeshRenderer(border);

            border.transform.name = "LengthBorder_" + (i + 1);
            border.transform.parent = bordersContainer.transform;
            borderObjects.Add(border);
        }
    }

    private void DisableMeshRenderer(GameObject border)
    {
        border.GetComponent<MeshRenderer>().enabled = false;
    }
}
