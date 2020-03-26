using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Allows map editing with mouse clicks.
 * Only paints the clicked tile red for now. Can be expanded for building placement and other uses.
 */
public class NodeEdit : MonoBehaviour
{

    MapGen mapGen;
    Plane plane;
    int xPosOld, zPosOld;
    bool initialPos;
    void Start()
    {
        mapGen = this.GetComponent<MapGen>(); 
        plane = new Plane(Vector3.up, 0f);
        initialPos = false;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            Vector3 clickPos;
            /*
             * Creates a plane that is aligned with the terrain. 
             * We then raycast and check the distance from mouse pointer to the plane.
             * The distance also happens to be the distance from mouse pointer to the terrain as long as the terrain is flat.
             */

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //If the ray collides with the plane, it will output the ray to distanceToPlane
            float distanceToPlane;
            if (plane.Raycast(ray, out distanceToPlane))
            {
                clickPos = ray.GetPoint(distanceToPlane);

                //Rounds the world coordinates to the nearest tile value before sending to MapGen.
                int xPos = (int)Math.Round(clickPos.x / 10.0);
                int zPos = (int)Math.Round(clickPos.z / 10.0);

                if (!initialPos || (xPosOld != xPos && zPosOld != zPos))
                {
                    updateMap(xPos, zPos);
                }
                //If right-clicked, then record the position. This is the start of drag and holding.
                if (Input.GetButtonDown("Fire1"))
                {

                }
            }
            
        }

        //Prevents Unity from thorwing an exception when dragging cursor out of bounds.
        catch(IndexOutOfRangeException e)
        {
            if (e.Data == null)
            {
                throw;
            }
        }
        
    }
    void updateMap(int xPos, int zPos)
    {
        //Debug line if you want to check.
        //print(xPos +" "+zPos);

        //Inefficient way of resetting the map when cursor not on tile.
        //mapGen.displayMap()       

        //Better way is to update the single node instead of every node.
        GameObject oldNode = mapGen.nodes[xPosOld, zPosOld];
        GameObject newNode = mapGen.nodes[xPos, zPos];

        //Revert previous node.
        Material materialOld = oldNode.GetComponent<Renderer>().material;
        float noiseValue = oldNode.GetComponent<NodeData>().getNoiseValue();
        
        //Update new node.
        Color oldColor = new Color(noiseValue, noiseValue, noiseValue);
        materialOld.SetColor("_Color", oldColor);

        Material material = newNode.GetComponent<Renderer>().material;
        material.SetColor("_Color", Color.red);

        xPosOld = xPos;
        zPosOld = zPos;
    }
}
