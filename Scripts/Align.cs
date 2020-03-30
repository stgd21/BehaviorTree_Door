using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehavior
{
    public Kinematic character;
    public Kinematic target;

    float maxAngularAcceleration = 60f;
    float maxRotation = 60f;

    //Radius for arriving at target
    float targetRadius = 0.1f;

    //Radius for beginning to slow down
    float slowRadius = 30f;

    //Time over which to acheive target speed
    float timeToTarget = 0.1f;

    public virtual float getAngle()
    {
        return target.kOrientation;
    }

    public override SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();
        float targetRotation;

        //Get naive direction to target and map result to (-pi, pi) interval
        float rotation = Mathf.DeltaAngle(
            character.kOrientation,
            getAngle());
        float rotationSize = Mathf.Abs(rotation);

        //Check if we are there, return no steering
        if (rotationSize < targetRadius)
        {
            character.kRotation = 0f;
            return null;
        }

        //If we are outside slowRadius, use maximum rotation
        if (rotationSize > slowRadius)
            targetRotation = maxRotation;
        //Otherwise calculate scaled rotation
        else
            targetRotation = maxRotation * rotationSize / slowRadius;

        //The final target rotation combines speed (already in variable) and direction
        targetRotation *= rotation / rotationSize;

        //Acceleration tries to get to the target rotation
        result.angular = targetRotation - character.kRotation;
        result.angular /= timeToTarget;

        //Check if acceleration is too great
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        result.linear = Vector3.zero;
        return result;
    }

}
