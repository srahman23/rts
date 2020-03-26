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

    public MapGen mapGen;
    void Start()
    {
        mapGen = this.GetComponent<MapGen>();
    }

    // Update is called once per frame
    void Update()
    {
        //If right-clicked
        if (Input.GetButtonDown("Fire1"))
        {

            Vector3 clickPos;

            /*
             * Creates a plane that is aligned with the terrain. 
             * We then raycast and check the distance from mouse pointer to the plane.
             * The distance also happens to be the distance from mouse pointer to the terrain as long as the terrain is flat.
             */

            Plane plane = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If the ray collides with the plane, it will output the ray to distanceToPlane
            float distanceToPlane; 
            if(plane.Raycast(ray, out distanceToPlane))
            {

                clickPos = ray.GetPoint(distanceToPlane);
                print(clickPos);
                updateMap(clickPos);
            }
            
        }
    }
    void updateMap(Vector3 position)
    {
        //Rounds the world coordinates to the nearest tile value before sending to MapGen.
        int xPos = (int)Math.Round(position.x / 10.0);
        int zPos = (int)Math.Round(position.z / 10.0);
       
        //Debug line if you want to check.
        //print(xPos +" "+zPos);

        mapGen.updatePosition(xPos, zPos);
    }
}
