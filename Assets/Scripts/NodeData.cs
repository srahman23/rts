using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data holding class for later use.
public class NodeData : MonoBehaviour
{
    private float noiseValue;
    private bool occupied;

    public float getNoiseValue()
    {
        return noiseValue;
    }
    public void setNoiseValue(float value)
    {
        this.noiseValue = value;
    }

    public bool getOccupied()
    {
        return occupied;
    }
    public void setOccupied(bool occupied)
    {
        this.occupied = occupied;
    }

}
