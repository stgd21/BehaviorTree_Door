using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    public Kinematic target;

    float maxPrediction = 4f;
    float prediction;
    Kinematic targetWithOffset;
    

    public override SteeringOutput GetSteering()
    {
        Vector3 direction = target.kPosition - character.kPosition;
        float distance = direction.magnitude;
        float speed = character.kVelocity.magnitude;

        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }
        targetWithOffset = target;
        targetWithOffset.kPosition += target.kVelocity * prediction;
        base.target = targetWithOffset;
        return base.GetSteering();
    }
}
