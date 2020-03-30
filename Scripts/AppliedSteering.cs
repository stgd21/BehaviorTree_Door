using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SteeringType
{
    None,
    Seek,
    Flee,
    Pursue,
    Evade,
    FollowPath,
    Seperation,
    Arrive,
    CollisionAvoidance,
    ObstacleAvoidance,
    Flocking
}

public enum LookType
{
    None,
    Align,
    Face,
    LookWhereGoing
}

public class AppliedSteering : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity;
    public Kinematic target;
    public SteeringType moveType;
    public LookType lookType;

    //Kinematic holds data about our agent
    private Kinematic kinematic;

    //For Pathfollow
    public Kinematic[] pathOfObjects;

    //For Seperation
    public Kinematic[] seperateObstacles;

    //For collisionAvoidance
    public Kinematic[] collisionTargets;

    private PathFollow followAI;
    private Pursue pursueAI;
    private Seek seekAI;
    private Flee fleeAI;
    private Evade evadeAI;
    private Seperation seperationAI;
    private Arrive arriveAI;
    private CollisionAvoidance avoidAI;
    private ObstacleAvoidance obstacleAI;
    private Flocker flockAI;

    private Align alignAI;
    private Face faceAI;
    private LookWhereGoing lookAI;

    private SteeringOutput lookSteering;

    private void Start()
    {
        kinematic = GetComponent<Kinematic>();

        switch (moveType)
        {
            case SteeringType.Pursue:
                pursueAI = new Pursue();
                pursueAI.character = kinematic;
                pursueAI.target = target;
                break;
            case SteeringType.Evade:
                evadeAI = new Evade();
                evadeAI.character = kinematic;
                evadeAI.target = target;
                break;
            case SteeringType.FollowPath:
                followAI = new PathFollow();
                followAI.character = kinematic;
                followAI.path = pathOfObjects;
                break;
            case SteeringType.Seek:
                seekAI = new Seek();
                seekAI.character = kinematic;
                seekAI.target = target;
                break;
            case SteeringType.Flee:
                fleeAI = new Flee();
                fleeAI.character = kinematic;
                fleeAI.target = target;
                break;
            case SteeringType.Seperation:
                seperationAI = new Seperation();
                seperationAI.character = kinematic;
                seperationAI.targets = seperateObstacles;
                break;
            case SteeringType.Arrive:
                arriveAI = new Arrive();
                arriveAI.character = kinematic;
                arriveAI.target = target;
                break;
            case SteeringType.CollisionAvoidance:
                avoidAI = new CollisionAvoidance();
                avoidAI.character = kinematic;
                avoidAI.targets = collisionTargets;
                break;
            case SteeringType.ObstacleAvoidance:
                obstacleAI = new ObstacleAvoidance();
                obstacleAI.character = kinematic;
                break;
            case SteeringType.Flocking:
                flockAI = new Flocker();
                break;
            case SteeringType.None:
                break;
        }

        switch (lookType)
        {
            case LookType.Align:
                alignAI = new Align();
                alignAI.character = kinematic;
                alignAI.target = target;
                break;
            case LookType.Face:
                faceAI = new Face();
                faceAI.character = kinematic;
                faceAI.target = target;
                break;
            case LookType.LookWhereGoing:
                lookAI = new LookWhereGoing();
                lookAI.character = kinematic;
                break;
        }
    }
    void Update()
    {
        SteeringOutput movementSteering;
        //SteeringOutput lookSteering;
        //Update position and rotation
        transform.position += linearVelocity * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angularVelocity * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;

        switch (moveType)
        {
            case SteeringType.Pursue:
                movementSteering = pursueAI.GetSteering();
                break;
            case SteeringType.Evade:
                movementSteering = evadeAI.GetSteering();
                break;
            case SteeringType.FollowPath:
                movementSteering = followAI.GetSteering();
                break;
            case SteeringType.Seek:
                movementSteering = seekAI.GetSteering();
                break;
            case SteeringType.Flee:
                movementSteering = fleeAI.GetSteering();
                break;
            case SteeringType.Seperation:
                movementSteering = seperationAI.GetSteering();
                break;
            case SteeringType.Arrive:
                movementSteering = arriveAI.GetSteering();
                break;
            case SteeringType.CollisionAvoidance:
                movementSteering = avoidAI.GetSteering();
                break;
            case SteeringType.ObstacleAvoidance:
                movementSteering = obstacleAI.GetSteering();
                break;
            default:
                movementSteering = new SteeringOutput();
                break;
        }
  
        if (movementSteering != null)
        {
            linearVelocity += movementSteering.linear * Time.deltaTime;
            //angularVelocity += movementSteering.angular * Time.deltaTime;
        }

        switch (lookType)
        {
            case LookType.None:
                break;
            case LookType.Align:
                lookSteering = alignAI.GetSteering();
                break;
            case LookType.Face:
                lookSteering = faceAI.GetSteering();
                break;
            case LookType.LookWhereGoing:
                lookSteering = lookAI.GetSteering();
                break;
            default:
                lookSteering = alignAI.GetSteering();
                break;
        }

        if (lookSteering != null)
        {
            angularVelocity += lookSteering.angular * Time.deltaTime;
        }
        //Update kinematic reference with complex data it can't get by itself
        kinematic.GetData(movementSteering);
        kinematic.GetData(lookSteering);
    }
}

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;
}
