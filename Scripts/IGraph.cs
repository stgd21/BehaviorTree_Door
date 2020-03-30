using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraph
{
    void GetCost(Node[] nodes);
    void Build();
    List<Connection> getConnections(Node fromNode);
}
