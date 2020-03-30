using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance
{
    public Kinematic character;
    float maxAcceleration = 4f;

    public Kinematic[] targets;

    float radius = 0.2f; //our own collision radius


    public SteeringOutput GetSteering()
    {
        //1. See if there's impending danger
        float shortestTime = float.PositiveInfinity;
        Kinematic firstTarget = null;
        float firstMinSeperation = 0;
        float firstDistance = 0;
        Vector3 firstRelativePosition = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;
        Vector3 relativePos;

        foreach (Kinematic target in targets)
        {
            //Calculate time to collision
            relativePos = target.kPosition - character.kPosition;
            Vector3 relativeVelocity = character.kVelocity - target.kVelocity;
            //Vector3 relativeVelocity = target.kVelocity - character.kVelocity;
            float relativeSpeed = relativeVelocity.magnitude;
            float timeToCollision = Vector3.Dot(relativePos, relativeVelocity) / (relativeSpeed * relativeSpeed);

            //Is it close enough to care?
            float distance = relativePos.magnitude;
            float minSeperation = distance - relativeSpeed * timeToCollision;
            if (minSeperation > 2 * radius)
            {
                continue;
            }

            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeperation = minSeperation;
                firstDistance = distance;
                firstRelativePosition = relativePos;
                firstRelativeVel = relativeVelocity;
            }
        }

        if (firstTarget == null)
        {
            Debug.Log("SAFE: No impending collisions");
            return null;
        }

        Debug.Log("Danger of collision");
        if (firstMinSeperation <= 0 || firstDistance < 2 * radius)
        {
            relativePos = firstTarget.kPosition - character.kPosition;
        }
        else
        {
            relativePos = -firstTarget.kVelocity;
        }

        relativePos = relativePos.normalized;

        SteeringOutput result = new SteeringOutput();
        result.linear = relativePos * maxAcceleration;
        result.angular = 0;
        return result;
    }
}
