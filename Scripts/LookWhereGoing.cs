using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereGoing : Align
{
    float angle; 

    public override float getAngle()
    {
        return angle;
    }

    public override SteeringOutput GetSteering()
    {
        //1. Calculate target to delegate to align
        Vector3 velocity = character.kVelocity;
        if (velocity.magnitude == 0)
            return null;

        //Otherwise set target based on velocity
        angle = Mathf.Atan2(velocity.x, velocity.z);
        angle *= Mathf.Rad2Deg;
        //target.transform.eulerAngles = new Vector3(0, angle, 0);
        return base.GetSteering();

    }
}