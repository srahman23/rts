using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 clickPos = new Vector3(0, 0, 0);
            Plane plane = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

        int xPos = (int)Math.Round(position.x / 10.0);
        int zPos = (int)Math.Round(position.z / 10.0);
       
        print(xPos +" "+zPos);
        mapGen.updatePosition(xPos, zPos);
    }
}
