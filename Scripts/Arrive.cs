using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehavior
{
    public Kinematic character;
    public Kinematic target;

    float maxAcceleration = 5f;
    float maxSpeed = 7f;

    //Radius for arriving at target
    float targetRadius = 0.25f;

    //Radius for beginning to slow down
    float slowRadius = 8f;

    //Time over which to achieve target speed
    float timeToTarget = 0.1f;

    public override SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();
        float targetSpeed;

        //Get direction to target
        Vector3 direction = target.kPosition - character.kPosition;
        float distance = direction.magnitude;

        //Check if we are there, return no steering
        if (distance < targetRadius)
        {
            character.kVelocity = Vector3.zero;
            character.kRotation = 0f;
            return null;
        }


        //If we are outside slowRadius, move at max speed
        if (distance > slowRadius)
            targetSpeed = maxSpeed;
        //Otherwise calculate a scaled speed
        else
            targetSpeed = maxSpeed * distance / slowRadius;

        //Target velocity combines speed and direction
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        //Acceleration tries to get to target velocity
        result.linear = targetVelocity - character.kVelocity;
        result.linear /= timeToTarget;

        //Check if acceleration is too fast
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }

        result.angular = 0;
        return result;

    }
}
