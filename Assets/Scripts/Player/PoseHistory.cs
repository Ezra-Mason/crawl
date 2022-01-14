using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseHistory : MonoBehaviour
{
    public List<Pose> History;

    // Start is called before the first frame update
    void Start()
    {
        History = new List<Pose>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        History.Add(new Pose(transform.position, transform.rotation));
    }

    public void ResetList()
    {
        History.Clear();
        UpdateList();
    }

    public void RemoveFirst()
    {
        History.RemoveAt(0);
    }
}
