using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTerrain : Graph
{
    float mudMultiplier = 2f;
    public override void GetCost(Node[] nodes)
    {
        RaycastHit hit;
        foreach (Node fromNode in nodes)
        {
            foreach (Node toNode in fromNode.ConnectsTo)
            {
                float cost = (toNode.transform.position - fromNode.transform.position).magnitude;
                if (Physics.Raycast(toNode.transform.position, Vector3.down, out hit))
                {
                    if (hit.transform.tag == "mud")
                    {
                        Debug.Log("mud detected");
                        cost *= mudMultiplier;
                    }
                }
                Connection c = new Connection(cost, fromNode, toNode);
                mConnections.Add(c);
            }
        }
    }
}
