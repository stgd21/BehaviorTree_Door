using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Node start;
    public Node goal;
    IGraph myGraph;

    PathFollow myMoveType;
    LookWhereGoing myRotateType;
    SteeringOutput steeringUpdate;
    Kinematic kinematic;

    //public GameObject[] myPath = new GameObject[4];
    Kinematic[] myPath;

    // Start is called before the first frame update
    void Start()
    {
        kinematic = GetComponent<Kinematic>();
        myRotateType = new LookWhereGoing();
        myRotateType.character = kinematic;

        myGraph = new GraphTerrain();
        myGraph.Build();
        List<Connection> path = Dijkstra.pathfind(myGraph, start, goal);
        // path is a list of connections - convert this to gameobjects for the FollowPath steering behavior
        myPath = new Kinematic[path.Count + 1];
        int i = 0;
        foreach (Connection c in path)
        {
            Debug.Log("from " + c.getFromNode() + " to " + c.getToNode() + " @" + c.getCost());
            myPath[i] = c.getFromNode().GetComponent<Kinematic>();
            i++;
        }
        myPath[i] = goal.GetComponent<Kinematic>();

        myMoveType = new PathFollow();
        myMoveType.character = kinematic;
        myMoveType.path = myPath;
    }

    // Update is called once per frame
    void Update()
    {
        steeringUpdate = new SteeringOutput();
        //GetComponent<Kinematic>().kRotation = myRotateType.GetSteering().angular;
        //steeringUpdate.angular = myRotateType.GetSteering().angular;
        //GetComponent<Kinematic>().kVelocity = myMoveType.GetSteering().linear;
        steeringUpdate.linear = myMoveType.GetSteering().linear;

        kinematic.GetData(steeringUpdate);
    }

}