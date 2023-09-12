using UnityEngine;

public class TerrainGenerator
{
    public int width = 256;
    public int height = 256;
    public float scale = 0.01f;
    public float depth = 5.4f;

    private GameObject terrainGo;
    private float minPeekCoordinateX;
    private float maxPeekCoordinateX;
    private float minPeekCoordinateY;
    private float maxPeekCoordinateY;
    private int peekSize = 5;

    public TerrainGenerator(GameObject terrainGo, LandModel landModel)
    {
        this.terrainGo = terrainGo;
        this.terrainGo.transform.position = new Vector3(-width / 2, this.terrainGo.transform.position.y, -height / 2);

        minPeekCoordinateY = width / 2 - Mathf.Floor(landModel.offsetX);
        maxPeekCoordinateY = width / 2 + Mathf.Floor(landModel.offsetX);

        minPeekCoordinateX = height / 2 - Mathf.Floor(landModel.offsetY);
        maxPeekCoordinateX = height / 2 + Mathf.Floor(landModel.offsetY);
    }

    public void init()
    {
        Terrain terrain = this.terrainGo.GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[, ] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight (int x, int y)
    {  

        if(x < minPeekCoordinateX || x > maxPeekCoordinateX || y < minPeekCoordinateY || y > maxPeekCoordinateY)
        {
            if (x < (minPeekCoordinateX-peekSize) || x > (maxPeekCoordinateX + peekSize) || y < (minPeekCoordinateY - peekSize) || y > (maxPeekCoordinateY + peekSize))
            {
                return -5f;
            } else
            {
                return 1f;
            }   
        }  else
        {
            return 0.9f;
        }

        
    }
}
