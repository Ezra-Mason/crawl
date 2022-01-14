using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pose 
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public Pose(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
