using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee
{
    public Kinematic character;
    public Kinematic target;

    private float maxAcceleration = 4f;

    public virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();

        //Get direction to target
        result.linear = character.kPosition - target.kPosition;
        result.linear = result.linear.normalized;
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }
}
