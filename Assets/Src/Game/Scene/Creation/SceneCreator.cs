using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SceneCreator
{
    public Vector3 terrainPosition = new Vector3(0, 0, 0);
    public Vector3 terrainSize;
    private Quaternion randomRotation;

    private List<GameObject> sceneObjects = new List<GameObject>();
    //private GameObject cubeContainer = new GameObject();
    private GameObject isFloorFound;
    private GameObject floorObject;
    private GameObject windArea;

    private int halfOfSceneSizeX;
    private int halfOfSceneSizeY;
    private int randomFloorNumber1;
    private int randomFloorNumber2;
    private int randomNumber;
    private int randomIndex;
    private float height = 10f;
    private float sizeMultipier = 2.5f;
    private float randomValue;
    private float positionX = 0f;
    private float positionY = 0f;
    private float positionZ = 0f;
    private float randomRotationValue;
    private float[] arrayOfRotationValues = { 0f, 90f, 180f, 270f };
    private const float yBoxColliderSize = 0.45f;
    private string floorNumber;

    /*public LandModel createSceneLand(Level.Scene scene, int levelToLoad, GameObject cube)
    {
        Level.Scene coordinates = scene;

        halfOfSceneSizeX = coordinates.arena.x / 2;
        halfOfSceneSizeY = coordinates.arena.y / 2;

        cubeContainer.name = "CUBES";
        sceneObjects.Add(cubeContainer);

        isFloorFound = GameObject.FindGameObjectWithTag("Floor");

        for (int i = 0; i <= coordinates.arena.x; i+=2)
        { 
            GameObject cubeCloneX = GameObject.Instantiate(cube, new Vector3(i, 0, 0), cube.transform.rotation);
            cubeCloneX.transform.parent = cubeContainer.transform;
            cubeCloneX.name = "CubeX_" + (i/2);

            if (isFloorFound)
            {
                randomFloorNumber1 = SetRandomFloor();

                floorNumber = "Floor" + randomFloorNumber1.ToString();
                floorObject = GameObject.Find(floorNumber);

                cubeCloneX.GetComponent<MeshFilter>().mesh = floorObject.GetComponent<MeshFilter>().mesh;
                cubeCloneX.GetComponent<MeshRenderer>().material = floorObject.GetComponent<MeshRenderer>().material;
            }
            
            for (float j = 2; j < coordinates.arena.y; j+=2)
            {
                randomRotation = SetRandomRotation();

                GameObject cubeCloneY = GameObject.Instantiate(cube, new Vector3(i, 0, j), randomRotation);
                cubeCloneY.transform.parent = cubeCloneX.transform;
                cubeCloneY.name = "CubeY_" + (j/2);

                if (isFloorFound)
                {
                    randomFloorNumber2 = SetRandomFloor();

                    floorNumber = "Floor" + randomFloorNumber2.ToString();
                    floorObject = GameObject.Find(floorNumber);

                    cubeCloneY.GetComponent<MeshFilter>().mesh = floorObject.GetComponent<MeshFilter>().mesh;
                    cubeCloneY.GetComponent<MeshRenderer>().material = floorObject.GetComponent<MeshRenderer>().material;
                }

            }
        }

        terrainSize = new Vector3((coordinates.arena.x - 1), 0, (coordinates.arena.y - 1));

        LandModel result = new LandModel();
        result.sizeX = coordinates.arena.x;
        result.sizeY = coordinates.arena.y;

        result.offsetX = terrainSize.x / 2;
        result.offsetY = terrainSize.z / 2;

        cubeContainer.AddComponent<BoxCollider>();
        cubeContainer.GetComponent<BoxCollider>().size = new Vector3(result.sizeX, -yBoxColliderSize, result.sizeY);
        cubeContainer.GetComponent<BoxCollider>().center = new Vector3(result.offsetX, 0, result.offsetY);
        cubeContainer.layer = 10;
        cubeContainer.transform.position = new Vector3(cubeContainer.transform.position.x - result.offsetX, -0.2f, cubeContainer.transform.position.z - result.offsetY);
       
        return result;
    }*/

    public void createWindZone(Vector3 terrainPosition, Vector3 terrainSize)
    {
        windArea = GameObject.FindGameObjectWithTag("WindArea");
        windArea.GetComponent<BoxCollider>().size = new Vector3(terrainSize.x, height, terrainSize.z);
        windArea.GetComponent<BoxCollider>().center = new Vector3(terrainPosition.x, 0, terrainPosition.z);
        windArea.transform.position = new Vector3(0f, height / 2, 0f);
    }

    public void createWindZone(Vector3 terrainPosition, Level.Scene scene)
    {
        windArea = new GameObject();
        windArea.transform.name = "WindArea";
        windArea.AddComponent<BoxCollider>();

        windArea.GetComponent<BoxCollider>().size = new Vector3(scene.arena.x * sizeMultipier, 50f, scene.arena.y * sizeMultipier);
        windArea.GetComponent<BoxCollider>().center = new Vector3(terrainPosition.x, 0, terrainPosition.z);
        windArea.GetComponent<BoxCollider>().isTrigger = true;

        windArea.transform.position = new Vector3(0f, height, 0f);
    }

    public List<GameObject> getSceneObjects()
    {
        return sceneObjects;
    }

    public void generateTerrainBorders(Vector3 terrainPosition, Vector3 terrainSize)
    {
        BorderCreator borderCreator = new BorderCreator(sizeMultipier);
        borderCreator.bordersContainer.name = "BORDERS";
        borderCreator.createWidthBorders(terrainPosition, terrainSize);
        borderCreator.createLengthBorders(terrainPosition, terrainSize);
        borderCreator.bordersContainer.transform.position = new Vector3(-halfOfSceneSizeX, 0f, -halfOfSceneSizeY);
    }

    public void generateTerrainBorders(Vector3 terrainPosition, Level.Scene scene)
    {
        BorderCreator borderCreator = new BorderCreator(sizeMultipier);
        borderCreator.createWidthBorders(terrainPosition, scene);
        borderCreator.createLengthBorders(terrainPosition, scene);
        borderCreator.bordersContainer.transform.position = Vector3.zero;
    }

    public class Arena
    {
        public Coordinates arena { get; set; }
    }

    public class Coordinates
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    private int SetRandomFloor()
    {
        randomNumber = Random.Range(1, 5);

        return randomNumber;
    }

    private float RandomRotationValue()
    {
        randomIndex = Random.Range(0, 4);

        randomValue = arrayOfRotationValues[randomIndex];

        return randomValue;
    }

    private Quaternion SetRandomRotation()
    {
        randomRotationValue = RandomRotationValue();

        positionX = 0f;
        positionY = randomRotationValue;
        positionZ = 0f;

        Quaternion rotation = Quaternion.Euler(positionX, positionY, positionZ);
        return rotation;
    }
}
