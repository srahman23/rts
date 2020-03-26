using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public int mapSize;
    public float[,] mapArray;

    public GameObject node;

    public GameObject[,] nodes;

    public float scale, xOffset, yOffset;
    float lastScale, xLastOffset, yLastOffset;  
    
    void Start()
    {
        //Used later to check for change
        lastScale = scale;
        xLastOffset = xOffset;
        yLastOffset = yOffset;
        mapArray = new float[mapSize, mapSize];
        nodes = new GameObject[mapSize, mapSize];

        //I'm thinking of layering multiple noise maps on top of each other. Right now I got these two layers generated. 
        mapArray = generateNoiseMap(mapArray, scale);
        //resourceArray = generateNoiseMap(resourceArray, scale); //Unused for now.

        adjustCamera();
        createNodeMap();
        displayMap();

    }

    float[,] generateNoiseMap(float[,] array, float scale)
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)

            {
                float x = (float)i;
                float y = (float)j;

                float iPerlin = (x /mapSize * scale)+xOffset;
                float jPerlin = (y /mapSize * scale)+yOffset;
                float noiseValue = Mathf.PerlinNoise(iPerlin, jPerlin);
                
                array[i,j] = noiseValue;
            }
        }
        return array;
    }
    void adjustCamera()
    {
        GameObject camera = this.gameObject.transform.GetChild(0).gameObject;

        camera.transform.position = new Vector3((mapSize * 10 / 2), (mapSize*10), (mapSize * 10 / 2));
        camera.transform.Rotate(90, 0, 0, Space.Self);
    }

    void createNodeMap()
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)
            {
                float x = (float)i;
                float z = (float)j;

                float xOffset = (x * 10f);
                float zOffset = (z * 10f);

                Vector3 position = new Vector3(xOffset, 0, zOffset);
                //Creates multiple gameobjects. Set mapSize to small initial values for performance. Probably can do this a better way but this is for testing.
                nodes[i,j] = Instantiate(node, position, Quaternion.AngleAxis(0, Vector3.left)) as GameObject;
                nodes[i,j].transform.parent = this.transform;
                nodes[i,j].name = "Node(" + i+ "," + j+")";

                nodes[i, j].GetComponent<NodeData>().setNoiseValue(mapArray[i, j]);
            }
        }
    }

    public void updatePosition(int xPos, int zPos)
    {
        Material material = nodes[xPos, zPos].GetComponent<Renderer>().material;
        material.SetColor("_Color", Color.red);
    }

    void displayMap()
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)
            {
                //Adds the color to each game tile. 
                float noiseValue = mapArray[i,j];
                Color newColor = new Color(noiseValue, noiseValue, noiseValue);
                Material material = nodes[i,j].GetComponent<Renderer>().material;
                material.SetColor("_Color", newColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (xLastOffset != xOffset || yLastOffset != yOffset || lastScale != scale)
        {
            mapArray = generateNoiseMap(mapArray, scale);
            displayMap();
            xLastOffset = xOffset;
            yLastOffset = yOffset;
            lastScale = scale;
        }
    }
}
